using System.Collections.Generic;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;

namespace Forsir.IctProject.BusinessLayer.Services
{
	public interface IAuthorService
	{
		Task<List<AuthorsList>> GetListAsync();

		Task<AuthorDetail> GetAuthor(int id);

		Task SaveAsync(AuthorEdit authorEdit);

		Task DeleteAuthor(int id);
	}
}