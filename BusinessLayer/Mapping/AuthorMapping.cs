using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.Repository.Data.Model;

namespace Forsir.IctProject.BusinessLayer.Mapping
{
	public class AuthorMapping : Profile
	{
		public AuthorMapping()
		{
			CreateMap<Author, AuthorsList>();

			CreateMap<Author, AuthorDetail>();

			CreateMap<AuthorEdit, Author>()
				.ForMember(a => a.Id, m => m.Ignore())
				.ForMember(a => a.Books, m => m.Ignore());
		}
	}
}
