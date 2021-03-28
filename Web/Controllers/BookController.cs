using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Facades;
using Forsir.IctProject.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
	public class BookController
	{
		private readonly IBookFacade bookFacade;

		public BookController(IBookFacade bookFacade)
		{
			this.bookFacade = bookFacade;
		}

		[HttpGet]
		[Route("Books")]
		public async Task<IEnumerable<BooksList>> Index()
		{
			List<BooksList> result = await bookFacade.GetListAsync();
			return result.ToArray();
		}

		[HttpGet]
		[Route("Book/{id}")]
		public async Task<BookDetail> Details(int? id)
		{
			if (id == null)
			{
				throw new Exception("Not found");
			}

			BookDetail bookDetail = await bookFacade.GetBookAsync(id.Value);

			if (bookDetail == null)
			{
				throw new Exception("Not found");
			}

			return bookDetail;
		}

		[HttpGet]
		[Route("Book/Create/")]
		public async Task Create(BookEdit bookEdit)
		{
			if (bookEdit == null)
			{
				throw new Exception("Not found");
			}

			await bookFacade.SaveAsync(bookEdit);
		}

		[HttpGet]
		[Route("Book/Delete/{id}")]
		public async Task Delete(int id)
		{
			await bookFacade.DeleteBook(id);
		}
	}
}
