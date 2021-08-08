﻿using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Selectors;
using FluentAssertions.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            return Throw<TException>(
                PropertyAccessorEvaluationType.Setter,
                matchExactExceptionType: false,
                because,
                becauseArgs);
        }

        public PropertyExceptionCollection<TException> ThrowFromGetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return Throw<TException>(
                PropertyAccessorEvaluationType.Getter,
                matchExactExceptionType: false, 
                because, 
                becauseArgs);
        }

        public PropertyExceptionCollection<TException> Throw<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return Throw<TException>(
                PropertyAccessorEvaluationType.GetterOrSetter, 
                matchExactExceptionType: false, 
                because, 
                becauseArgs);
        }

        public PropertyExceptionCollection<TException> ThrowFromSetterExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return Throw<TException>(
                PropertyAccessorEvaluationType.Setter,
                matchExactExceptionType: true,
                because,
                becauseArgs);
        }

        public PropertyExceptionCollection<TException> ThrowFromGetterExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return Throw<TException>(
                PropertyAccessorEvaluationType.Getter,
                matchExactExceptionType: true,
                because,
                becauseArgs);
        }

        public PropertyExceptionCollection<TException> ThrowExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return Throw<TException>(
                PropertyAccessorEvaluationType.GetterOrSetter,
                matchExactExceptionType: true,
                because,
                becauseArgs);
        }

        private enum PropertyAccessorEvaluationType
        {
            [Description("getter")]
            Getter,
            [Description("setter")]
            Setter,
            [Description("getter or setter")]
            GetterOrSetter
        }

        private PropertyExceptionCollection<TException> Throw<TException>(PropertyAccessorEvaluationType evalType, bool matchExactExceptionType, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            PropertyExceptionCollection<TException> propertyExceptions = new PropertyExceptionCollection<TException>();

            string accessorTypeFailMessagePart = evalType.GetDescription();

            foreach (var instancePropInfo in Subject)
            {
                try
                {
                    if (evalType == PropertyAccessorEvaluationType.Setter ||
                        evalType == PropertyAccessorEvaluationType.GetterOrSetter)
                    {
                        _propertyInvoker.SetValue(instancePropInfo.PropertyInfo.Name, Subject.Value);
                    }
                    else if (evalType == PropertyAccessorEvaluationType.Getter ||
                        evalType == PropertyAccessorEvaluationType.GetterOrSetter)
                    {
                        _propertyInvoker.GetValue<TProperty>(instancePropInfo.PropertyInfo.Name);
                    }

                    Execute.Assertion
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} of property {1} to throw {2}, but no exception was thrown.", 
                            accessorTypeFailMessagePart, 
                            instancePropInfo.PropertyInfo.Name, 
                            typeof(TException));
                }
                catch (Exception ex)
                {
                    bool exceptionTypeMatches = matchExactExceptionType 
                        ? ex.GetType().Equals(typeof(TException))
                        : ex is TException;

                    if (!exceptionTypeMatches)
                    {
                        Execute.Assertion
                            .BecauseOf(because, becauseArgs)
                            .FailWith("Expected property {0} of property {1} to throw {2}, but {3} was thrown. {4}", 
                                accessorTypeFailMessagePart, 
                                instancePropInfo.PropertyInfo.Name, 
                                typeof(TException), 
                                ex.GetType(), 
                                ex);
                    }

                    propertyExceptions.Add((TException)ex, instancePropInfo.PropertyInfo.Name);
                }
            }

            return propertyExceptions;
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
