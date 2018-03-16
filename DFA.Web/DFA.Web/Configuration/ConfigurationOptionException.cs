using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DFA.Web.Configuration
{
    public class ConfigurationOptionException : Exception
    {
        /**********************************************************************/
        #region Constructors

        public ConfigurationOptionException(string optionName, string message)
            : base(message)
        {
            Contract.Requires(optionName != null);

            OptionName = optionName;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Properties

        public string OptionName { get; }

        #endregion Properties

        /**********************************************************************/
        #region ISerializable

        internal protected ConfigurationOptionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            OptionName = info.GetString(nameof(OptionName));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(OptionName), OptionName);

            base.GetObjectData(info, context);
        }

        #endregion ISerializable
    }
}
