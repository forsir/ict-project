using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using Forsir.IctProject.BusinessLayer.Mapping;
using Forsir.IctProject.BusinessLayer.Services;
using Forsir.IctProject.DataLayer.Repositories;
using Forsir.IctProject.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

		public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddDbContext<IctProjectContext>(options =>
					options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
#if DEBUG
						.UseLoggerFactory(MyLoggerFactory)
#endif
					);

			services.AddIdentity<IdentityUser, IdentityRole>()
				.AddEntityFrameworkStores<IctProjectContext>();

			string secret = Configuration.GetSection("JwtConfig").GetSection("secret").Value;
			byte[] key = Encoding.ASCII.GetBytes(secret);
			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(x =>
				{
					x.TokenValidationParameters = new TokenValidationParameters
					{
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidIssuer = "localhost",
						ValidAudience = "localhost"
					};
				});

			services.AddControllers();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			// database initialization
			using (IServiceScope serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				IServiceProvider services = serviceScope.ServiceProvider;

				IctProjectContext context = services.GetRequiredService<IctProjectContext>();
				IEnumerable<string> migrations = context.Database.GetPendingMigrations();
				if (migrations.Any())
				{
					context.Database.Migrate();
				}
				DbInitializer.Initialize(context);
			}

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthentication();
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

			containerBuilder.RegisterAssemblyTypes(typeof(IService).Assembly)
				.Where(t => (t.Name != null) && (t.Namespace?.EndsWith(nameof(Forsir.IctProject.BusinessLayer.Services)) == true))
				.AsImplementedInterfaces();

			containerBuilder.RegisterAssemblyTypes(typeof(Repository<>).Assembly)
				.Where(t => (t.Name != null) && (t.Namespace?.EndsWith(nameof(Forsir.IctProject.DataLayer.Repositories)) == true))
				.AsImplementedInterfaces();
		}
	}
}
