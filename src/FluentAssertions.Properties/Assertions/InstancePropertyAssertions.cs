using System.Diagnostics;
using System.Linq;
using System.Reflection;
using FluentAssertions.Execution;
using FluentAssertions.Properties.Data;

namespace FluentAssertions.Properties.Assertions
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertyAssertions<TAssertions, TDeclaringType>
        where TAssertions : InstancePropertyAssertions<TAssertions, TDeclaringType>
    {
        public InstancePropertyAssertions(InstancePropertyInfo<TDeclaringType> value)
        {
            Subject = value;
        }

        /// <summary>
        /// Gets the object which value is being asserted.
        /// </summary>
        public InstancePropertyInfo<TDeclaringType> Subject { get; }

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
                .ForCondition(Subject.PropertyInfo.CanRead && !Subject.PropertyInfo.CanWrite)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property {0} to be read only, but was not.", Subject.PropertyInfo.Name);

            return new AndConstraint<TAssertions>((TAssertions)this);
        }
    }
}
