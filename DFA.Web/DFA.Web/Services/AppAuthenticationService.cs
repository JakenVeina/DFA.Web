using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using IdentityModel.Client;

using DFA.Common.Extensions;

using DFA.Web.Authentication;
using DFA.Web.Configuration;
using DFA.Web.Mapping;
using DFA.Web.Models.Entities;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Services
{
    public class AppAuthenticationService : IAppAuthenticationService
    {
        /**********************************************************************/
        #region Constants

        public const string IdClaimType = "sub";

        public const string NonceClaimType = "jti";

        public const string RoleClaimType = "rls";

        #endregion Constants

        /**********************************************************************/
        #region Constructors

        public UserClaimsViewModel CurrentUserClaims { get; set; }

        public AppAuthenticationService(AppConfiguration configuration, DFAWebDataContext dataContext)
        {
            Contract.Requires(configuration != null);
            Contract.Requires(dataContext != null);

            _dataContext = dataContext;

            _schemeConfiguration = configuration.Authentication.Schemes.First(x => x.Name == configuration.Authentication.DefaultScheme);

            _tokenLifetime = _schemeConfiguration.TokenLifetime.ToTimeSpan();

            _signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_schemeConfiguration.SigningKey.ToBase64ByteArray()),
                _schemeConfiguration.SigningAlgorithm);
        }

        #endregion Constructors

        /**********************************************************************/
        #region ITokenService

        public async Task<(ExternalLoginPolicy externalLoginPolicy, DiscoveryResponse discoveryResponse, string errorDescription)> TryDiscoverExternalLoginDetails(string policyName)
        {
            var policy = await _dataContext
                .ExternalLoginPolicies
                .FirstOrDefaultAsync(x => string.Compare(x.Name, policyName, StringComparison.OrdinalIgnoreCase) == 0);

            if (policy == null)
                return (null, null, null);

            using (var client = new DiscoveryClient(policy.Authority)
            {
                Policy = policy.MapTo<DiscoveryPolicy>()
            })
            {
                var response = await client.GetAsync();

                if (response.IsError)
                    return (policy, null, response.Error);

                if (!response.ScopesSupported.Any(x => string.Compare(x, "openid", StringComparison.OrdinalIgnoreCase) == 0))
                    return (policy, null, $"Authentication authrority {policy.Authority} does not support the OAuth2 scope \"openid\"");

                if (!response.ResponseTypesSupported.Any(x => string.Compare(x, "code", StringComparison.OrdinalIgnoreCase) == 0))
                    return (policy, null, $"Authentication authrority {policy.Authority} does not support the OAuth2 response type \"code\"");

                if (!response.ClaimsSupported.Any(x => string.Compare(x, "sub", StringComparison.OrdinalIgnoreCase) == 0))
                    return (policy, null, $"Authentication authrority {policy.Authority} does not support the OpenIDConnect claim \"sub\"");

                return (policy, response, null);
            }
        }

        public JwtSecurityToken EncodeToken(UserClaimsViewModel viewModel)
            => new JwtSecurityToken(
                issuer: _schemeConfiguration.TokenIssuer,
                audience: _schemeConfiguration.TokenAudience,
                claims: Enumerable.Empty<Claim>()
                    .Append(new Claim(IdClaimType, viewModel.Id.ToString()))
                    .Append(new Claim(NonceClaimType, viewModel.LoginNonce.ToString()))
                    .Concat(viewModel.RoleNames.Select(x => new Claim(RoleClaimType, x))),
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow + _tokenLifetime,
                signingCredentials: _signingCredentials);

        public UserClaimsViewModel DecodeToken(JwtSecurityToken token)
            => new UserClaimsViewModel()
            {
                Id = token.Subject
                    .ToInt32(),
                LoginNonce = token.Claims
                    .First(x => x.Type == NonceClaimType)
                    .Value
                    .ToInt32(),
                RoleNames = token.Claims
                    .Where(x => x.Type == RoleClaimType)
                    .Select(x => x.Value)
                    .ToHashSet()
            };

        #endregion ITokenService

        /**********************************************************************/
        #region Private Fields

        private readonly DFAWebDataContext _dataContext;

        private readonly AuthenticationSchemeConfiguration _schemeConfiguration;

        private readonly TimeSpan _tokenLifetime;

        private readonly SigningCredentials _signingCredentials;

        #endregion Private Fields
    }
}
