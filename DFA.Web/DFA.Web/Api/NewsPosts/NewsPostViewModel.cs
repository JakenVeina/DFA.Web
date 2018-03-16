using System;
using System.Linq;

using AutoMapper;

using DFA.Common.Extensions;

using DFA.Web.Api.User;
using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Api.NewsPosts
{
    [MapsFrom(typeof(NewsPost), nameof(MapFromNewsPost))]
    public class NewsPostViewModel : NewsPostEventViewModel
    {
        /**********************************************************************/
        #region Static Methods

        public static IMappingExpression MapFromNewsPost(IMappingExpression expression)
            => expression.AfterMap((src, dest) =>
                dest.Cast<NewsPostViewModel>().IsUnread
                    = src.Cast<NewsPost>()
                        .UnreadNewsPostNotices
                        .Any());

        #endregion Static Methods

        /**********************************************************************/
        #region Properties

        public bool IsUnread { get; set; }

        #endregion Properties
    }
}
