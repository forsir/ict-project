using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Forsir.IctProject.DataLayer.Data.Model;

namespace Forsir.IctProject.Repository.Data.Model
{
	public class Book : IDataModel
	{
		public int Id { get; set; }

		[MaxLength(200)]
		[Required]
		public string Name { get; set; } = String.Empty;

		public List<Author> Authors { get; set; } = new List<Author>();
	}
}
