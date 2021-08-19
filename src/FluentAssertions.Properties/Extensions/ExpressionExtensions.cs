using System;
using System.Linq.Expressions;

namespace FluentAssertions.Properties.Extensions
{
    internal static class ExpressionExtensions
    {
        public static string GetMemberName<T>(this Expression<T> expression)
        {
            if (expression.Body is MemberExpression m)
            {
                return m.Member.Name;
            }

            if (expression.Body is UnaryExpression u
                && u.Operand is MemberExpression me)
            {
                return me.Member.Name;
            }

            throw new NotSupportedException($"Unsupported expression: {expression.GetType()}");
        }
    }
}
