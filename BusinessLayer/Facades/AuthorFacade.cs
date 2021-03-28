using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public class AuthorFacade : IAuthorFacade
	{
		private readonly IAuthorRepository authorRepository;
		private readonly IMapper mapper;

		public AuthorFacade(IAuthorRepository authorRepository, IMapper mapper)
		{
			this.authorRepository = authorRepository;
			this.mapper = mapper;
		}

		public async Task<List<AuthorsList>> GetListAsync()
		{
			List<Author> list = await authorRepository.GetListAsync(false, include: i => i.Include(s => s.Books), orderBy: o => o.OrderBy(a => a.Name));
			return mapper.Map<List<AuthorsList>>(list);
		}

		public async Task<AuthorDetail> GetAuthor(int id)
		{
			Author author = await authorRepository.GetAuthorAsync(id);
			return mapper.Map<AuthorDetail>(author);
		}

		public async Task DeleteAuthor(int id)
		{
			Author author = await authorRepository.GetAuthorAsync(id);
			authorRepository.Delete(author);
			await authorRepository.SaveChangesAsync();
		}

		public async Task SaveAsync(AuthorEdit authorEdit)
		{
			Author author = authorEdit.Id == null ? new Author() : await authorRepository.GetEntityAsync(authorEdit.Id.Value, true);
			mapper.Map<AuthorEdit, Author>(authorEdit, author);

			if (authorEdit.Id == null)
			{
				await authorRepository.AddAsync(author);
			}
			await authorRepository.SaveChangesAsync();
		}
	}
}
