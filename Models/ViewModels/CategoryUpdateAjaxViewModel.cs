using ServerSide.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Models.ViewModels
{
    public class CategoryUpdateAjaxViewModel
    {
        public Category Category { get; set; }
        public string PartialUpdateCategory { get; set; }
    }
}
