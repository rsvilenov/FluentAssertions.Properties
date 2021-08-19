using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;
using FluentAssertions.Properties.Data.Enums;
using FluentAssertions.Properties.Extensions;
using System;
using System.Diagnostics;

namespace FluentAssertions.Properties.Assertions
{
    [DebuggerNonUserCode]
    public class PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>
        : PropertyInvocationCollectionAssertions<PropertyInvocationCollectionAssertions<TDeclaringType, TProperty>, TDeclaringType, TProperty>
    {
        internal PropertyInvocationCollectionAssertions(PropertyInvocationCollection<TDeclaringType, TProperty> value)
            : base(value)
        {
        }
    }

    [DebuggerNonUserCode]
    public class PropertyInvocationCollectionAssertions<TAssertions, TDeclaringType, TProperty>
        where TAssertions : PropertyInvocationCollectionAssertions<TAssertions, TDeclaringType, TProperty>
    {
        private readonly PropertyInvoker<TDeclaringType> _propertyInvoker;

        internal PropertyInvocationCollectionAssertions(PropertyInvocationCollection<TDeclaringType, TProperty> value)
        {
            Subject = value;
            _propertyInvoker = new PropertyInvoker<TDeclaringType>(value.Instance);
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public PropertyInvocationCollection<TDeclaringType, TProperty> Subject { get; }

        public AndConstraint<TAssertions> ProvideSymmetricAccess(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 ProvideSymmetricAccessInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> ProvideSymmetricAccessInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope assertion = new AssertionScope())
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
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public PropertyExceptionCollectionAssertions<TException> ThrowFromSetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Setter,
                    matchExactExceptionType: false,
                    because,
                    becauseArgs));
        }

        public PropertyExceptionCollectionAssertions<TException> ThrowFromGetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Getter,
                    matchExactExceptionType: false, 
                    because, 
                    becauseArgs));
        }

        public AndConstraint<TAssertions> NotThrowFromSetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               NotThrow<Exception>(
                    PropertyAccessorEvaluation.Setter,
                    matchExactExceptionType: true,
                    because,
                    becauseArgs));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotThrowFromSetter(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
                NotThrow<Exception>(
                    PropertyAccessorEvaluation.Setter,
                    matchExactExceptionType: true,
                    because,
                    becauseArgs));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public PropertyExceptionCollectionAssertions<TException> ThrowFromSetterExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Setter,
                    matchExactExceptionType: true,
                    because,
                    becauseArgs));
        }

        public PropertyExceptionCollectionAssertions<TException> ThrowFromGetterExactly<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                Throw<TException>(
                    PropertyAccessorEvaluation.Getter,
                    matchExactExceptionType: true,
                    because,
                    becauseArgs));
        }

        private PropertyExceptionCollectionAssertions<TException> Throw<TException>(PropertyAccessorEvaluation evalType, bool matchExactExceptionType, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            PropertyExceptionCollection<TException> propertyExceptions = new PropertyExceptionCollection<TException>();

            string accessorTypeFailMessagePart = evalType.GetDescription();

            using (AssertionScope assertion = new AssertionScope())
            {
                foreach (var instancePropInfo in Subject)
                {
                    try
                    {
                        if (evalType == PropertyAccessorEvaluation.Setter)
                        {
                            _propertyInvoker.SetValue(instancePropInfo.PropertyInfo.Name, Subject.Value);
                        }
                        else if (evalType == PropertyAccessorEvaluation.Getter)
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
                                .FailWith("Expected {0} of property {1} to throw {2}, but {3} was thrown. {4}",
                                    accessorTypeFailMessagePart,
                                    instancePropInfo.PropertyInfo.Name,
                                    typeof(TException),
                                    ex.GetType(),
                                    ex);
                        }

                        propertyExceptions.Add((TException)ex, 
                            instancePropInfo.PropertyInfo.Name,
                            evalType);
                    }
                }
            }

            return new PropertyExceptionCollectionAssertions<TException>(propertyExceptions);
        }

        private PropertyExceptionCollectionAssertions<TException> NotThrow<TException>(PropertyAccessorEvaluation evalType, bool matchExactExceptionType, string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            PropertyExceptionCollection<TException> propertyExceptions = new PropertyExceptionCollection<TException>();

            using (AssertionScope assertion = new AssertionScope())
            {
                foreach (var instancePropInfo in Subject)
                {
                    try
                    {
                        if (evalType == PropertyAccessorEvaluation.Setter)
                        {
                            _propertyInvoker.SetValue(instancePropInfo.PropertyInfo.Name, Subject.Value);
                        }
                        else if (evalType == PropertyAccessorEvaluation.Getter)
                        {
                            _propertyInvoker.GetValue<TProperty>(instancePropInfo.PropertyInfo.Name);
                        }
                    }
                    catch (Exception ex)
                    {
                        bool exceptionTypeMatches = matchExactExceptionType
                            ? ex.GetType().Equals(typeof(TException))
                            : ex is TException;
                        if (exceptionTypeMatches)
                        {
                            using (var innerScope = Execute.Assertion)
                            {
                                innerScope.Context = evalType.GetDescription();
                                innerScope.BecauseOf(because, becauseArgs)
                                .FailWith("Did not expect the {context} of property {0} to throw {1}{reason}, but it threw {2}",
                                    instancePropInfo.PropertyInfo.Name,
                                    typeof(TException),
                                    ex);
                            }
                        }
                        else
                        {
                            using (var innerScope = Execute.Assertion)
                            {
                                innerScope.Context = evalType.GetDescription();
                                Execute.Assertion
                                .BecauseOf(because, becauseArgs)
                                .FailWith("Did not expect the {context} of property {0} to throw any exceptions {reason}, but it threw {1}",
                                    instancePropInfo.PropertyInfo.Name,
                                    ex);
                            }
                        }

                        propertyExceptions.Add((TException)ex, 
                            instancePropInfo.PropertyInfo.Name,
                            evalType);
                    }
                }
            }

            return new PropertyExceptionCollectionAssertions<TException>(propertyExceptions);
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
