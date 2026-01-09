using System;
using System.Linq.Expressions;

namespace GameDevUtils.Runtime
{
    public static class GenericExpressionMethods
    {
        public abstract class BaseOperationExpression<T>
        {
            protected readonly T defaultValue = default;
            readonly Func<T, T, T> expressionLambda;
            
            protected BaseOperationExpression()
            {
                ParameterExpression exprParamA = Expression.Parameter(typeof(T));
                ParameterExpression exprParamB = Expression.Parameter(typeof(T));
                BinaryExpression addOperation = this.GetExpression(exprParamA, exprParamB);
                Expression<Func<T, T, T>> lambda = Expression.Lambda<Func<T, T, T>>(addOperation, exprParamA, exprParamB);
            
                expressionLambda = lambda.Compile();
            }
            
            protected abstract BinaryExpression GetExpression(ParameterExpression exprParamA, ParameterExpression exprParamB);
            
            public virtual T Process(T a, T b)
            {
                return expressionLambda(a, b);
            }
        }
        
        public class Add<T> : BaseOperationExpression<T>
        {
            protected override BinaryExpression GetExpression(ParameterExpression exprParamA, ParameterExpression exprParamB)
                => Expression.Add(exprParamA, exprParamB);
        }
        
        public class Subtract<T> : BaseOperationExpression<T>
        {
            protected override BinaryExpression GetExpression(ParameterExpression exprParamA, ParameterExpression exprParamB)
                => Expression.Subtract(exprParamA, exprParamB);
        }
        
        public class Multiply<T> : BaseOperationExpression<T>
        {
            protected override BinaryExpression GetExpression(ParameterExpression exprParamA, ParameterExpression exprParamB)
                => Expression.Multiply(exprParamA, exprParamB);
        }
        
        public class Divide<T> : BaseOperationExpression<T>
        {
            protected override BinaryExpression GetExpression(ParameterExpression exprParamA, ParameterExpression exprParamB)
                => Expression.Divide(exprParamA, exprParamB);

            public override T Process(T a, T b)
            {
                if (b.Equals(defaultValue))
                    throw new ArithmeticException("-!-Try divide on default value-!-");
                
                return base.Process(a, b);
            }
        }
    }
}