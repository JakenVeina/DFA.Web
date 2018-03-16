using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Requests
{
    public class DataPageRequest
    {
        /**********************************************************************/
        #region Properties

        [Range(0, int.MaxValue)]
        public int? PageIndex { get; set; }
            = 0;

        [Range(0, int.MaxValue)]
        public int? PageSize { get; set; }
            = int.MaxValue;

        #endregion Properties
    }
}
