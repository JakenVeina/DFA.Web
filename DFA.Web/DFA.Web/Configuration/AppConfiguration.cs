using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Configuration
{
    public class AppConfiguration : ConfigurationBase
    {
        /**********************************************************************/
        #region Properties

        public AuthenticationConfiguration Authentication { get; set; }

        public DataContextConfiguration DataContext { get; set; }

        public PasswordHashingConfiguration PasswordHashing { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region Methods

        public override void Validate(string optionName = null)
        {
            RequireConfiguration(Authentication, nameof(Authentication), optionName);
            RequireConfiguration(DataContext, nameof(DataContext), optionName);
            RequireConfiguration(PasswordHashing, nameof(PasswordHashing), optionName);
        }

        #endregion Methods
    }
}
