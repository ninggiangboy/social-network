using System.Linq.Expressions;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SharedKernel.SeedWorks;

namespace Application.Utils;

public abstract class CursorHandler<T, TResult>(CursorPageRequest cursor)
{
    public abstract Dictionary<string, Expression<Func<T, object>>> SortByMap { get; }
    protected abstract Dictionary<string, Expression<Func<TResult, object>>> ResponseSortByMap { get; }
    protected abstract string DefaultSortKey { get; }

    public string GetSortKey()
    {
        return cursor.SortBy ?? DefaultSortKey;
    }

    public Expression<Func<T, object>> GetSortKeySelector()
    {
        return SortByMap.TryGetValue(GetSortKey(), out var value)
            ? value
            : SortByMap[DefaultSortKey];
    }

    private Expression<Func<TResult, object>> GetResponseSortKeySelector()
    {
        return ResponseSortByMap.TryGetValue(GetSortKey(), out var value)
            ? value
            : ResponseSortByMap[DefaultSortKey];
    }

    public string? GetNextToken(
        List<TResult> list
    )
    {
        if (list.Count == 0)
        {
            return null;
        }

        if (list.Count < cursor.Limit)
        {
            return null;
        }

        var lastKeyValue = GetResponseSortKeySelector().Compile().Invoke(list.Last());
        var keyString = lastKeyValue!.ToString();
        var tokenBytes = Encoding.UTF8.GetBytes(keyString!);
        var nextToken = Convert.ToBase64String(tokenBytes);

        return nextToken;
    }

    public Expression<Func<T, bool>> GetCursorWhereClause()
    {
        if (string.IsNullOrEmpty(cursor.NextToken))
        {
            return x => true;
        }

        var decodedCursor = Encoding.UTF8.GetString(Convert.FromBase64String(cursor.NextToken));
        var sortKeySelector = GetSortKeySelector();
        var cursorValue = Convert.ChangeType(decodedCursor, sortKeySelector.Body.Type);

        var parameter = sortKeySelector.Parameters[0];
        var comparison = cursor.IsAscending
            ? Expression.GreaterThan(sortKeySelector.Body, Expression.Constant(cursorValue))
            : Expression.LessThan(sortKeySelector.Body, Expression.Constant(cursorValue));

        return Expression.Lambda<Func<T, bool>>(comparison, parameter);
    }
}