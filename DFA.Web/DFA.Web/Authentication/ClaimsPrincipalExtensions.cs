 using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Authentication.Cookies;

using DFA.Common.Extensions;
using DFA.Web.Models.Entities;

namespace DFA.Web.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public const string IdClaimType = "Id";

        public const string RoleClaimType = "Role";

        public const string NonceClaimType = "Nonce";

        /**********************************************************************/
        #region Extension Methods

        public static int? GetId(this ClaimsPrincipal claimsPrincipal)
            => claimsPrincipal?.Identity?.Name?.ToInt32();

        #endregion Extension Methods
    }
}
