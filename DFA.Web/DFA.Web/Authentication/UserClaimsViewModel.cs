using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DFA.Common.Extensions;
using DFA.Web.Mapping;
using DFA.Web.Models.Entities;

namespace DFA.Web.Authentication
{
    [MapsFrom(typeof(Models.Entities.User), nameof(MapFromUser))]
    public class UserClaimsViewModel
    {
        /**********************************************************************/
        #region Static Methods

        public static IMappingExpression MapFromUser(IMappingExpression expression)
            => expression.AfterMap((src, dest) =>
                dest.Cast<UserClaimsViewModel>().RoleNames
                    = src.Cast<Models.Entities.User>()
                        .UserRoleMaps
                        .Select(x => x.Role.Name)
                        .ToHashSet());

        #endregion Static Methods

        /**********************************************************************/
        #region Properties

        public int Id { get; set; }
        
        public int LoginNonce { get; set; }

        public ICollection<string> RoleNames { get; set; }

        #endregion Properties
    }
}
