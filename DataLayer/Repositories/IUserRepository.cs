using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Forsir.IctProject.DataLayer.Repositories
{
	public interface IUserRepository : IRepository
	{
		Task<IdentityUser> GetUserAsync(string email);
	}
}