using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forsir.IctProject.Repository.Data.Model
{
	public class Book
	{
		public int Id { get; set; }

		[MaxLength(200)]
		[Required]
		public string Name { get; set; }

		public List<Author> Authors { get; set; } = new List<Author>();
	}
}
