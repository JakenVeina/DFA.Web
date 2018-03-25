using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using DFA.Web.Models.Requests;

namespace DFA.Web.Models.ViewModels
{
    public delegate IQueryable<T> QueryTransformation<T>(IQueryable<T> query);

    public class DataPageViewModelBuilder<TRow>
    {
        #region Properties

        public IQueryable<TRow> Query { get; set; }

        public QueryTransformation<TRow> Filter { get; set; }

        public QueryTransformation<TRow> Sort { get; set; }

        #endregion Properties

        #region Method

        public async Task<DataPageViewModel<TRow>> Build(DataPageRequest request)
        {
            var viewModel = new DataPageViewModel<TRow>();

            if(Query == null)
            {
                viewModel.TotalRowCount = 0;
                viewModel.FilteredRowCount = 0;
                viewModel.Rows = new TRow[0];
            }
            else
            {
                viewModel.TotalRowCount = await Query.CountAsync();

                var filteredQuery = (Filter == null) ? Query : Filter(Query);
                viewModel.FilteredRowCount = (Filter == null) ? viewModel.TotalRowCount : await filteredQuery.CountAsync();

                var sortedQuery = (Sort == null) ? filteredQuery : Sort(filteredQuery);
                viewModel.Rows = await sortedQuery
                    .Skip(request.FirstRowIndex.Value)
                    .Take(request.PageSize)
                    .ToArrayAsync();
            }

            return viewModel;
        }

        #endregion Method
    }
}
