using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.ViewModels;

namespace DFA.Web.Api.Events
{
    public class InvalidSubscriptionTokenViewModel : ErrorViewModel
    {
        /**********************************************************************/
        #region Construction

        public InvalidSubscriptionTokenViewModel(string subscriptionToken)
        {
            Error = "InvalidSubscriptionToken";
            Description = $"\"{subscriptionToken}\" is not a registered subscription token";
        }

        #endregion Construction
    }
}
