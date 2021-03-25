using System.Collections.Generic;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public interface IAuthorFacade
	{
		Task<List<AuthorsList>> GetListAsync();
	}
}