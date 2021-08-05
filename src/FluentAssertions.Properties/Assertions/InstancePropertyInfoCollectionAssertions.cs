//using FluentAssertions.Execution;
//using FluentAssertions.Properties.Objects;
//using System.Collections.Generic;
//using System.Diagnostics;

//namespace FluentAssertions.Properties.Primitives
//{
//    /// <summary>
//    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
//    /// </summary>
//    [DebuggerNonUserCode]
//    public class InstancePropertyInfoCollectionAssertions
//        : InstancePropertyInfoCollectionAssertions<InstancePropertyInfoCollectionAssertions>
//    {
//        public InstancePropertyInfoCollectionAssertions(IEnumerable<InstancePropertyInfo> value)
//            : base(value)
//        {
//        }
//    }

//    /// <summary>
//    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
//    /// </summary>
//    [DebuggerNonUserCode]
//    public class InstancePropertyInfoCollectionAssertions<TAssertions>
//        where TAssertions : InstancePropertyInfoCollectionAssertions<TAssertions>
//    {
//        public InstancePropertyInfoCollectionAssertions(IEnumerable<InstancePropertyInfo> value)
//        {
//            SubjectCollection = value;
//        }

//        /// <summary>
//        /// Gets the object which value is being asserted.
//        /// </summary>
//        public IEnumerable<InstancePropertyInfo> SubjectCollection { get; }

//        /// <summary>
//        /// Asserts that the value is <c>false</c>.
//        /// </summary>
//        /// <param name="because">
//        /// A formatted phrase as is supported by <see cref="string.Format(string,object[])" /> explaining why the assertion
//        /// is needed. If the phrase does not start with the word <i>because</i>, it is prepended automatically.
//        /// </param>
//        /// <param name="becauseArgs">
//        /// Zero or more objects to format using the placeholders in <paramref name="because" />.
//        /// </param>
//        public AndConstraint<TAssertions> BeReadOnly(string because = "", params object[] becauseArgs)
//        {
//            foreach (var subject in SubjectCollection)
//            {
//                Execute.Assertion
//                    .ForCondition(subject.PropertyInfo.CanRead && !subject.PropertyInfo.CanWrite)
//                    .BecauseOf(because, becauseArgs)
//                    .FailWith("Expected property {0} to be read only, but was not.", subject.PropertyInfo.Name);
//            }

//            return new AndConstraint<TAssertions>((TAssertions)this);
//        }
//    }
//}