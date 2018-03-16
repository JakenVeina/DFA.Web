using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.ViewModels;

namespace DFA.Web.Api.Authentication
{
    public class MissingCredentialViewModel : ErrorViewModel
    {
        /**********************************************************************/
        #region Constructors

        public MissingCredentialViewModel()
        {
            Error = "missing_credential";
        }

        #endregion Constructors
    }
}
