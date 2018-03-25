using System;
using System.Linq;

using AutoMapper;

using DFA.Common.Extensions;

using DFA.Web.Api.User;
using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Api.NewsPosts
{
    [MapsFrom(typeof(NewsPost))]
    public class NewsPostEventViewModel
    {
        /**********************************************************************/
        #region Properties

        public int Id { get; set; }

        public DateTime Created { get; set; }

        public UserViewModel CreatedBy { get; set; }

        public string Message { get; set; }

        #endregion Properties
    }
}
