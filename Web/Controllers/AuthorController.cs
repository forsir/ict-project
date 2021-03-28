using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Facades;
using Forsir.IctProject.BusinessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.Controllers
{
	[ApiController]
	public class AuthorController : ControllerBase
	{
		private readonly ILogger<AuthorController> logger;
		private readonly IAuthorFacade authorFacade;

		public AuthorController(ILogger<AuthorController> logger, IAuthorFacade authorFacade)
		{
			this.logger = logger;
			this.authorFacade = authorFacade;
		}

		[HttpGet]
		[Route("Authors")]
		public async Task<IEnumerable<AuthorsList>> Index()
		{
			List<AuthorsList> result = await authorFacade.GetListAsync();
			return result.ToArray();
		}

		[HttpGet]
		[Route("Author/{id}")]
		public async Task<AuthorDetail> Details(int? id)
		{
			if (id == null)
			{
				throw new Exception("Not found");
			}

			AuthorDetail authorDetail = await authorFacade.GetAuthor(id.Value);

			if (authorDetail == null)
			{
				throw new Exception("Not found");
			}

			return authorDetail;
		}

		[HttpGet]
		[Route("Author/Create/")]
		public async Task Create(AuthorEdit authorEdit)
		{
			if (authorEdit == null)
			{
				throw new Exception("Not found");
			}

			await authorFacade.SaveAsync(authorEdit);
		}

		[HttpGet]
		[Route("Author/Delete/{id}")]
		public async Task Delete(int id)
		{
			await authorFacade.DeleteAuthor(id);
		}
	}
}
