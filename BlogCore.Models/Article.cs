using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
	public class Article
	{
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [Display(Name = "Título")]
		public string Title { get; set; }

		[Required(ErrorMessage = "La descripción es obligatoria")]
		public string Description { get; set; }

		[Display(Name = "Fecha de creación")]
		public DateTime CreateDate { get; set; }

		[DataType(DataType.ImageUrl)]
		[Display(Name = "Imagen")]
		public string UrlImage { get; set; }

		[Required(ErrorMessage = "La categoría es obligatoria")]
		public int CategoryId { get; set; }
		[ForeignKey("CategoryId")]
		public Category Category { get; set; } //?
	}
}
