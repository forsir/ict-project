using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository;
using Forsir.IctProject.Repository.Data.Model;
using Microsoft.EntityFrameworkCore;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public class AuthorFacade : IAuthorFacade
	{
		private readonly IAuthorRepository authorRepository;
		private readonly OctProjectContext context;
		private readonly IMapper mapper;

		public AuthorFacade(IAuthorRepository authorRepository, OctProjectContext context, IMapper mapper)
		{
			this.authorRepository = authorRepository;
			this.context = context;
			this.mapper = mapper;
		}

		public async Task<List<AuthorsList>> GetListAsync()
		{
			List<Author> list = await context.Authors.Include(a => a.Books).OrderBy(a => a.Name).ToListAsync();
			return mapper.Map<List<AuthorsList>>(list);
		}

		public async Task<AuthorDetail> GetAuthor(int id)
		{
			Author author = await authorRepository.GetAuthorAsync(id);
			return mapper.Map<AuthorDetail>(author);
		}

		public async Task SaveAsync(AuthorEdit authorEdit)
		{
			Author author = authorEdit.Id == null ? new Author() : await authorRepository.GetEntityAsync(authorEdit.Id.Value, true);
			mapper.Map<AuthorEdit, Author>(authorEdit, author);

			if (authorEdit.Id == null)
			{
				context.Authors.Add(author);
			}
			await authorRepository.SaveChangesAsync();
		}
	}
}
