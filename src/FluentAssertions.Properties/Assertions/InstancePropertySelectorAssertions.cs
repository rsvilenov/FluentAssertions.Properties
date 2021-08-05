using FluentAssertions.Execution;
using FluentAssertions.Properties.Selectors;
using FluentAssertions.Properties.Tests.TestableObjects;
using FluentAssertions.Specialized;
using FluentAssertions.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace FluentAssertions.Properties.Data
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    //[DebuggerNonUserCode]
    //public class InstancePropertyInfoSelectorAssertions
    //    : InstancePropertyInfoSelectorAssertions<InstancePropertyInfoSelectorAssertions>
    //{
    //    public InstancePropertyInfoSelectorAssertions(InstancePropertyInfoSelector value)
    //        : base(value)
    //    {
    //    }
    //}

    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    //[DebuggerNonUserCode]
    //public class InstancePropertyInfoSelectorAssertions<TAssertions>
    //    where TAssertions : InstancePropertyInfoSelectorAssertions<TAssertions>
    //{
    //    public InstancePropertyInfoSelectorAssertions(InstancePropertyInfoSelector value)
    //    {
    //        Subject = value;
    //    }

    //    /// <summary>
    //    /// Gets the object which value is being asserted.
    //    /// </summary>
    //    public InstancePropertyInfoSelector Subject { get; }

    //    /// <summary>
    //    /// Asserts that the value is <c>false</c>.
    //    /// </summary>
    //    /// <param name="because">
    //    /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
    //    /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
    //    /// </param>
    //    /// <param name="becauseArgs">
    //    /// Zero or more objects to format using the placeholders in <paramref name="because" />.
    //    /// </param>
    //    public AndConstraint<TAssertions> BeReadOnly(string because = "", params object[] becauseArgs)
    //    {
    //        foreach (var subject in Subject)
    //        {
    //            Execute.Assertion
    //                .ForCondition(subject.PropertyInfo.CanRead && !subject.PropertyInfo.CanWrite)
    //                .BecauseOf(because, becauseArgs)
    //                .FailWith("Expected property {0} to be read only, but was not.", subject.PropertyInfo.Name);
    //        }

    //        return new AndConstraint<TAssertions>((TAssertions)this);
    //    }


    //    //public AndConstraint<TAssertions> BeSymetric(string because = "", params object[] becauseArgs)
    //    //{
    //    //    Execute.Assertion
    //    //        .ForCondition(Subject.PropertyInfo.CanRead && Subject.PropertyInfo.CanWrite)
    //    //        .BecauseOf(because, becauseArgs)
    //    //        .FailWith("Expected property {0} to be writable, but was not.", Subject.PropertyInfo.Name)
    //    //        .Then
    //    //        .ForCondition(Subject.Instance;

    //    //    return new AndConstraint<TAssertions>((TAssertions)this);
    //    //}

    //    //private bool AreGetSetOperationsSymetric(PropertyInfo propertyInfo, object instance)
    //    //{
    //    //    MethodInfo[] accessors = propertyInfo.GetAccessors();
    //    //    MethodInfo getter = accessors.Single(a => a.Name == "get");
    //    //    MethodInfo setter = accessors.Single(a => a.Name == "set");
    //    //    setter.Invoke(setter, )
    //    //}
    //}


    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertySelectorAssertions<TDeclaringType, TProperty>
    {
        public InstancePropertySelectorAssertions(InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstancePropertyWithKnownTypeSelector<TDeclaringType, TProperty> Subject { get; }

        
        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TProperty>> BeReadOnly(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.CanRead && !subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be read only, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TProperty>>(this);
        }


        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TProperty>> BeWritable(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be writable, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TProperty>>(this);
        }
    }
}