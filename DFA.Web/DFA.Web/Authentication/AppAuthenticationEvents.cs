using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper.QueryableExtensions;

using DFA.Common.Extensions;
using DFA.Web.Models;
using DFA.Web.Models.Entities;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Authentication
{
    public class AppAuthenticationEvents : JwtBearerEvents
    {
        /**********************************************************************/
        #region JwtBearerEvents

        public override Task Challenge(JwtBearerChallengeContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.HttpContext != null);
            Contract.Requires(context.HttpContext.RequestServices != null);

            var deferredActionService = context.HttpContext.RequestServices.GetRequiredService<IDeferredActionService>();

            var logData = new
            {
                Exception = context.AuthenticateFailure,
                Headers = context.Request.Headers,
                Method = context.Request.Method,
                Query = context.Request.Query
            };

            deferredActionService.AddAction<ILoggingService>(loggingService
                => loggingService.CreateEntry(LogLevelType.Warning, "AppAuthenticationEvents.Challenge", logData));

            return base.Challenge(context);
        }

        public override async Task TokenValidated(TokenValidatedContext context)
        {
            Contract.Requires(context != null);
            Contract.Requires(context.HttpContext != null);
            Contract.Requires(context.HttpContext.RequestServices != null);

            var serviceProvider = context.HttpContext.RequestServices;
            var authenticationService = serviceProvider.GetRequiredService<IAppAuthenticationService>();

            var token = context.SecurityToken.Cast<JwtSecurityToken>();
            var userClaims = authenticationService.DecodeToken(token);

            var dataContext = serviceProvider.GetRequiredService<DFAWebDataContext>();
            var nonce = await dataContext
                .Users
                .Where(x => x.Id == userClaims.Id)
                .Select(x => x.LoginNonce)
                .FirstOrDefaultAsync();

            if (userClaims.LoginNonce != nonce)
            {
                context.Fail("The session has expired");
                return;
            }
            authenticationService.CurrentUserClaims = userClaims;

            var user = new User()
            {
                Id = userClaims.Id,
                LastActive = DateTime.UtcNow
            };
            var deferredActionService = context.HttpContext.RequestServices.GetRequiredService<IDeferredActionService>();
            deferredActionService.AddAction<DFAWebDataContext>(async deferredDataContext
                => deferredDataContext.UpdateEntityProperty(user, x => x.LastActive));

            await base.TokenValidated(context);
        }

        #endregion JwtBearerEvents
    }
}
