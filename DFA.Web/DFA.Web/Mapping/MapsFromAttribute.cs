using System;
using System.Diagnostics.Contracts;

using AutoMapper;

namespace DFA.Web.Mapping
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class MapsFromAttribute : MappingAttribute
    {
        /**********************************************************************/
        #region Constructors

        public MapsFromAttribute(Type sourceType, string customizeMappingMethodName = null)
            : base(customizeMappingMethodName)
        {
            Contract.Requires(sourceType != null);

            SourceType = sourceType;
        }

        #endregion Constructors

        /**********************************************************************/
        #region MappingAttribute

        public override IMappingExpression CreateMap(IMapperConfigurationExpression configuration, Type attachedType)
        {
            Contract.Requires(configuration != null);
            Contract.Requires(attachedType != null);

            return CreateMap(configuration, attachedType, SourceType, attachedType);
        }

        #endregion MappingAttribute

        /**********************************************************************/
        #region Protected Properties

        internal protected Type SourceType { get; }

        #endregion Protected Properties
    }
}
