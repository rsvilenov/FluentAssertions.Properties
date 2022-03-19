using FluentAssertions.Common;
using FluentAssertions.Execution;
using FluentAssertions.Properties.Assertions;
using FluentAssertions.Properties.Common;
using FluentAssertions.Properties.Selectors;
using System.Diagnostics;
using System.Linq;
using System;

namespace FluentAssertions.Properties.Data
{
    /// <summary>
    /// Contains a number of methods for asserting a selection of properties.
    /// </summary>
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
    /// Contains a number of methods for asserting a selection of properties.
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
        /// Gets the object whose value is being asserted.
        /// </summary>
        internal InstancePropertySelectorBase<TDeclaringType, TInstancePropertyInfo, TSelector> Subject { get; }

        /// <summary>
        /// Asserts that the selected properties have a specified count.
        /// </summary>
        /// <param name="expectedCount">The expected count of the selected properties.</param>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> HaveCount(int expectedCount, string because = "", params object[] becauseArgs)
        {
            ExceptionStackTrace.StartFromCurrentFrame(() =>
               Execute.Assertion
                .ForCondition(Subject.Count() == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property count of type {0} to be {1}{reason}, but was not.", typeof(TDeclaringType), expectedCount));

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties's types are primitive.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() => 
                    BeOfPrimitiveTypeInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeOfPrimitiveTypeInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeOfPrimitiveType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties's types are not primitive.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                       NotBeOfPrimitiveTypeInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> NotBeOfPrimitiveTypeInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().NotBeOfPrimitiveType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties are of value type.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                        BeOfValueTypeInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeOfValueTypeInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeOfValueType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties are of reference type.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                           BeOfReferenceTypeInternal(because, becauseArgs));
        }
        
        private AndConstraint<TAssertions> BeOfReferenceTypeInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeOfReferenceType(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties are virtual.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                              BeVirtualInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeVirtualInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeVirtual(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties are not virtual.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 NotBeVirtualInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> NotBeVirtualInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().NotBeVirtual(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties do not have a setter.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                    NotBeWritableInternal(because, becauseArgs));
        }


        private AndConstraint<TAssertions> NotBeWritableInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().NotBeWritable(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties have a setter.
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
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                    BeWritableInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeWritableInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeWritable(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

        /// <summary>
        /// Asserts that the selected properties have a setter with the specified C# access modifier.
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

            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 BeWritableInternal(accessModifier, because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeWritableInternal(CSharpAccessModifier accessModifier, string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeWritable(accessModifier, because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }


#if NET5_0_OR_GREATER

        /// <summary>
        /// Asserts that the selected properties have an init only setter.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeInitOnly(string because = "", params object[] becauseArgs)
        {
            return ExceptionStackTrace.StartFromCurrentFrame(() =>
                                 BeInitOnlyInternal(because, becauseArgs));
        }

        private AndConstraint<TAssertions> BeInitOnlyInternal(string because, params object[] becauseArgs)
        {
            using (new AssertionScope())
            {
                foreach (var item in Subject.GetSelection())
                {
                    item.Should().BeInitOnly(because, becauseArgs);
                }
            }

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

#endif

    }
}