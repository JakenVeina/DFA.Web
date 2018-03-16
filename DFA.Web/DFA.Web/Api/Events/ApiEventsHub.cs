using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using DFA.Common.Extensions;

namespace DFA.Web.Api.Events
{
    public class ApiEventsHub : Hub
    {
        /**********************************************************************/
        #region Construction

        public ApiEventsHub(IApiEventsService apiEventsService)
        {
            Contract.Requires(apiEventsService != null);

            ApiEventsService = apiEventsService;
        }

        #endregion Construction

        /**********************************************************************/
        #region Hub Overrides

        public override Task OnConnectedAsync()
        {
            var subscriptionTokenParamName = "token";

            var httpContext = Context.Connection.GetHttpContext();
            var token = httpContext.Request.Query[subscriptionTokenParamName];
            var subscriptionToken = token.ToString();

            if (subscriptionToken == null)
                throw new ArgumentNullException(subscriptionTokenParamName);

            // TODO: Log attempts to use non-numeric characters, possible exploit attempts
            if (!subscriptionToken.All(char.IsLetterOrDigit))
                throw new ArgumentException($"{subscriptionTokenParamName} may only contain alphanumeric characters");

            if (subscriptionToken.Length < 32)
                throw new ArgumentException($"{subscriptionTokenParamName} must be at lesat 32 characters in length");

            if (!ApiEventsService.RegisterSubscriptionToken(Context.ConnectionId, subscriptionToken))
                throw new ArgumentException($"{subscriptionTokenParamName} \"{subscriptionToken}\" is already in use");
             
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            ApiEventsService.CancelSubscriptionToken(Context.ConnectionId);

            // TODO: Log exception, if not null

            return base.OnDisconnectedAsync(exception);
        }

        #endregion Hub Overrides

        /**********************************************************************/
        #region Protected Properties

        internal protected IApiEventsService ApiEventsService { get; }

        #endregion Protected Properties
    }
}
