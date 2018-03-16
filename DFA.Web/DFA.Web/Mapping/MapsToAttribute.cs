using System;
using System.Diagnostics.Contracts;

using AutoMapper;

namespace DFA.Web.Mapping
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class MapsToAttribute : MappingAttribute
    {
        /**********************************************************************/
        #region Constructors

        public MapsToAttribute(Type destinationType, string customizeMappingMethodName = null)
            : base(customizeMappingMethodName)
        {
            Contract.Requires(destinationType != null);

            DestinationType = destinationType;
        }

        #endregion Constructors

        /**********************************************************************/
        #region MappingAttribute

        public override IMappingExpression CreateMap(IMapperConfigurationExpression configuration, Type attachedType)
        {
            Contract.Requires(configuration != null);
            Contract.Requires(attachedType != null);

            return CreateMap(configuration, attachedType, attachedType, DestinationType);
        }

        #endregion MappingAttribute

        /**********************************************************************/
        #region Protected Properties

        internal protected Type DestinationType { get; }

        #endregion Protected Properties
    }
}
