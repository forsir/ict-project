using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.DataLayer.Data.Model;
using Forsir.IctProject.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Forsir.IctProject.DataLayer.Repositories
{
	/// <summary>
	/// Base class for all common operations
	/// </summary>
	/// <typeparam name="T">Model type</typeparam>
	public class Repository<T> : IGenericRepository<T>
			where T : class, IDataModel
	{
		protected readonly IctProjectContext context;

		public Repository(IctProjectContext context)
		{
			this.context = context;
		}

		public async Task<List<T>> GetListAsync(
				bool tracking,
			Expression<Func<T, bool>>? whereExpression = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
		{
			IQueryable<T> query = context.Set<T>();
			if (!tracking)
			{
				query = query.AsNoTracking();
			}

			if (include != null)
			{
				query = include(query);
			}

			if (whereExpression != null)
			{
				query = query.Where(whereExpression);
			}

			if (orderBy != null)
			{
				return await orderBy(query).ToListAsync();
			}
			else
			{
				return await query.ToListAsync();
			}
		}

		protected Expression<Func<T, bool>> EntityWhereExpression(int id)
		{
			return model => model.Id == id;
		}

		public async Task<T> GetEntityAsync(Expression<Func<T, bool>> whereExpression, bool tracking, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
		{
			IQueryable<T> query = context.Set<T>();
			if (!tracking)
			{
				query = query.AsNoTracking();
			}

			if (include != null)
			{
				query = include(query);
			}

			if (whereExpression != null)
			{
				query = query.Where(whereExpression);
			}

			return await query.FirstOrDefaultAsync(whereExpression).ConfigureAwait(false);
		}

		public async Task<T> GetEntityAsync(int id, bool tracking, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
		{
			return await GetEntityAsync(EntityWhereExpression(id), tracking, include);
		}

		public async Task AddAsync(T entity)
		{
			await context.AddAsync(entity).ConfigureAwait(false);
		}

		public void Delete(T entity)
		{
			context.Remove(entity);
		}

		public async Task SaveChangesAsync()
		{
			await context.SaveChangesAsync().ConfigureAwait(false);
		}

		public IQueryable<T> GetListQuery()
		{
			return context.Set<T>();
		}
	}
}