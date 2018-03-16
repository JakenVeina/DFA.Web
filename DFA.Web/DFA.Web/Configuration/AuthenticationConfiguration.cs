using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Configuration
{
    public class AuthenticationConfiguration : ConfigurationBase
    {
        /**********************************************************************/
        #region Properties

        public string DefaultScheme { get; set; }

        public ICollection<AuthenticationSchemeConfiguration> Schemes { get; }
            = new List<AuthenticationSchemeConfiguration>();

        #endregion Properties

        /**********************************************************************/
        #region Methods

        public override void Validate(string optionName = null)
        {
            RequireNonEmptyString(DefaultScheme, nameof(DefaultScheme), optionName);

            RequireOption(Schemes, nameof(Schemes), optionName);
            ValidateCollection(Schemes, nameof(Schemes), optionName);
            ValidateOption(Schemes.Any(x => x.Name == DefaultScheme), nameof(Schemes), optionName, $"Must contain a scheme matching {nameof(DefaultScheme)}");
        }

        #endregion Methods
    }
}
