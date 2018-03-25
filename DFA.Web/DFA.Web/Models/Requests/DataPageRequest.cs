using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace DFA.Web.Models.Requests
{
    public class DataPageRequest
    {
        /**********************************************************************/
        #region Properties

        [Range(0, int.MaxValue)]
        public int? FirstRowIndex { get; set; }
            = 0;

        [Range(0, int.MaxValue)]
        public int? LastRowIndex { get; set; }
            = int.MaxValue;

        [JsonIgnore]
        public int PageSize
        {
            get => LastRowIndex.Value - FirstRowIndex.Value + 1;
            set => LastRowIndex = FirstRowIndex.Value + value - 1;
        }

        #endregion Properties
    }
}
