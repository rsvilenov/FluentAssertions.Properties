using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;
using System.Diagnostics;

namespace FluentAssertions.Properties.Assertions
{
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
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
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
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstancePropertyInfo<TDeclaringType> Subject { get; }

        public AndConstraint<TAssertions> BeReadable(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
                Subject.PropertyInfo.Should().BeReadable(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }
        
        public AndConstraint<TAssertions> BeReadable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().BeReadable(accessModifier, because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeReadable(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().NotBeReadable(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeWritable(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
                BeWritableInternal(null, because, because));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeWritable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
                BeWritableInternal(accessModifier, because, because));
                
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeWritable(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().NotBeWritable(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeVirtual(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().BeVirtual(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Subject.PropertyInfo.Should().NotBeVirtual(because, becauseArgs));
            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(Subject.PropertyInfo.PropertyType.IsPrimitive)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} to be of primitive type, but was not.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(!Subject.PropertyInfo.PropertyType.IsPrimitive)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} not to be of primitive type, but was.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfValueType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(Subject.PropertyInfo.PropertyType.IsValueType)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} to be of value type, but was not.", Subject.PropertyInfo.Name));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfReferenceType(string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                        .ForCondition(!Subject.PropertyInfo.PropertyType.IsValueType)
                        .BecauseOf(because, becauseArgs)
                        .FailWith("Expected property {0} to be of reference type, but was not.", Subject.PropertyInfo.Name));

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
    }
}
