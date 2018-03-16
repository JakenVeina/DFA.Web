using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Api.Authentication
{
    public class LoginRequest
    {
        /**********************************************************************/
        #region Properties

        [Required]
        [MinLength(1)]
        public string UserName { get; set; }

        [Required]
        [MinLength(1)]
        public string Password { get; set; }

        #endregion Properties
    }
}
