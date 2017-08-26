using Procons.Durrah.Common.Enumerators;
using System;
using System.Linq.Expressions;

public class ExpressionBuilder<T>
{
    Expression finalExpression = null;
    ParameterExpression parameter = null;

    public Expression FinalExpression { get { return finalExpression; } }
    public ExpressionBuilder<T> BuildExpression<Y>(string left, Operation operationType, Y value)
    {
        parameter = Expression.Parameter(typeof(T), "x");

        var leftProperty = Expression.Property(parameter, left);
        var comparableValue = Expression.Constant(value, typeof(Y));
        BinaryExpression body = null;

        switch (operationType)
        {
            case Operation.EqualTo: body = Expression.Equal(leftProperty, comparableValue); break;
            case Operation.NotEqualTo: body = Expression.NotEqual(leftProperty, comparableValue); break;
            case Operation.GreaterThan: body = Expression.GreaterThan(leftProperty, comparableValue); break;
            case Operation.GreaterThanOrEqualTo: body = Expression.GreaterThanOrEqual(leftProperty, comparableValue); break;
            case Operation.LessThan: body = Expression.LessThan(leftProperty, comparableValue); break;
            case Operation.LessThanOrEqualTo: body = Expression.LessThanOrEqual(leftProperty, comparableValue); break;
        }
        finalExpression = body;
        return this;
    }

    public ExpressionBuilder<T> And<Y>(string left, Operation operationType, Y value)
    {

        var result = BuildExpression<Y>(left, operationType, value);
        finalExpression = Expression.And(finalExpression, result.finalExpression);

        return this;
    }

    public Expression<Func<T, bool>> Commit()
    {
        return Expression.Lambda<Func<T, bool>>(finalExpression, parameter); ;
    }

    public ExpressionBuilder<T> Or<Y>(string left, Operation operationType, Y value)
    {
        var result = BuildExpression<Y>(left, operationType, value);
        finalExpression = Expression.Or(finalExpression, result.finalExpression);
        return this;
    }

}