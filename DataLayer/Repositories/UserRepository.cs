using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forsir.IctProject.DataLayer.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IctProjectContext _context;

		public UserRepository(IctProjectContext context)
		{
			_context = context;
		}

		public async Task<IdentityUser> GetUserAsync(string name)
		{
			return await _context.Users.FirstOrDefaultAsync(u => u.UserName == name);
		}
	}
}
