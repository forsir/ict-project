using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Forsir.IctProject.BusinessLayer.Models;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository;
using Microsoft.EntityFrameworkCore;

namespace Forsir.IctProject.BusinessLayer.Facades
{
	public class AuthorFacade : IAuthorFacade
	{
		private readonly IAuthorRepository authorRepository;
		private readonly OctProjectContext context;

		public AuthorFacade(IAuthorRepository authorRepository, OctProjectContext context)
		{
			this.authorRepository = authorRepository;
			this.context = context;
		}

		public async Task<List<AuthorsList>> GetListAsync()
		{
			List<Repository.Data.Model.Author> list = await context.Authors.Include(a => a.Books).OrderBy(a => a.Name).ToListAsync();
			return Mapper.Map<List<AuthorsList>>(list);
		}

		//public async Task<AuthorDetail> GetAuthor(int id)
		//{
		//	var actor = await actorRepository.GetEntityAsync(id, false, i => i.Include(a => a.Parts).ThenInclude(p => p.Block).ThenInclude(p => p.Stage).ThenInclude(p => p.Play));
		//	var actorGrouped = actor.Parts.GroupBy(p => p.Block)
		//		.Select(g => new { Id = g.Key.Id, Name = g.Key.Name, Order = g.Key.Order, OriginalStage = g.Key.Stage, Parts = Mapper.Map<List<ActorPartModel>>(g) })
		//		.GroupBy(b => b.OriginalStage)
		//		.Select(g => new { Key = Mapper.Map<StageListModel>(g.Key), List = g.ToList() })
		//		.Select(g => new ActorStageModel { Id = g.Key.Id, Play = g.Key.Play, PlayName = g.Key.PlayName, Order = g.Key.Order, StillPlaying = g.Key.StillPlaying, Blocks = Mapper.Map<List<ActorBlockModel>>(g.List) })
		//		.ToList();

		//	var actorDisplayModel = Mapper.Map<ActorDisplayModel>(actor);
		//	actorDisplayModel.Stages = actorGrouped;

		//	return actorDisplayModel;
		//}

		//public async Task SaveEditModelAsync(ActorEditModel actorEditModel)
		//{
		//	Actor actor = actorEditModel.Id == null ? new Actor() : await actorRepository.GetEntityAsync(actorEditModel.Id.Value, true);
		//	Mapper.Map<ActorEditModel, Actor>(actorEditModel, actor);

		//	if (actorEditModel.Id == null)
		//	{
		//		context.Actors.Add(actor);
		//	}
		//}
	}
}
