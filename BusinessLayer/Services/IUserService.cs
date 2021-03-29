using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;

namespace Forsir.IctProject.BusinessLayer.Services
{
	public interface IUserService : IService
	{
		Task<UserDetail> GetUserDetailAsync(string email);

		Task<bool> IsValidUserCredentialsAsync(string userName, string password);
	}
}