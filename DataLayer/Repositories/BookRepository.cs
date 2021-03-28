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
	public class BookRepository : Repository<Book>, IBookRepository
	{
		public BookRepository(IctProjectContext context) : base(context)
		{
		}
	}
}
