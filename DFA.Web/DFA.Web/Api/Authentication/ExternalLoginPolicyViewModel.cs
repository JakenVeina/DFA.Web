using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Api.Authentication
{
    [MapsFrom(typeof(ExternalLoginPolicy))]
    public class ExternalLoginPolicyViewModel
    {
        /**********************************************************************/
        #region Properties

        public string Name { get; set; }

        public string Authority { get; set; }

        public string ClientId { get; set; }

        #endregion Properties
    }
}
