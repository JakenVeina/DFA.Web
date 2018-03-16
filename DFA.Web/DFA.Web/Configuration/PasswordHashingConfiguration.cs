using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace DFA.Web.Configuration
{
    public class PasswordHashingConfiguration : ConfigurationBase
    {
        /**********************************************************************/
        #region Properties

        public string Algorithm { get; set; }

        public string Iterations { get; set;  }

        public string SaltByteCount { get; set; }

        public string HashByteCount { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region ConfigurationBase

        public override void Validate(string optionName = null)
        {
            RequireEnumValue<KeyDerivationPrf>(Algorithm, nameof(Algorithm), optionName);
            RequirePositiveNonZeroInteger(Iterations, nameof(Iterations), optionName);
            RequirePositiveNonZeroInteger(SaltByteCount, nameof(SaltByteCount), optionName);
            RequirePositiveNonZeroInteger(HashByteCount, nameof(HashByteCount), optionName);
        }

        #endregion ConfigurationBase
    }
}
