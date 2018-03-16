using System;

using DFA.Web.Mapping;
using DFA.Web.Models.Entities;
using DFA.Web.Models.ViewModels;

namespace DFA.Web.Api.Authentication
{
    [MapsFrom(typeof(LoginCredential))]
    public class LockoutViewModel : ErrorViewModel
    {
        /**********************************************************************/
        #region Constructors

        public LockoutViewModel()
        {
            Error = "lockout";
            Description = "The account has been locked.";
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        public DateTime LockoutEnd { get; set; }

        #endregion Properties
    }
}
