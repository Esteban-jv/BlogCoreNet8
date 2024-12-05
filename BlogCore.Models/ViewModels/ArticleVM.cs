using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models.ViewModels
{
	public class ArticleVM
	{
		public Article Article { get; set; }

		public IEnumerable<SelectListItem>? CategoriesList { get; set; }
	}
}
