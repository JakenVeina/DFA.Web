using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Web.Models.ViewModels;

namespace DFA.Web.Api.Authentication
{
    public class InvalidCredentialViewModel : ErrorViewModel
    {
        /**********************************************************************/
        #region Constructors

        public InvalidCredentialViewModel()
        {
            Error = "invalid_credential";
            Description = "The username or password was incorrect";
        }

        #endregion Constructors
    }
}
