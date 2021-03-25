using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.Repository.Data.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Forsir.IctProject.Repository
{
	public class OctProjectContext : IdentityDbContext
	{
		public DbSet<Book> Books { get; set; }

		public DbSet<Author> Authors { get; set; }

		public OctProjectContext()
		{
		}

		public OctProjectContext(DbContextOptions<OctProjectContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			this.ConfigureBook(builder.Entity<Book>());
			this.ConfigureAuthor(builder.Entity<Author>());
		}

		private void ConfigureBook(EntityTypeBuilder<Book> builder)
		{
			builder.HasKey(l => l.Id);

			builder.HasMany(p => p.Authors).WithMany(s => s.Books);
		}

		private void ConfigureAuthor(EntityTypeBuilder<Author> builder)
		{
			builder.HasKey(l => l.Id);

			builder.HasMany(p => p.Books).WithMany(s => s.Authors);
		}
	}
}
