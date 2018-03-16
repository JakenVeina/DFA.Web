using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using DFA.Common.Extensions;

namespace DFA.Web.Configuration
{
    public abstract class ConfigurationBase
    {
        /**********************************************************************/
        #region Methods

        public abstract void Validate(string optionName = null);

        #endregion Methods

        /**********************************************************************/
        #region Protected Methods

        internal protected void RequireOption<T>(T value, string optionName, string parentOptionName)
            => ValidateOption((value != null), optionName, parentOptionName, "A required configuration option is missing.");

        internal protected void RequireConfiguration(ConfigurationBase configuration, string optionName, string parentOptionName)
        {
            RequireOption(configuration, optionName, parentOptionName);
            configuration.Validate();
        }

        internal protected void RequireNonEmptyString(string value, string optionName, string parentOptionName)
        {
            RequireOption(value, optionName, parentOptionName);
            ValidateOption(!value.IsNullOrEmpty(), optionName, parentOptionName, "Cannot be empty");
        }

        internal protected void RequireEnumValue<T>(string value, string optionName, string parentOptionName)
        {
            RequireOption(value, optionName, parentOptionName);
            ValidateOption(Enum.TryParse(typeof(T), value, out _), optionName, parentOptionName, $"Must be a valid {typeof(T).Name} value");
        }

        internal protected void RequirePositiveNonZeroInteger(string value, string optionName, string parentOptionName)
        {
            RequireOption(value, optionName, parentOptionName);
            ValidateOption(int.TryParse(value, out var integer), optionName, parentOptionName, "Must be an integer");
            ValidateOption((integer > 0), optionName, parentOptionName, "Must be greater than 0");
        }

        internal protected void RequireBase64String(string value, string optionName, string parentOptionName)
        {
            RequireOption(value, optionName, parentOptionName);
            var isValid = Regex.IsMatch(value, @"^[a-zA-Z0-9+\/]+=?=?$");
            ValidateOption(isValid, nameof(value), optionName, "Must be a valid Base64 encoded string");
        }

        internal protected void RequireTimeSpan(string value, string optionName, string parentOptionName)
        {
            RequireOption(value, optionName, parentOptionName);
            var isValid = Regex.IsMatch(value, @"^P(?!$)(\d+Y)?(\d+M)?(\d+W)?(\d+D)?(T(?=\d)(\d+H)?(\d+M)?(\d+S)?)?$");
            ValidateOption(isValid, nameof(value), optionName, "Must be a valid ISO8601 duration");
        }

        internal protected void ValidateOption(bool isValid, string optionName, string parentOptionName, string message)
        {
            Contract.Requires(optionName != null);
            Contract.Requires(message != null);

            if(!isValid)
                throw new ConfigurationOptionException(BuildFullOptionName(optionName, parentOptionName), message);
        }

        internal protected void ValidateCollection<T>(ICollection<T> collection, string optionName, string parentOptionName, Func<T, (bool isValid, string message)> isOptionValid = null)
        {
            Contract.Requires(collection != null);
            Contract.Requires(optionName != null);

            collection
                .Select((item, index) => (item, name: $"{optionName}.{index}"))
                .Do(x => RequireOption(x.item, x.name, parentOptionName))
                .Do(x => x.item.As<ConfigurationBase>()?.Validate())
                .ForEach(x =>
                {
                    if(isOptionValid != null)
                    {
                        (var isValid, var message) = isOptionValid.Invoke(x.item);
                        ValidateOption(isValid, x.name, parentOptionName, message);
                    }
                });
        }

        #endregion Protected Methods

        /**********************************************************************/
        #region Private Methods

        private static string BuildFullOptionName(string optionName, string parentOptionName)
            => (parentOptionName == null) ? optionName : $"{parentOptionName}:{optionName}";

        #endregion Private Methods
    }
}
