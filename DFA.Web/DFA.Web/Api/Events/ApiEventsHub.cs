using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using DFA.Common.Extensions;

using DFA.Web.Models;
using DFA.Web.Services.Interfaces;

namespace DFA.Web.Api.Events
{
    public class ApiEventsHub : Hub
    {
        /**********************************************************************/
        #region Construction

        public ApiEventsHub(IApiEventsService apiEventsService, ILoggingService loggingService)
        {
            Contract.Requires(apiEventsService != null);
            Contract.Requires(loggingService != null);

            ApiEventsService = apiEventsService;
            LoggingService = loggingService;
        }

        #endregion Construction

        /**********************************************************************/
        #region Hub Overrides

        public override async Task OnConnectedAsync()
        {
            var subscriptionTokenParamName = "token";

            var httpContext = Context.Connection.GetHttpContext();
            var token = httpContext.Request.Query[subscriptionTokenParamName];
            var subscriptionToken = token.ToString();

            if (subscriptionToken == null)
                throw new ArgumentNullException(subscriptionTokenParamName);

            if (!subscriptionToken.All(char.IsLetterOrDigit))
            {
                // An Attempt to use invalid characters might be an attempt at code injection
                await LoggingService.CreateEntry(
                    LogLevelType.Warning,
                    $"{nameof(ApiEventsHub)}.{nameof(OnConnectedAsync)}.BadToken",
                    new
                    {
                        subscriptionToken = subscriptionToken
                    });

                throw new ArgumentException($"{subscriptionTokenParamName} may only contain alphanumeric characters");
            }

            if (subscriptionToken.Length < 32)
                throw new ArgumentException($"{subscriptionTokenParamName} must be at lesat 32 characters in length");

            if (!ApiEventsService.RegisterSubscriptionToken(Context.ConnectionId, subscriptionToken))
                throw new ArgumentException($"{subscriptionTokenParamName} \"{subscriptionToken}\" is already in use");
             
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ApiEventsService.CancelSubscriptionToken(Context.ConnectionId);

            if (exception != null)
                LoggingService.CreateEntry(
                    LogLevelType.Error,
                    $"{nameof(ApiEventsHub)}.{nameof(OnDisconnectedAsync)}.Exception",
                    exception);

            await base.OnDisconnectedAsync(exception);
        }

        #endregion Hub Overrides

        /**********************************************************************/
        #region Protected Properties

        internal protected IApiEventsService ApiEventsService { get; }

        internal protected ILoggingService LoggingService { get; }

        #endregion Protected Properties
    }
}
