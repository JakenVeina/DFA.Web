using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.Responses;

namespace DFA.Web.Api.Authentication
{
    public class ExternalLoginPolicyResponse
    {
        /**********************************************************************/
        #region Properties

        public string AuthorizationEndpoint { get; set; }

        public string ClientId { get; set; }

        #endregion Properties
    }
}
