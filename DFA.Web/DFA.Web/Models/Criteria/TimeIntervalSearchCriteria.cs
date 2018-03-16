using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Criteria
{
    public class TimeIntervalSearchCriteria
    {
        /**********************************************************************/
        #region Properties

        public DateTime? FromTime { get; set; }

        public DateTime? ToTime { get; set; }

        #endregion Properties
    }
}
