using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forsir.IctProject.Repository.Data.Model
{
	public class Author
	{
		public int Id { get; set; }

		[MaxLength(200)]
		[Required]
		public string Name { get; set; }

		public List<Book> Books { get; set; } = new List<Book>();
	}
}
