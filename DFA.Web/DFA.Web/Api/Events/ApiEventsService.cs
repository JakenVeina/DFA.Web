using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.SignalR;

using DFA.Common.Extensions;

namespace DFA.Web.Api.Events
{
    public class ApiEventsService : IApiEventsService
    {
        /**********************************************************************/
        #region Construction

        public ApiEventsService(IHubContext<ApiEventsHub> apiEventsHubContext)
        {
            Contract.Requires(apiEventsHubContext != null);

            ApiEventsHubContext = apiEventsHubContext;
        }

        #endregion Construction

        /**********************************************************************/
        #region IEventsService

        public bool RegisterSubscriptionToken(string connectionId, string subscriptionToken)
        {
            Contract.Requires(!connectionId.IsNullOrWhitespace());
            Contract.Requires(!subscriptionToken.IsNullOrWhitespace());

            return _connectionIdsBySubscriptionToken.TryAdd(subscriptionToken, connectionId);
        }

        public bool CancelSubscriptionToken(string connectionId)
        {
            Contract.Requires(!connectionId.IsNullOrWhitespace());

            return _connectionIdsBySubscriptionToken.TryRemove(connectionId, out _);
        }

        public async Task<bool> AddSubscription(string subscriptionToken, string name)
        {
            Contract.Requires(!subscriptionToken.IsNullOrWhitespace());
            Contract.Requires(!name.IsNullOrWhitespace());

            if (!_connectionIdsBySubscriptionToken.TryGetValue(subscriptionToken, out var connectionId))
                return false;

            await ApiEventsHubContext.Groups.AddAsync(connectionId, name.ToLower());

            return true;
        }

        public async Task<bool> RemoveSubscription(string subscriptionToken, string name)
        {
            Contract.Requires(!subscriptionToken.IsNullOrWhitespace());
            Contract.Requires(!name.IsNullOrWhitespace());

            if (!_connectionIdsBySubscriptionToken.TryGetValue(subscriptionToken, out var connectionId))
                return false;

            await ApiEventsHubContext.Groups.RemoveAsync(connectionId, name.ToLower());

            return true;
        }

        public Task RaiseEvent(string name, object data)
        {
            Contract.Requires(!name.IsNullOrWhitespace());

            var normalizedName = name.ToLower();

            return ApiEventsHubContext
                .Clients
                .Group(normalizedName)
                .SendAsync(nameof(RaiseEvent), normalizedName, data);
        }

        #endregion IEventsService

        /**********************************************************************/
        #region Protected Properties

        internal protected IHubContext<ApiEventsHub> ApiEventsHubContext { get; }

        #endregion Protected Properties

        /**********************************************************************/
        #region Private Fields

        private readonly ConcurrentDictionary<string, string> _connectionIdsBySubscriptionToken
            = new ConcurrentDictionary<string, string>();

        #endregion Private Fields
    }
}
