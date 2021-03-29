using System.Collections.Generic;
using System.Linq;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository;
using Forsir.IctProject.Repository.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataLayerTests
{
	[TestClass]
	public class AuthorTests : TestBase
	{
		[TestMethod]
		public void Author_AddAndGet()
		{
			// arange
			string authorName = "name";
			AuthorTestFixture fixture = new AuthorTestFixture(ictProjectContext!)
				.CreateAndAddAuthor(authorName);
			fixture.SaveChanges();

			// act
			List<Author> authors = fixture.AuthorRepository.GetListAsync(false).Result;

			//assert
			Assert.AreEqual(1, authors.Count);
			Assert.AreEqual(authorName, authors.First().Name);
		}

		private class AuthorTestFixture
		{

			public Author? Author { get; set; }

			public IAuthorRepository AuthorRepository { get; set; }

			private readonly IctProjectContext _ictProjectContext;

			public AuthorTestFixture(IctProjectContext ictProjectContext)
			{
				_ictProjectContext = ictProjectContext;
				AuthorRepository = new AuthorRepository(_ictProjectContext);
			}

			public AuthorTestFixture CreateAndAddAuthor(string name)
			{
				Author = new Author();
				Author.Name = name;
				_ictProjectContext.Authors.Add(Author);

				return this;
			}

			public void SaveChanges()
			{
				_ictProjectContext.SaveChanges();
			}
		}
	}
}
