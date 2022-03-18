using System.Runtime.ExceptionServices;

namespace FluentAssertions.Properties.Invocation
{
    public interface IInvocationResult<TProperty>
        : IInvocationResult
    {
        TProperty Value { get; }
    }

    public interface IInvocationResult
    {
        bool Success { get; }
        ExceptionDispatchInfo ExceptionDispatchInfo { get; }
    }
}
