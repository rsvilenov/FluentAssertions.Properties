using System.Runtime.ExceptionServices;

namespace FluentAssertions.Properties.Invocation
{
    internal class PropertyInvocationResult<TProperty> 
        : InvocationResult, IInvocationResult<TProperty>
    {
        public PropertyInvocationResult(TProperty value)
            : base()
        {
            Value = value;
        }

        public PropertyInvocationResult(ExceptionDispatchInfo exceptionDispatchInfo)
            : base(exceptionDispatchInfo)
        { }

        public TProperty Value { get; set; }
    }

    internal class InvocationResult 
        : IInvocationResult
    {
        public InvocationResult()
        {
        }

        public InvocationResult(ExceptionDispatchInfo exceptionDispatchInfo)
        {
            ExceptionDispatchInfo = exceptionDispatchInfo;
            Success = false;
        }
        public bool Success { get; set; } = true;
        public ExceptionDispatchInfo ExceptionDispatchInfo { get; set; }
    }
}
