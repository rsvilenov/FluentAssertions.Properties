using System.Diagnostics;
using System.Reflection;
using FluentAssertions.Execution;

namespace FluentAssertions.Properties.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInfoAssertions
        : PropertyInfoAssertions<PropertyInfoAssertions>
    {
        public PropertyInfoAssertions(PropertyInfo value)
            : base(value)
        {
        }
    }

    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class PropertyInfoAssertions<TAssertions>
        where TAssertions : PropertyInfoAssertions<TAssertions>
    {
        public PropertyInfoAssertions(PropertyInfo value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public PropertyInfo Subject { get; }

        /// <summary>
        /// Asserts that the value is <c>false</c>.
        /// </summary>
        /// <param name="because">
        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
        /// </param>
        /// <param name="becauseArgs">
        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
        /// </param>
        public AndConstraint<TAssertions> BeReadOnly(string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.CanRead && !Subject.CanWrite)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property {0} to be read only, but was not.", Subject.Name);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }

    }
}
