using FluentAssertions.Execution;
using FluentAssertions.Properties.Selectors;
using System.Diagnostics;
using System.Linq;

namespace FluentAssertions.Properties.Data
{
    /// <summary>
    /// Contains a number of methods to assert that a <see cref="bool"/> is in the expected state.
    /// </summary>
    [DebuggerNonUserCode]
    public class InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>
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

        
        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> BeReadOnly(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(!subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be read only, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }


        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> BeWritable(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.CanWrite)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be writable, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> HaveCount(int expectedCount, string because = "", params object[] becauseArgs)
        {
            Execute.Assertion
                .ForCondition(Subject.Count() == expectedCount)
                .BecauseOf(because, becauseArgs)
                .FailWith("Expected property count of type {0} to be {1}, but was not.", typeof(TDeclaringType), expectedCount);

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> BeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.PropertyType.IsPrimitive)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be of primitive type, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> NotBeOfPrimitiveType(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.PropertyType.IsPrimitive)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} not to be of primitive type, but was.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> BeOfValueType(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.PropertyType.IsValueType)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be of value type, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> BeOfReferenceType(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(!subject.PropertyInfo.PropertyType.IsValueType)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be of reference type, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> BeVirtual(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(subject.PropertyInfo.GetMethod.IsVirtual)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} to be virtual, but was not.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }

        public AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>> NotBeVirtual(string because = "", params object[] becauseArgs)
        {
            foreach (var subject in Subject)
            {
                Execute.Assertion
                    .ForCondition(!subject.PropertyInfo.GetMethod.IsVirtual)
                    .BecauseOf(because, becauseArgs)
                    .FailWith("Expected property {0} not to be virtual, but was.", subject.PropertyInfo.Name);
            }

            return new AndConstraint<InstancePropertySelectorAssertions<TDeclaringType, TInstancePropertyInfo>>(this);
        }
    }
}