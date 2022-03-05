using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Invocation;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Properties.Selectors
{
    /// <summary>
    /// Allows for fluent selection of class properties of an instance and setting them up for assertion.
    /// <typeparamref name="TDeclaringType">The type of the instance.</typeparamref>
    /// <typeparamref name="TProperty">The type of the selected properties.</typeparamref>
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> :
        InstancePropertySelectorBase<TDeclaringType, InstancePropertyInfo<TDeclaringType, TProperty>, InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty>>
        
    {
        internal InstancePropertyOfKnownTypeSelector(TDeclaringType instance, IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> instancePropertyInfos)
            : base(instance, instancePropertyInfos)
        {
        }

        /// <summary>
        /// Selects all properties that have the specified value already assigned to them.
        /// </summary>
        /// <param name="value">
        /// The value of the properties that have to be selected. The type of the value is <typeparamref name="TProperty" />.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> HavingValue(TProperty value)
        {
            var propertyInvoker = InvocationContext.PropertyInvokerFactory.CreatePropertyInvoker<TDeclaringType>(Instance);
            
            var filteredProperties = SelectedProperties
                .Where(p => Equals(propertyInvoker.GetValue(p.PropertyInfo.Name), value));

            return CloneFiltered(filteredProperties);
        }

        /// <summary>
        /// Prepares the selected properties to be asserted by associating them with a specified value.
        /// This method does not assign the value to the properties. It just associates the value
        /// with the property internally in the library, so that the assert step knows how to assert them.
        /// </summary>
        /// <param name="value">
        /// The value to be used when asserting the properties. The type of the value is <typeparamref name="TProperty" />.
        /// </param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public PropertyInvocationCollection<TDeclaringType, TProperty> WhenCalledWith(TProperty value)
        {
            var propertyInvocationInfos = SelectedProperties
                .Select(ipi => new PropertyInvocationInfo<TDeclaringType, TProperty>(ipi.PropertyInfo, () => value));
            
            return new PropertyInvocationCollection<TDeclaringType, TProperty>(
                Instance,
                propertyInvocationInfos);
        }

        protected override InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty> CloneFiltered(IEnumerable<InstancePropertyInfo<TDeclaringType, TProperty>> filteredProperties)
        {
            return new InstancePropertyOfKnownTypeSelector<TDeclaringType, TProperty>(Instance, filteredProperties);
        }
    }
}
