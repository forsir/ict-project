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
	[Route("[controller]")]
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
		public async Task<IEnumerable<AuthorsList>> Index()
		{
			List<AuthorsList> result = await authorFacade.GetListAsync();
			return result.ToArray();
		}

		// GET: Actors/Details/5
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
	}
}
