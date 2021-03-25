using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository.Data.Model;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public class BookFacade : IBookFacade
	{
		private readonly IBookRepository bookRepository;

		public BookFacade(IBookRepository bookRepository)
		{
			this.bookRepository = bookRepository;
		}

		public async Task<List<BooksList>> GetListAsync()
		{
			List<Book> list = await bookRepository.GetListAsync(false).ConfigureAwait(false);
			return Mapper.Map<List<AuthorsList>>(list);
		}
	}
}
