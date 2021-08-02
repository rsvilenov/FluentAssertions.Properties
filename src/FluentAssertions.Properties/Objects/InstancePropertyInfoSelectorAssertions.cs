using FluentAssertions.Execution;
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

namespace FluentAssertions.Properties.Objects
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
    public class InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty>
    {
        public InstancePropertyInfoSelectorAssertions(InstancePropertyInfoSelector<TDeclaringType, TProperty> value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstancePropertyInfoSelector<TDeclaringType, TProperty> Subject { get; }

        
        public AndConstraint<InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty>> BeReadOnly(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.CanRead && !subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be read only, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty>>(this);
        }


        public AndConstraint<InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty>> BeWritable(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be writable, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertyInfoSelectorAssertions<TDeclaringType, TProperty>>(this);
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstanceWithValuePropertyInfoSelectorAssertions<TDeclaringType, TProperty>
    {
        public InstanceWithValuePropertyInfoSelectorAssertions(InstanceWithValuePropertyInfoSelector<TDeclaringType, TProperty> value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstanceWithValuePropertyInfoSelector<TDeclaringType, TProperty> Subject { get; }

        public AndConstraint<InstanceWithValuePropertyInfoSelectorAssertions<TDeclaringType, TProperty>> ProvideSymmetricAccess(string because = "", params object[] becauseArgs)
        {
            foreach (var instancePropertyInfo in Subject)
            {
                Execute.Assertion
                .ForCondition(instancePropertyInfo.PropertyInfo.CanRead && instancePropertyInfo.PropertyInfo.CanWrite)
                .FailWith("Expected property {0} to be writable, but was not.", instancePropertyInfo.PropertyInfo.Name)
                .Then
                .ForCondition(AreGetSetOperationsSymetric(instancePropertyInfo, Subject.Value))
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected the get and set operations of property {0} to be symetric, but was not.", instancePropertyInfo.PropertyInfo.Name);
            }

            return new AndConstraint<InstanceWithValuePropertyInfoSelectorAssertions<TDeclaringType, TProperty>>(this);
        }

        public ExceptionAssertions<TException> ThrowFromSetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            Action setAction = new Action(() =>
            {
                foreach (var instancePropInfo in Subject)
                {
                    SetValue(Subject.Instance, instancePropInfo, Subject.Value);
                }
            });

            return setAction
                .Should()
                .Throw<TException>(because, becauseArgs);
        }

        public ExceptionAssertions<TException> ThrowFromGetter<TException>(string because = "", params object[] becauseArgs)
            where TException : Exception
        {
            Action setAction = new Action(() =>
            {
                foreach (var instancePropInfo in Subject)
                {
                    GetValue(Subject.Instance, instancePropInfo);
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
                    SetValue(Subject.Instance, instancePropInfo, Subject.Value);
                    GetValue(Subject.Instance, instancePropInfo);
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
                SetValue(Subject.Instance, instancePropertyInfo, testData);
                object got = GetValue(Subject.Instance, instancePropertyInfo);
                isSymmetric = testData.Equals(got);
            }
            catch(Exception ex)
            {
                Execute.Assertion
                .FailWith($"Did not expect any exceptions for property {instancePropertyInfo.PropertyInfo.Name}, but got {ex}.");
            }

            return isSymmetric;
        }

        private void SetValue(TDeclaringType instance, InstancePropertyInfo<TDeclaringType, TProperty> instancePropertyInfo,
            TProperty testData)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TDeclaringType));

            ParameterExpression paramExpression2 = Expression.Parameter(typeof(TProperty), instancePropertyInfo.PropertyInfo.Name);

            MemberExpression propertyGetterExpression = Expression.Property(paramExpression, instancePropertyInfo.PropertyInfo.Name);

            Action<TDeclaringType, TProperty> result = Expression.Lambda<Action<TDeclaringType, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();

            result(instance, testData);

            //(instancePropertyInfo.Instance as SampleDto).MyStringProperty2 = (string)testData;
            //instancePropertyInfo.PropertyInfo.GetValueSetter<TProperty>().Invoke(instancePropertyInfo.Instance, testData); ;
            //throw new ArgumentException("test");
            // MethodInfo setter = instancePropertyInfo.PropertyInfo.GetSetMethod();
            // setter.Invoke(instancePropertyInfo.Instance, new object[] { testData });
        }

        

        private object GetValue(TDeclaringType instance, InstancePropertyInfo<TDeclaringType, TProperty> instancePropertyInfo)
        {
            MethodInfo setter = instancePropertyInfo.PropertyInfo.GetGetMethod();
            return setter.Invoke(instance, null);
        }
    }
}