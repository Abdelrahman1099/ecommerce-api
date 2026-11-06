using E_Commerce.Domain.Common;
using E_Commerce.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<Tentity> CreateQuery<Tentity, TKey>(IQueryable<Tentity> inputQuery, ISpecifications<Tentity, TKey> specs) where Tentity : BaseEntity<TKey>
        {
            var query = inputQuery;

            if(specs.criteria is not null)
            {
                query = query.Where(specs.criteria);
            }

            if(specs.IcludeExpressions.Any())
            {
                query =  specs.IcludeExpressions.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
            }
            if (specs.orderBy is not null)
            {
                query = query.OrderBy(specs.orderBy);
            }
            else if (specs.orderByBescending is not null)
            {
                query = query.OrderByDescending(specs.orderByBescending);
            }
            if(specs.IsPaginated)
            {
                query = query.Skip(specs.Skip).Take(specs.Take);
            }
                return query;
        }
    }
}
