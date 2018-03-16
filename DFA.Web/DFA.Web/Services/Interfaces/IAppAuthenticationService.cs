using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using IdentityModel.Client;

using DFA.Web.Authentication;
using DFA.Web.Models.Entities;

namespace DFA.Web.Services.Interfaces
{
    public interface IAppAuthenticationService
    {
        /**********************************************************************/
        #region Properties

        UserClaimsViewModel CurrentUserClaims { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region Methods

        Task<(ExternalLoginPolicy externalLoginPolicy, DiscoveryResponse discoveryResponse, string errorDescription)> TryDiscoverExternalLoginDetails(string policyName);

        JwtSecurityToken EncodeToken(UserClaimsViewModel viewModel);

        UserClaimsViewModel DecodeToken(JwtSecurityToken token);

        #endregion Methods
    }
}
