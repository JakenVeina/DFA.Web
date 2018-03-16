using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace DFA.Web.Models.Responses
{
    public class DataPageResponse<TRow> : ResponseBase
    {
        /**********************************************************************/
        #region Properties

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IReadOnlyList<TRow> Rows { get; set; }

        public int RowCount { get; set; }

        public int TotalRowCount { get; set; }

        #endregion Properties

        /**********************************************************************/
        #region ResponseBase

        public override void Validate()
        {
            Contract.Requires(PageIndex >= 0);
            Contract.Requires(PageSize >= 0);
            Contract.Requires(TotalRowCount >= 0);

            if (Rows == null)
                Rows = Enumerable.Empty<TRow>().ToArray();

            RowCount = Rows.Count;

            if (TotalRowCount < RowCount)
                TotalRowCount = RowCount;

            base.Validate();

            Contract.Ensures(Rows != null);
            Contract.Ensures(RowCount >= 0);
            Contract.Ensures(TotalRowCount >= RowCount);
        }

        #endregion ResponseBase
    }
}
