using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DFA.Common.Extensions;

namespace DFA.Web.Configuration
{
    public class AuthenticationSchemeConfiguration : ConfigurationBase
    {
        /**********************************************************************/
        #region Properties

        public string Name { get; set; }

        public string TokenIssuer { get; set; }

        public string TokenLifetime { get; set; }

        public string SigningKey { get; set; }

        public string SigningAlgorithm { get; set; }

        public string TokenAudience { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region ConfigurationBase

        public override void Validate(string optionName = null)
        {
            RequireNonEmptyString(Name, nameof(Name), optionName);
            RequireNonEmptyString(TokenIssuer, nameof(TokenIssuer), optionName);
            RequireTimeSpan(TokenLifetime, nameof(TokenLifetime), optionName);
            RequireBase64String(SigningKey, nameof(SigningKey), optionName);
            RequireNonEmptyString(SigningAlgorithm, nameof(SigningAlgorithm), optionName);
            RequireNonEmptyString(TokenAudience, nameof(TokenAudience), optionName);
        }

        #endregion ConfigurationBase
    }
}
