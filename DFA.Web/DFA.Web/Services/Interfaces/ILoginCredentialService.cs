using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using DFA.Web.Models.Entities;

namespace DFA.Web.Services.Interfaces
{
    public interface ILoginCredentialService
    {
        /**********************************************************************/
        #region Methods

        LoginCredential CreateCredential(string password);

        bool ValidateCredential(string password, LoginCredential credential);

        #endregion Methods
    }
}
