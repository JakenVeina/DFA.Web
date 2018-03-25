using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

namespace DFA.Web.Mapping
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class MapsToAndFromAttribute : MappingAttribute
    {
        /**********************************************************************/
        #region Constructors

        public MapsToAndFromAttribute(Type targetType, string customizeMappingMethodName = null)
            : base(customizeMappingMethodName)
        {
            Contract.Requires(targetType != null);

            TargetType = targetType;
        }

        #endregion Constructors

        /**********************************************************************/
        #region MappingAttribute

        public override IMappingExpression CreateMap(IMapperConfigurationExpression configuration, Type attachedType)
        {
            Contract.Requires(configuration != null);
            Contract.Requires(attachedType != null);

            CreateMap(configuration, attachedType, attachedType, TargetType);
            return CreateMap(configuration, attachedType, TargetType, attachedType);
        }

        #endregion MappingAttribute

        /**********************************************************************/
        #region Protected Properties

        internal protected Type TargetType { get; }

        #endregion Protected Properties
    }
}
