using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Properties.Selectors;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Properties.Data
{
    [DebuggerNonUserCode]
    public class InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>
        : InstancePropertySelectorAssertions<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>, TDeclaringType, TInstancePropertyInfo>
        where TInstancePropertyInfo : InstancePropertyInfo<TDeclaringType>, new()
    {
        public InstancePropertySelectorAssertions(InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> value)
            : base (value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertySelectorAssertions<TAssertions, TDeclaringType, TInstancePropertyInfo>
        where TAssertions : InstancePropertySelectorAssertions<TAssertions, TDeclaringType, TInstancePropertyInfo>
        where TInstancePropertyInfo : InstancePropertyInfo<TDeclaringType>, new()
    {
        public InstancePropertySelectorAssertions(InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> value)
        {
            Subject = value; 
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo> Subject { get; }

        public AndConstraint<TAssertions> HaveCount(int expectedCount, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Count() == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property count of type {0} to be {1}, but was not.", typeof(TDeclaringType), expectedCount);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeOfPrimitiveType(string because = "", params object[] becauseArgs)
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
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().NotBeVirtual(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeReadable(string because = "", params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeReadable(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeReadable(CSharpAccessModifier accessModifier, string because = "", params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().BeReadable(accessModifier, because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> NotBeReadable(string because = "", params object[] becauseArgs)
        {
            using (AssertionScope scope = new AssertionScope())
            {
                foreach (var subject in Subject)
                {
                    subject.Should().NotBeReadable(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        public AndConstraint<TAssertions> BeWritable(string because = "", params object[] becauseArgs)
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