using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using DFA.Common.Extensions;

namespace DFA.Web.Mapping
{
    public static class MappingExtensions
    {
        /**********************************************************************/
        #region Extension Methods

        public static void BuildMappings(this IMapperConfigurationExpression mapperConfig, Assembly assembly)
        {
            Contract.Requires(mapperConfig != null);
            Contract.Requires(assembly != null);

            mapperConfig.BuildMappings(assembly.MakeEnumerable());
        }

        public static void BuildMappings(this IMapperConfigurationExpression mapperConfig, IEnumerable<Assembly> assemblies)
        {
            Contract.Requires(mapperConfig != null);
            Contract.Requires(assemblies != null);
            Contract.Requires(assemblies.All(x => x != null));

            assemblies.SelectMany(x => x.GetTypes())
                      .SelectMany(x => x.GetCustomAttributes<MappingAttribute>()
                                        .Select(y => new { Type = x, Attribute = y }))
                      .ForEach(x => x.Attribute.CreateMap(mapperConfig, x.Type));
        }

        public static TDestination MapTo<TDestination>(this object source)
            => Mapper.Map<TDestination>(source);

        public static void MapFrom<TSource, TDestination>(this TDestination destination, TSource source)
            => Mapper.Map(source, destination);

        #endregion Extension Methods
    }
}
