using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Validation
{
    public class AlphanumericAttribute : ValidationAttribute
    {
        /**********************************************************************/
        #region Constructors

        public AlphanumericAttribute()
        {
            ErrorMessage = "Must contain only alphanumeric characters";
        }

        #endregion Constructors

        /**********************************************************************/
        #region ValidationAttribute Overrides

        public virtual string FormatErrorMessage(string name)
            => $"{name} must contain only alphanumeric characters";

        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var str = value as string;

            if (str == null)
                return false;

            return str.All(char.IsLetterOrDigit);
        }

        #endregion ValidationAttribute Overrides
    }
}
