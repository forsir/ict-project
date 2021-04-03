using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Forsir.IctProject.Web.Controllers
{
	[Authorize]
	[ApiController]
	public class AuthorController : ControllerBase
	{
		private readonly ILogger<AuthorController> logger;
		private readonly IAuthorService authorService;

		public AuthorController(ILogger<AuthorController> logger, IAuthorService authorService)
		{
			this.logger = logger;
			this.authorService = authorService;
		}

		[HttpGet]
		[Route("Authors")]
		public async Task<ActionResult<IEnumerable<AuthorsList>>> Index()
		{
			List<AuthorsList> result = await authorService.GetListAsync();
			return Ok(result.ToArray());
		}

		[HttpGet]
		[Route("Author/{id}")]
		public async Task<ActionResult<AuthorDetail>> Details(int? id)
		{
			if (id == null)
			{
				return BadRequest("Object not found");
			}

			AuthorDetail authorDetail = await authorService.GetAuthor(id.Value);

			if (authorDetail == null)
			{
				// Only for eample
				return StatusCode(StatusCodes.Status400BadRequest, "Object not found");
			}

			return Ok(authorDetail);
		}

		[HttpPost]
		[Route("Author/Create/")]
		public async Task<IActionResult> Create([FromBody] AuthorEdit authorEdit)
		{
			try
			{
				if ((authorEdit == null) || !ModelState.IsValid)
				{
					return BadRequest(ModelState.Values.SelectMany(v => v.Errors));
				}

				await authorService.SaveAsync(authorEdit);
			}
			catch (Exception e)
			{
				logger.LogError(e, $"{nameof(AuthorController)}.{nameof(Create)} ({JsonSerializer.Serialize(authorEdit)})");
				return BadRequest("Cannot create");
			}
			return Ok();
		}

		[HttpDelete]
		[Route("Author/Delete/{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await authorService.DeleteAuthor(id);
				return Ok();
			}
			catch (Exception e)
			{
				logger.LogError(e, $"{nameof(AuthorController)}.{nameof(Delete)} ({id})");
				return BadRequest("Cannot delete");
			}
		}
	}
}
