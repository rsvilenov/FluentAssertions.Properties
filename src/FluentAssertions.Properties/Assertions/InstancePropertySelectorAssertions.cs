using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Properties.Assertions;
using FluentAssertions.Properties.Selectors;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Properties.Data
{
    [DebuggerNonUserCode]
    public class InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo, TSelector>
        : InstancePropertySelectorAssertions<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo, TSelector>, TDeclaringType, TInstancePropertyInfo, TSelector>
        where TInstancePropertyInfo : InstancePropertyInfo<TDeclaringType>
        where TSelector : InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector>
    {
        internal InstancePropertySelectorAssertions(InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector> value)
            : base (value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertySelectorAssertions<TAssertions, TDeclaringType, TInstancePropertyInfo, TSelector>
        where TAssertions : InstancePropertySelectorAssertions<TAssertions, TDeclaringType, TInstancePropertyInfo, TSelector>
        where TInstancePropertyInfo : InstancePropertyInfo<TDeclaringType>
        where TSelector : InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector>
    {
        internal InstancePropertySelectorAssertions(InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector> value)
        {
            Subject = value; 
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector> Subject { get; }

        public AndConstraint<TAssertions> HaveCount(int expectedCount, string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                .ForCondition(Subject.Count() == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property count of type {0} to be {1}{reason}, but was not.", typeof(TDeclaringType), expectedCount));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() => 
                    BeOfPrimitiveTypeInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeOfPrimitiveTypeInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeOfPrimitiveType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                       NotBeOfPrimitiveTypeInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> NotBeOfPrimitiveTypeInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().NotBeOfPrimitiveType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfValueType(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                        BeOfValueTypeInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeOfValueTypeInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeOfValueType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfReferenceType(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                           BeOfReferenceTypeInternal(because, becauseArgs));
        }
        
        private AndConstraint<TAssertions> BeOfReferenceTypeInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeOfReferenceType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeVirtual(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                              BeVirtualInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeVirtualInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeVirtual(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 NotBeVirtualInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> NotBeVirtualInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().NotBeVirtual(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeWritable(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                    BeWritableInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeWritableInternal(string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeWritable(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeWritable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 BeWritableInternal(accessModifier, because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeWritableInternal(CSharpAccessModifier accessModifier, string because, params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeWritable(accessModifier, because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

    }
}