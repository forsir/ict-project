using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Forsir.IctProject.BusinessLayer.Models
{
	public class AuthorEdit
	{
		public int? Id { get; set; }

		public string Name { get; set; } = String.Empty;

		public List<int> BookIds { get; set; } = new List<int>();
	}
}
