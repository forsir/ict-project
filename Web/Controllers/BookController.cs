using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.BusinessLayer.Services;
using Microsoft.AspNetCore.Mvc;

namespace Forsir.IctProject.Web.Controllers
{
	public class BookController
	{
		private readonly IBookService bookService;

		public BookController(IBookService bookService)
		{
			this.bookService = bookService;
		}

		[HttpGet]
		[Route("Books")]
		public async Task<IEnumerable<BooksList>> Index()
		{
			List<BooksList> result = await bookService.GetListAsync();
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

			BookDetail bookDetail = await bookService.GetBookAsync(id.Value);

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

			await bookService.SaveAsync(bookEdit);
		}

		[HttpGet]
		[Route("Book/Delete/{id}")]
		public async Task Delete(int id)
		{
			await bookService.DeleteBook(id);
		}
	}
}
