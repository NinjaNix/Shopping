using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>>? Criteria { get; } //Where
    Expression<Func<T, object>>? OrderBy { get; } //Sorting
    Expression<Func<T, object>>? OrderByDescending { get; }
    bool IsDitinct { get; } //Distinct
    int Take { get; } //Take
    int Skip { get; } //Skip
    bool IsPagingEnabled { get; } //Paging
    IQueryable<T> ApplyCriteria(IQueryable<T> query);
}

public interface ISpecification<T, TResult> : ISpecification<T>
{
    Expression<Func<T,TResult>>? Select { get; } //Projection
}
