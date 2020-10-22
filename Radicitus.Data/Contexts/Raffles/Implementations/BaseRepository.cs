using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Radicitus.Data.Contexts.Raffles.Implementations
{
    public abstract class BaseRepository<TContext, TDbEntity> : DbContext where TContext : DbContext
    {
        public readonly TContext Context;
        public BaseRepository(TContext context)
        {
            Context = context;
        }

        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            return Context.Add(entity);
        }

        public override EntityEntry Add(object entity)
        {
            return Context.Add(entity);
        }

        public override ValueTask<EntityEntry<TEntity>> AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            return Context.AddAsync(entity, cancellationToken);
        }

        public override ValueTask<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken = default)
        {
            return Context.AddAsync(entity, cancellationToken);
        }

        public override void AddRange(params object[] entities)
        {
            Context.AddRange(entities);
        }

        public override void AddRange(IEnumerable<object> entities)
        {
            Context.AddRange(entities);
        }

        public override Task AddRangeAsync(params object[] entities)
        {
            return Context.AddRangeAsync(entities);
        }

        public override Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
        {
            return Context.AddRangeAsync(entities, cancellationToken);
        }

        public override EntityEntry<TEntity> Attach<TEntity>(TEntity entity)
        {
            return Context.Attach(entity);
        }

        public override EntityEntry Attach(object entity)
        {
            return Context.Attach(entity);
        }

        public override void AttachRange(params object[] entities)
        {
            Context.AttachRange(entities);
        }

        public override void AttachRange(IEnumerable<object> entities)
        {
            Context.AttachRange(entities);
        }

        public override object Find(Type entityType, params object[] keyValues)
        {
            return Context.Find(entityType, keyValues);
        }

        public override TEntity Find<TEntity>(params object[] keyValues)
        {
            return Context.Find<TEntity>(keyValues);
        }

        public override ValueTask<object> FindAsync(Type entityType, params object[] keyValues)
        {
            return Context.FindAsync(entityType, keyValues);
        }

        public override ValueTask<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken)
        {
            return Context.FindAsync(entityType, keyValues, cancellationToken);
        }

        public override ValueTask<TEntity> FindAsync<TEntity>(params object[] keyValues)
        {
            return Context.FindAsync<TEntity>(keyValues);
        }

        public override ValueTask<TEntity> FindAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken)
        {
            return Context.FindAsync<TEntity>(keyValues, cancellationToken);
        }

        public override EntityEntry<TEntity> Remove<TEntity>(TEntity entity)
        {
            return Context.Remove(entity);
        }

        public override EntityEntry Remove(object entity)
        {
            return Context.Remove(entity);
        }

        public override void RemoveRange(params object[] entities)
        {
            Context.RemoveRange(entities);
        }

        public override void RemoveRange(IEnumerable<object> entities)
        {
            Context.RemoveRange(entities);
        }

        public override int SaveChanges()
        {
            return Context.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return Context.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override EntityEntry<TEntity> Update<TEntity>(TEntity entity)
        {
            return Context.Update(entity);
        }

        public override EntityEntry Update(object entity)
        {
            return Context.Update(entity);
        }

        public override void UpdateRange(params object[] entities)
        {
            Context.UpdateRange(entities);
        }

        public override void UpdateRange(IEnumerable<object> entities)
        {
            Context.UpdateRange(entities);
        }
    }
}
