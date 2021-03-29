using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.DataLayer.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Forsir.IctProject.BusinessLayer.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository userRepository;
		private readonly IMapper mapper;

		public UserService(IUserRepository userRepository, IMapper mapper)
		{
			this.userRepository = userRepository;
			this.mapper = mapper;
		}

		public async Task<UserDetail> GetUserDetailAsync(string email)
		{
			IdentityUser user = await userRepository.GetUserAsync(email);
			return mapper.Map<UserDetail>(user);
		}

		public async Task<bool> IsValidUserCredentialsAsync(string userName, string password)
		{
			IdentityUser user = await userRepository.GetUserAsync(userName);
			return user != null;
		}
	}
}
