using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Filters
{
    public class ExceptionViewModel
    {
        /**********************************************************************/
        #region Properties

        public string Error { get; set; }
            = "An unexpected server error occurred.";

        public int LogEntryId { get; set; }

        #endregion Properties
    }
}
