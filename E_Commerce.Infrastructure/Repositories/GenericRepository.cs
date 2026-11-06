using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Specifications;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, Tkey>(StoreDbContext context) : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public void Add(TEntity entity) => context.Set<TEntity>().Add(entity);

        public async Task<int> CountAsync(ISpecifications<TEntity, Tkey> specs, CancellationToken ct = default)
        {
            return await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), specs).CountAsync(ct);
        }

        public void Delete(TEntity entity) => context.Set<TEntity>().Remove(entity);

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
            => await context.Set<TEntity>().ToListAsync();

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecifications<TEntity, Tkey> specs, CancellationToken ct = default)
        {
            return await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), specs).ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct = default)
            => await context.Set<TEntity>().FindAsync(id, ct);


        public async Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> specs, CancellationToken ct = default)
        {
            return await SpecificationEvaluator.CreateQuery(context.Set<TEntity>(), specs).FirstOrDefaultAsync(ct);
        }

        public void Update(TEntity entity) => context.Set<TEntity>().Update(entity);
    }
}
