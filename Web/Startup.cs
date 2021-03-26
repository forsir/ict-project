using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Forsir.IctProject.BusinessLayer.Facades;
using Forsir.IctProject.BusinessLayer.Mapping;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public ILifetimeScope AutofacContainer { get; private set; }

		public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<OctProjectContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
#if DEBUG
						.UseLoggerFactory(MyLoggerFactory)
#endif
					);

			//services.AddDefaultIdentity<IdentityUser>()
			//	//.AddDefaultUI(UIFramework.Bootstrap4)
			//	.AddEntityFrameworkStores<OctProjectContext>();

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		public void ConfigureContainer(ContainerBuilder containerBuilder)
		{
			// init AutoMapper
			containerBuilder.RegisterAutoMapper(typeof(AuthorMapping).Assembly);

			//IContainer container = containerBuilder.Build();
			//MapperConfiguration mapperConfiguration = container.Resolve<MapperConfiguration>();
			//mapperConfiguration.AssertConfigurationIsValid();

			containerBuilder.RegisterAssemblyTypes(typeof(IFacade).Assembly)
				.Where(t => (t.Name != null) && t.Namespace.EndsWith(nameof(Forsir.IctProject.BusinessLayer.Facades)))
				.AsImplementedInterfaces();

			containerBuilder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
				.Where(t => (t.Name != null) && (t.Namespace.EndsWith(nameof(Forsir.IctProject.DataLayer.Repositories))))
				.AsImplementedInterfaces();
		}
	}
}
