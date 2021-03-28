﻿using System;
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
		private readonly IMapper mapper;

		public BookFacade(IBookRepository bookRepository, IMapper mapper)
		{
			this.bookRepository = bookRepository;
			this.mapper = mapper;
		}

		public async Task<List<BooksList>> GetListAsync()
		{
			List<Book> list = await bookRepository.GetListAsync(false).ConfigureAwait(false);
			return mapper.Map<List<BooksList>>(list);
		}

		public async Task DeleteBook(int id)
		{
			Book book = await bookRepository.GetEntityAsync(id, false);
			bookRepository.Delete(book);
			await bookRepository.SaveChangesAsync();
		}

		public async Task SaveAsync(BookEdit bookEdit)
		{
			Book book = bookEdit.Id == null ? new Book() : await bookRepository.GetEntityAsync(bookEdit.Id.Value, true);
			mapper.Map<BookEdit, Book>(bookEdit, book);

			if (bookEdit.Id == null)
			{
				await bookRepository.AddAsync(book);
			}
			await bookRepository.SaveChangesAsync();
		}
	}
}
