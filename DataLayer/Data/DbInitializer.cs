using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Forsir.IctProject.Repository
{
	public static class DbInitializer
	{
		public static void Initialize(IctProjectContext context)
		{
			context.Database.EnsureCreated();

			if (!context.Users.Any())
			{
				context.Users.Add(new IdentityUser("admin"));
			}

			// set database initialization data
			context.SaveChanges();
		}
	}
}
