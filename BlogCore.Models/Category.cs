﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage="Ingrese un nombre para la categoría")]
        [Display(Name = "Nombre de categoría")]
        public string Name { get; set; }

        [Display(Name = "Orden de visualización")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser mayor o igual a 0")]
        public int? Order { get; set; }
    }
}
