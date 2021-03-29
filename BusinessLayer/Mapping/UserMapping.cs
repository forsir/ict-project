using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Microsoft.AspNetCore.Identity;

namespace Forsir.IctProject.BusinessLayer.Mapping
{
	public class UserMapping : Profile
	{
		public UserMapping()
		{
			CreateMap<IdentityUser, UserDetail>();
		}
	}
}
