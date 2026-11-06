using E_Commerce.Domain.Common;
using E_Commerce.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class BaseSpecificatopns<TEntity, Tkey> : ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IcludeExpressions { get; } = [];

        public Expression<Func<TEntity, bool>> criteria { get; private set; }

        public Expression<Func<TEntity, object>>? orderBy { get; private set; }

        public Expression<Func<TEntity, object>>? orderByBescending { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        public BaseSpecificatopns(Expression<Func<TEntity, bool>> expression)
        {
            criteria = expression;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IcludeExpressions.Add(expression);
        }

        protected void AddOrderBy(Expression<Func<TEntity, object>> expression)
        {
            orderBy = expression;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> expression)
        {
            orderByBescending = expression;
        }

        protected void ApplyPagination(int pageIndex, int PageSize)
        {
            Take = PageSize;
            Skip = (pageIndex - 1) * PageSize;
            IsPaginated = true; 
        }

    }
}
