using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AutoMapper.QueryableExtensions;

using IdentityModel.Client;

using DFA.Common.Extensions;

using DFA.Web.Api.Events;
using DFA.Web.Api.User;
using DFA.Web.Authentication;
using DFA.Web.Mapping;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Models.Requests;
using DFA.Web.Models.ViewModels;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Api.Authentication
{
    [Route("api/auth")]
    public class AuthenticationController : ApiControllerBase
    {
        /**********************************************************************/
        #region Constructors

        public AuthenticationController(
            DFAWebDataContext dataContext,
            IApiEventsService apiEventsService,
            IDeferredActionService deferredActionService,
            ILoggingService loggingService,
            ILoginCredentialService loginCredentialService,
            IAppAuthenticationService appAuthenticationService)
            : base(dataContext, apiEventsService, deferredActionService, loggingService)
        {
            Contract.Requires(loginCredentialService != null);
            Contract.Requires(appAuthenticationService != null);

            LoginCredentialService = loginCredentialService;
            PppAuthenticationService = appAuthenticationService;

            JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        #endregion Constructors

        /**********************************************************************/
        #region Methods

        [HttpGet("policies")]
        [AllowAnonymous]
        public async Task<IActionResult> Policies()
            => Ok(DataContext
                .ExternalLoginPolicies
                .ProjectTo<ExternalLoginPolicyViewModel>()
                .ToListAsync());

        [HttpGet("policies/{policyname}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPolicy(string policyName)
        {
            if (policyName == "password")
                return NotFound();

            (var externalLoginPolicy, var discoveryResponse, var errorDescription) = await PppAuthenticationService.TryDiscoverExternalLoginDetails(policyName);

            if(externalLoginPolicy == null)
                return NotFound();

            if (discoveryResponse == null)
            {
                DeferLogEntry(LogLevelType.Warning, "DiscoveryError", discoveryResponse);
                return BadGateway(new ExternalLoginRequestErrorViewModel()
                {
                    Description = errorDescription
                });
            }

            return Ok(new ExternalLoginPolicyResponse()
            {
                AuthorizationEndpoint = discoveryResponse.AuthorizeEndpoint,
                ClientId = externalLoginPolicy.ClientId
            });
        }

        [HttpPost("policies/password/[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await DataContext
                .Users
                .Include(x => x.LoginCredential)
                .Include(x => x.UserRoleMaps).ThenInclude(x => x.Role)
                .FirstOrDefaultAsync(x => x.Name == request.UserName);

            if (user == null)
            {
                DeferLogEntry(LogLevelType.Warning, "InvalidUsername", request.UserName);
                return BadRequest(new InvalidCredentialViewModel());
            }

            if (user.LoginCredential == null)
            {
                DeferLogEntry(LogLevelType.Warning, "NoCredential", user);
                return BadRequest(new MissingCredentialViewModel()
                {
                    Description = $"User {user.Name} does not have a password login credential"
                });
            }

            if (user.LoginCredential.LockoutEnd.HasValue)
            {
                if (user.LoginCredential.LockoutEnd.Value > DateTime.UtcNow)
                {
                    user.LoginCredential.LockoutEnd = null;
                    DeferEntityUpdate(user.LoginCredential, x => x.LockoutEnd);
                    DeferLogEntry(LogLevelType.Information, "LockoutExpired", user);
                }
                else
                {
                    DeferLogEntry(LogLevelType.Warning, "LockoutApplied", user);
                    return Forbid(user.LoginCredential.MapTo<LockoutViewModel>());
                }
            }

            if (!LoginCredentialService.ValidateCredential(request.Password, user.LoginCredential))
            {
                ++user.LoginCredential.SuccessiveFailedLoginCount;
                DeferEntityUpdate(user.LoginCredential, x => x.SuccessiveFailedLoginCount);

                // TODO: Pull lockout count from config
                if (user.LoginCredential.SuccessiveFailedLoginCount >= 10)
                {
                    // TODO: Pull lockout duration from config
                    user.LoginCredential.LockoutEnd = DateTime.UtcNow.AddMinutes(30);
                    DeferEntityUpdate(user.LoginCredential, x => x.LockoutEnd);
                    DeferLogEntry(LogLevelType.Warning, "LockoutStarted", user);
                    return Forbid(user.LoginCredential.MapTo<LockoutViewModel>());
                }
                else
                {
                    DeferLogEntry(LogLevelType.Warning, "InvalidPassword", user);
                    return BadRequest(new InvalidCredentialViewModel());
                }
            }

            user.LoginCredential.SuccessiveFailedLoginCount = 0;
            DeferEntityUpdate(user.LoginCredential, x => x.SuccessiveFailedLoginCount);
            DeferLogEntry(LogLevelType.Information, "Success", user);
            return Ok(new LoginResponse()
            {
                Token = JwtSecurityTokenHandler.WriteToken(
                    PppAuthenticationService.EncodeToken(
                        user.MapTo<UserClaimsViewModel>()))
            });
        }

        [HttpPost("policies/{policyname}/[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken(string policyName, [FromBody] ExternalLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            (var externalLoginPolicy, var discoveryResponse, var errorDescription) = await PppAuthenticationService.TryDiscoverExternalLoginDetails(policyName);

            if (externalLoginPolicy == null)
                return NotFound();

            if (discoveryResponse == null)
            {
                DeferLogEntry(LogLevelType.Warning, "DiscoveryFailure", discoveryResponse);
                return BadGateway(new ExternalLoginRequestErrorViewModel()
                {
                    Description = errorDescription
                });
            }

            var tokenClient = new TokenClient(
                discoveryResponse.TokenEndpoint,
                externalLoginPolicy.ClientId,
                externalLoginPolicy.ClientSecret);

            var tokenResponse = await tokenClient.RequestAuthorizationCodeAsync(request.Code, request.RedirectUri);
            if (tokenResponse.IsError)
            {
                DeferLogEntry(LogLevelType.Warning, "AuthorizationFailure", tokenResponse);
                return BadGateway(new ExternalLoginRequestErrorViewModel()
                {
                    Description = $"{tokenResponse.Error}: {tokenResponse.ErrorDescription}"
                });
            }

            var subject = new JwtSecurityToken(tokenResponse.IdentityToken).Claims.First(x => x.Type == "sub").Value;

            var credential = await DataContext
                .ExternalLoginCredentials
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Subject == subject);

            if (credential == null)
            {
                DeferLogEntry(LogLevelType.Warning, "NoCredential", tokenResponse);
                return BadRequest(new MissingCredentialViewModel()
                {
                    Description = $"The chosen {externalLoginPolicy.Name} user does not have an account"
                });
            }

            DeferLogEntry(LogLevelType.Information, "Success", credential);
            return Ok(new LoginResponse()
            {
                Token = JwtSecurityTokenHandler.WriteToken(
                    PppAuthenticationService.EncodeToken(
                        credential.User.MapTo<UserClaimsViewModel>()))
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> InvalidateToken()
        {
            var nonceBytes = new byte[4];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(nonceBytes);
            }

            var user = new Models.Entities.User()
            {
                Id = PppAuthenticationService.CurrentUserClaims.Id,
                LoginNonce = nonceBytes.ToInt32()
            };

            DeferEntityUpdate(user, x => x.LoginNonce);
            DeferLogEntry(LogLevelType.Information, "Success", PppAuthenticationService.CurrentUserClaims);
            return Ok();
        }

        #endregion Methods

        /**********************************************************************/
        #region Protected Properties

        internal protected ILoginCredentialService LoginCredentialService { get; }

        internal protected IAppAuthenticationService PppAuthenticationService { get; }

        internal protected JwtSecurityTokenHandler JwtSecurityTokenHandler { get; }

        #endregion Protected Properties
    }
}
