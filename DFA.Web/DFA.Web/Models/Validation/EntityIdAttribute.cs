using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Validation
{
    public class EntityIdAttribute : RangeAttribute
    {
        /**********************************************************************/
        #region Constructors

        public EntityIdAttribute()
            : base(1, int.MaxValue) { }

        #endregion Constructors
    }
}
