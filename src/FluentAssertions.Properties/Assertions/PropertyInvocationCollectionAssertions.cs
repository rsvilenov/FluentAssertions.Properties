using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Selectors;
using FluentAssertions.Specialized;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentAssertions.Properties.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>
    {
        private readonly PropertyInvoker<TDeclaringType> _propertyInvoker;
        public PropertyInvocationCollectionAssertions(PropertyInvocationCollection<TDeclaringType, TProperty> value)
        {
            Subject = value;
            _propertyInvoker = new PropertyInvoker<TDeclaringType>(value.Instance);
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public PropertyInvocationCollection<TDeclaringType, TProperty> Subject { get; }

        public AndConstraint<PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>> ProvideSymmetricAccess(string because = "", params object[] becauseArgs)
        {
            foreach (var instancePropertyInfo in Subject)
            {
                Execute.Assertion
                .ForCondition(instancePropertyInfo.PropertyInfo.CanWrite)
                .FailWith("Expected property {0} to be writable, but was not.", instancePropertyInfo.PropertyInfo.Name)
                .Then
                .ForCondition(AreGetSetOperationsSymetric(instancePropertyInfo, Subject.Value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the get and set operations of property {0} to be symetric, but was not.", instancePropertyInfo.PropertyInfo.Name);
            }

            return new AndConstraint<PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>>(this);
        }

        public PropertyExceptionCollection<TException> ThrowFromSetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            PropertyExceptionCollection<TException> propertyExceptions = new PropertyExceptionCollection<TException>();

            foreach (var instancePropInfo in Subject)
            {
                try
                {
                    _propertyInvoker.SetValue(instancePropInfo.PropertyInfo.Name, Subject.Value);

                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property setter of property {0} to throw {1}, but no exception was thrown.", instancePropInfo.PropertyInfo.Name, typeof(TException));
                }
                catch (Exception ex)
                {
                    TException tex = ex as TException;
                    if (tex == null)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected property setter of property {0} to throw {1}, but {2} was thrown. {3}", instancePropInfo.PropertyInfo.Name, typeof(TException), ex.GetType(), ex);
                    }

                    propertyExceptions.Add(tex, instancePropInfo.PropertyInfo.Name);
                }
            }

            return propertyExceptions;
        }

        public ExceptionAssertions<TException> ThrowFromGetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            Action setAction = new Action(() =>
            {
                foreach (var instancePropInfo in Subject)
                {
                    _propertyInvoker.GetValue<TProperty>(instancePropInfo.PropertyInfo.Name);
                }
            });

            return setAction
                .Should()
                .Throw<TException>(because, becauseArgs);
        }

        public ExceptionAssertions<TException> Throw<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            Action setAction = new Action(() =>
            {
                foreach (var instancePropInfo in Subject)
                {
                    _propertyInvoker.SetValue(instancePropInfo.PropertyInfo.Name, Subject.Value);
                    _propertyInvoker.GetValue<TProperty>(instancePropInfo.PropertyInfo.Name);
                }
            });

            return setAction
                .Should()
                .Throw<TException>(because, becauseArgs);
        }

        private bool AreGetSetOperationsSymetric(InstancePropertyInfo<TDeclaringType, TProperty> instancePropertyInfo,
            TProperty testData)
        {
            bool isSymmetric = false;
            try
            {
                _propertyInvoker.SetValue(instancePropertyInfo.PropertyInfo.Name, testData);
                TProperty got = _propertyInvoker.GetValue<TProperty>(instancePropertyInfo.PropertyInfo.Name);
                isSymmetric = testData.Equals(got);
            }
            catch (Exception ex)
            {
                Execute.Assertion
                .FailWith($"Did not expect any exceptions for property {instancePropertyInfo.PropertyInfo.Name}, but got {ex}.");
            }

            return isSymmetric;
        }
    }
}
