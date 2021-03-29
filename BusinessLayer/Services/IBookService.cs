using System.Collections.Generic;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;

namespace Forsir.IctProject.BusinessLayer.Services
{
	public interface IBookService : IService
	{
		Task<List<BooksList>> GetListAsync();

		Task DeleteBook(int id);

		Task SaveAsync(BookEdit bookEdit);

		Task<BookDetail> GetBookAsync(int id);
	}
}