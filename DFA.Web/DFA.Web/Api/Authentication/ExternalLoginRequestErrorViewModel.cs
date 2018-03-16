using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.ViewModels;

namespace DFA.Web.Api.Authentication
{
    public class ExternalLoginRequestErrorViewModel : ErrorViewModel
    {
        /**********************************************************************/
        #region Constructors

        public ExternalLoginRequestErrorViewModel()
        {
            Error = "external_login_request";
        }

        #endregion Constructors
    }
}
