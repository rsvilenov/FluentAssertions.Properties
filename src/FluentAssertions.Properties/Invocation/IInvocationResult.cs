using System.Runtime.ExceptionServices;

namespace FluentAssertions.Properties.Invocation
{
    internal interface IInvocationResult<TProperty>
        : IInvocationResult
    {
        TProperty Value { get; }
    }

    internal interface IInvocationResult
    {
        bool Success { get; }
        ExceptionDispatchInfo ExceptionDispatchInfo { get; }
    }
}
