using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataLayerTests
{
	[TestClass]
	public abstract class TestBase
	{
		public IctProjectContext? ictProjectContext;

		[TestInitialize]
		public void Initialize()
		{
			DbContextOptions<IctProjectContext> options = new DbContextOptionsBuilder<IctProjectContext>().UseInMemoryDatabase("IctProject").Options;

			ictProjectContext = new IctProjectContext(options);

			OnTestInitialize();
		}

		protected virtual void OnTestInitialize()
		{
		}

		[TestCleanup]
		public void Cleanup()
		{
			ictProjectContext?.Dispose();
		}
	}
}
