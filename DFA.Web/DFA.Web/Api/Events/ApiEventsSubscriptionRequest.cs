using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.Validation;

namespace DFA.Web.Api.Events
{
    public class ApiEventsSubscriptionRequest
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [MinLength(32)]
        [Alphanumeric]
        public string SubscriptionToken { get; set; }

        #endregion Properties
    }
}
