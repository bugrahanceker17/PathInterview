﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PathInterview.Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> where TEntity : class, new() where TContext : DbContext, new()
    {
        public async Task<int> Add(TEntity entity)
        {
            await using TContext context = new TContext();
            EntityEntry<TEntity> addedEntity = context.Entry(entity);
            addedEntity.State = EntityState.Added;
            return await context.SaveChangesAsync();
        }

        public async Task<int> Delete(TEntity entity)
        {
            await using TContext context = new TContext();
            EntityEntry<TEntity> deletedEntity = context.Entry(entity);
            deletedEntity.State = EntityState.Deleted;
            return await context.SaveChangesAsync();
        }

        public async Task<TEntity> Get(Expression<Func<TEntity, bool>> filter)
        {
            await using TContext context = new TContext();
            return await context.Set<TEntity>().SingleOrDefaultAsync(filter);
        }

        public async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            await using TContext context = new TContext();
            return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
        }

        public async Task<int> Update(TEntity entity)
        {
            await using TContext context = new TContext();
            EntityEntry<TEntity> updatedEntity = context.Entry(entity);
            updatedEntity.State = EntityState.Modified;
            return await context.SaveChangesAsync();
        }
    }
}