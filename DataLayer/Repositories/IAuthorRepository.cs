using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forsir.IctProject.Repository.Data.Model;

namespace Forsir.IctProject.DataLayer.Repositories
{
	public interface IAuthorRepository : IRepository<Author>
	{
		Task<List<Author>> GetAllActorsAsync(Func<IQueryable<Author>, IQueryable<Author>> func);

		Task<Author> GetAuthorAsync(int authorId);
	}
}