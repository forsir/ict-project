using System.Collections.Generic;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public interface IAuthorFacade
	{
		Task<List<AuthorsList>> GetListAsync();

		Task<AuthorDetail> GetAuthor(int id);

		Task SaveAsync(AuthorEdit authorEdit);
	}
}