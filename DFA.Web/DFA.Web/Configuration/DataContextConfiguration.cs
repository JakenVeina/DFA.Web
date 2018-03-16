using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DFA.Common.Extensions;

namespace DFA.Web.Configuration
{
    public class DataContextConfiguration : ConfigurationBase
    {
        /**********************************************************************/
        #region Properties

        public string ConnectionString { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region ConfigurationBase

        public override void Validate(string optionName = null)
        {
            RequireNonEmptyString(ConnectionString, nameof(ConnectionString), optionName);
        }

        #endregion ConfigurationBase
    }
}
