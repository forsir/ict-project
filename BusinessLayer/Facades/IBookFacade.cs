using System.Collections.Generic;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public interface IBookFacade
	{
		Task<List<BooksList>> GetListAsync();

		Task DeleteBook(int id);

		Task SaveAsync(BookEdit bookEdit);
	}
}