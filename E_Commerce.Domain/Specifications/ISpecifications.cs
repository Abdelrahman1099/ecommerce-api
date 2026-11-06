using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Specifications
{
    public interface ISpecifications<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
         ICollection<Expression<Func<TEntity, object>>> IcludeExpressions { get;}
        Expression<Func<TEntity, bool>> criteria { get;}
        Expression<Func<TEntity, object>>? orderBy { get;}
        Expression<Func<TEntity, object>>? orderByBescending { get;}
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; }
    }
}
