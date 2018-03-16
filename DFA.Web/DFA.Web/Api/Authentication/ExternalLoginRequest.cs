using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Api.Authentication
{
    public class ExternalLoginRequest
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [MinLength(1)]
        public string Code { get; set; }

        [Required]
        [MinLength(1)]
        public string RedirectUri { get; set; }

        #endregion Properties
    }
}
