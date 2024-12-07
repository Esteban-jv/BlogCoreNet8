using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BlogCore.Models
{
	public class Slider
	{
		[Key]
		public int Id { get; set; }

		[Required(ErrorMessage = "El nombre es obligatorio")]
		[Display(Name = "Nombre")]
		public string Name { get; set; }

		[Display(Name = "Estado")]
		[DefaultValue(true)]
		public bool Status { get; set; }

		[DataType(DataType.ImageUrl)]
		[Display(Name = "Imagen")]
		public string? UrlImage { get; set; }
	}
}
