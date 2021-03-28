using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.Repository;
using Forsir.IctProject.Repository.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Forsir.IctProject.DataLayer.Repositories
{
	public class AuthorRepository : Repository<Author>, IAuthorRepository
	{
		public AuthorRepository(OctProjectContext context) : base(context)
		{
		}

		public async Task<List<Author>> GetAllAuthorsAsync(Func<IQueryable<Author>, IQueryable<Author>> func)
		{
			if (func != null)
			{
				return await func(context.Authors).ToListAsync().ConfigureAwait(false);
			}
			else
			{
				return await context.Authors.ToListAsync();
			}
		}

		public async Task<Author> GetAuthorAsync(int authorId)
		{
			return await context.Authors.FirstAsync(s => s.Id == authorId);
		}
	}
}
