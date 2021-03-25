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

		public IContainer ApplicationContainer { get; private set; }

		public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

		// This method gets called by the runtime. Use this method to add services to the container.
		public IServiceProvider ConfigureServices(IServiceCollection services)
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

			ConfigureAutofac(services);

			return new AutofacServiceProvider(this.ApplicationContainer);
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

		private void ConfigureAutofac(IServiceCollection services)
		{
			// Create the container builder.
			var containerBuilder = new ContainerBuilder();
			ConfigureAutomapper(containerBuilder);

			containerBuilder.RegisterAssemblyTypes(typeof(IFacade).Assembly)
				.Where(t => (t.Name != null) && t.Namespace.EndsWith(nameof(Forsir.IctProject.BusinessLayer.Facades)))
				.AsImplementedInterfaces();

			//.RegisterAssemblyTypes(assembly)
			//.AssignableTo<IFacade>()
			//.AsImplementedInterfaces()
			//.InstancePerRequest();

			containerBuilder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
				.Where(t => (t.Name != null) && (t.Namespace.EndsWith(nameof(Forsir.IctProject.DataLayer.Repositories))))
				.AsImplementedInterfaces();

			containerBuilder.Populate(services);
			this.ApplicationContainer = containerBuilder.Build();
		}

		private void ConfigureAutomapper(ContainerBuilder containerBuilder)
		{
			//var assembly = Assembly.GetAssembly(typeof(IRepository<>));
			var assembly = Assembly.GetExecutingAssembly();

			// init AutoMapper
			containerBuilder.RegisterAutoMapper(typeof(Program).Assembly);

			IContainer container = containerBuilder.Build();
			MapperConfiguration mapperConfiguration = container.Resolve<MapperConfiguration>();

			// this line will throw when mappings are not working as expected
			// it's wise to write a test for that, which is always executed within a CI pipeline for your project.
			mapperConfiguration.AssertConfigurationIsValid();
		}
	}
}
