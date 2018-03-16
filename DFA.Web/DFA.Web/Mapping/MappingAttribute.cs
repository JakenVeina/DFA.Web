using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

using AutoMapper;

namespace DFA.Web.Mapping
{
    public abstract class MappingAttribute : Attribute
    {
        /**********************************************************************/
        #region Types

        public delegate IMappingExpression CustomizeMappingDelegate(IMappingExpression expression);

        #endregion Types

        /**********************************************************************/
        #region Constructors

        public MappingAttribute(string customizeMappingMethodName = null)
        {
            Contract.Requires(customizeMappingMethodName != string.Empty);

            CustomizeMappingMethodName = customizeMappingMethodName;
        }

        #endregion Constructors

        /**********************************************************************/
        #region Methods

        public abstract IMappingExpression CreateMap(IMapperConfigurationExpression configuration, Type attachedType);

        #endregion Methods

        /**********************************************************************/
        #region Protected Properties

        internal protected string CustomizeMappingMethodName { get; }

        #endregion Protected Properties

        /**********************************************************************/
        #region Protected Methods

        internal protected IMappingExpression CreateMap(IMapperConfigurationExpression configuration, Type attachedType, Type sourceType, Type destinationType)
        {
            Contract.Requires(configuration != null);
            Contract.Requires(attachedType != null);
            Contract.Requires(sourceType != null);
            Contract.Requires(destinationType != null);

            var expression = configuration.CreateMap(sourceType, destinationType);

            if(CustomizeMappingMethodName != null)
            {
                var customizeMappingMethodInfo = attachedType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                             .SingleOrDefault(x => x.Name == CustomizeMappingMethodName);

                if (customizeMappingMethodInfo == null)
                    throw new InvalidOperationException($"Type {attachedType.Name} does not contain a single public static method {CustomizeMappingMethodName}");

                var customizeMappingMethod = (CustomizeMappingDelegate)Delegate.CreateDelegate(typeof(CustomizeMappingDelegate), customizeMappingMethodInfo, false);

                if (customizeMappingMethod == null)
                    throw new InvalidOperationException($"Method {CustomizeMappingMethodName} is not compatible with delegate type {nameof(CustomizeMappingDelegate)}");

                expression = customizeMappingMethod.Invoke(expression);
            }

            return expression;
        }

        #endregion Protected Methods
    }
}
