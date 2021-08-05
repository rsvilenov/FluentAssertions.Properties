using System;
using System.Linq.Expressions;

namespace FluentAssertions.Properties
{
    internal class PropertyInvoker<TDeclaringType>
    {
        private readonly TDeclaringType _instance;
        public PropertyInvoker(TDeclaringType instance)
        {
            _instance = instance;
        }

        public void SetValue<TProperty>(string propertyName, TProperty testData)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TDeclaringType));
            ParameterExpression paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            MemberExpression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            Action<TDeclaringType, TProperty> result = Expression.Lambda<Action<TDeclaringType, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            )
            .Compile();

            result(_instance, testData);
        }

        public TProperty GetValue<TProperty>(string propertyName)
        {
            ParameterExpression paramExpression = Expression.Parameter(typeof(TDeclaringType));
            MemberExpression propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            var expression = Expression.Lambda<Func<TDeclaringType, TProperty>>(propertyGetterExpression, paramExpression);
            var labmda = expression.Compile();

            return labmda(_instance);
        }
    }
}
