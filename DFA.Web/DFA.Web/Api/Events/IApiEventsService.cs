using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Api.Events
{
    public interface IApiEventsService
    {
        /**********************************************************************/
        #region Methods

        bool RegisterSubscriptionToken(string connectionId, string subscriptionToken);

        bool CancelSubscriptionToken(string connectionId);

        Task<bool> Subscribe(string subscriptionToken, string name);

        Task<bool> Unsubscribe(string subscriptionToken, string name);

        Task RaiseEvent(string name, object data);

        #endregion Methods
    }
}
