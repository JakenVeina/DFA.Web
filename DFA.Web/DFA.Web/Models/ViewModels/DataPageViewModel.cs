using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using DFA.Web.Models.Requests;

namespace DFA.Web.Models.ViewModels

{
    public class DataPageViewModel<TRow>
    {
        /**********************************************************************/
        #region Properties

        public int TotalRowCount { get; set; }

        public int FilteredRowCount { get; set; }

        public IReadOnlyList<TRow> Rows { get; set; }

        #endregion Properties
    }
}
