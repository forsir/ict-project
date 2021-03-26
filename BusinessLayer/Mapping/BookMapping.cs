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
	public class BookMapping : Profile
	{
		public BookMapping()
		{
			CreateMap<Book, BooksList>();

			CreateMap<Book, BookDetail>();
		}
	}
}
