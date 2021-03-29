using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Forsir.IctProject.DataLayer.Repositories
{
	public interface IRepository<T>
			where T : class
	{
		Task AddAsync(T entity);

		void Delete(T entity);

		Task<T> GetEntityAsync(Expression<Func<T, bool>> whereExpression, bool tracking, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

		Task<T> GetEntityAsync(int id, bool tracking, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);

		Task<List<T>> GetListAsync(
			   bool tracking,
			Expression<Func<T, bool>>? whereExpression = null,
			Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
			Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null);

		Task SaveChangesAsync();

		IQueryable<T> GetListQuery();
	}
}
