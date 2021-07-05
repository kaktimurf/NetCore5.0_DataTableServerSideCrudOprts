using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Models.DatabaseModels
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage =("Name is required"))]
        [MinLength(3, ErrorMessage = "Category Name is minimum 3 character contains")]
        [MaxLength(20, ErrorMessage = "Category Name is maximum 50 character contains")]
        public string Name { get; set; }
        [Required(ErrorMessage = ("Description is required"))]
        [MinLength(3, ErrorMessage = "Descriptionis minimum 5 character contains")]
        [MaxLength(20, ErrorMessage = "Description is maximum 300 character contains")]
        public string Description { get; set; }
        [Required(ErrorMessage = ("Stock is required"))]
        public int Stock { get; set; }
        [Required(ErrorMessage = ("Price is required"))]
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        [Required(ErrorMessage = ("Category is required"))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
