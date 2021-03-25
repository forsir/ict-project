using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forsir.IctProject.Repository
{
	public static class DbInitializer
	{
		public static void Initialize(OctProjectContext context)
		{
			context.Database.EnsureCreated();

			// set database initialization data
			context.SaveChanges();
		}
	}
}
