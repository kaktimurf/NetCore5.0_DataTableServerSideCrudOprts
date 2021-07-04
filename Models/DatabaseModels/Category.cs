using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Models.DatabaseModels
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string Description { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public ICollection<Product> Products { get; set; }
    }
}
