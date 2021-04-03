using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forsir.IctProject.BusinessLayer.Models
{
	public class AuthorEdit
	{
		public int? Id { get; set; }

		[Required]
		[MaxLength(200)]
		public string Name { get; set; }

		[Required]
		public List<int> BookIds { get; set; }

		public AuthorEdit(string name, List<int> bookIds)
		{
			Name = name;
			BookIds = bookIds;
		}
	}
}
