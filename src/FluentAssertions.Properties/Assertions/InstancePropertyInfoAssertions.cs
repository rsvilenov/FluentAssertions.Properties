using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Properties.Common;
using FluentAssertions.Properties.Data;
using System.Diagnostics;
using System;

namespace FluentAssertions.Properties.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert the characteristics of a property.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyInfoAssertions<TDeclaringType>
        : InstancePropertyInfoAssertions<InstancePropertyInfoAssertions<TDeclaringType>, TDeclaringType>
    {
        internal InstancePropertyInfoAssertions(InstancePropertyInfo<TDeclaringType> value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert the characteristics of a property.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyInfoAssertions<TAssertions, TDeclaringType>
        where TAssertions : InstancePropertyInfoAssertions<TAssertions, TDeclaringType>
    {
        internal InstancePropertyInfoAssertions(InstancePropertyInfo<TDeclaringType> value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object whose value is being asserted.
        /// </summary>
        public InstancePropertyInfo<TDeclaringType> Subject { get; }

        /// <summary>
        /// Asserts that the selected property has a setter. 
        /// This method is analogous to <see cref="FluentAssertions.Types.PropertyInfoAssertions.BeWritable(string, object[])"./>
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeWritable(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
                BeWritableInternal(null, because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property has a setter with the specified C# access modifier.
        /// This method is analogous to <see cref="FluentAssertions.Types.PropertyInfoAssertions.BeWritable(CSharpAccessModifier, string, object[])"./>
        /// </summary>
        /// <param name="accessModifier">The expected C# access modifier.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="accessModifier"/>
        /// is not a <see cref="CSharpAccessModifier"/> value.</exception>
        public AndConstraint<TAssertions> BeWritable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            Guard.ThrowIfArgumentIsOutOfRange(accessModifier, nameof(accessModifier));

            ExceptionStackTrace.StartFromCurrentFrame(() =>
                BeWritableInternal(accessModifier, because, becauseArgs));
                
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property does not have a setter.
        /// This method is analogous to <see cref="FluentAssertions.Types.PropertyInfoAssertions.NotBeWritable(string, object[])"./>
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeWritable(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
                NotBeWritableInternal(because, becauseArgs));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property is virtual.
        /// This method is analogous to <see cref="FluentAssertions.Types.PropertyInfoAssertions.BeVirtual(string, object[])"./>
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeVirtual(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().BeVirtual(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property is not virtual.
        /// This method is analogous to <see cref="FluentAssertions.Types.PropertyInfoAssertions.NotBeVirtual(string, object[])"./>
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().NotBeVirtual(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property's type is one of the primitive types.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(Subject.PropertyInfo.PropertyType.IsPrimitive)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} to be of primitive type{reason}, but was not.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property's type is not one of the primitive types .
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> NotBeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(!Subject.PropertyInfo.PropertyType.IsPrimitive)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} not to be of primitive type{reason}, but was.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property's type is a value type.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeOfValueType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(Subject.PropertyInfo.PropertyType.IsValueType)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} to be of value type{reason}, but was not.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected property's type is a reference type.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeOfReferenceType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(!Subject.PropertyInfo.PropertyType.IsValueType)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} to be of reference type{reason}, but was not.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private AndConstraint<TAssertions> BeWritableInternal(CSharpAccessModifier? accessModifier, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                    .ForCondition(Subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:property} {0} to have a setter{reason}.",
                        Subject.PropertyInfo);

            if (accessModifier.HasValue)
            {
                Subject.PropertyInfo.GetSetMethod(nonPublic: true).Should().HaveAccessModifier(accessModifier.Value, because, becauseArgs);
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        private AndConstraint<TAssertions> NotBeWritableInternal(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                    .ForCondition(!Subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith(
                        "Expected {context:property} {0} not to have a setter{reason}.",
                        Subject.PropertyInfo);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }
}
