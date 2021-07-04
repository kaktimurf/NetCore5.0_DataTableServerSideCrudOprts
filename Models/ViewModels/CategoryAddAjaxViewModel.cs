using ServerSide.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerSide.Models.ViewModels
{
    public class CategoryAddAjaxViewModel
    {
        public Category Category { get; set; }
        public string PartialAddCategory { get; set; }
    }
}
