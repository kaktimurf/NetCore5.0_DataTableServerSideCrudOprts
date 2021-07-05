using Microsoft.AspNetCore.Mvc;
using ServerSide.Data;
using ServerSide.Models.DatabaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ServerSide.Controllers
{
    public class EntitiesController : Controller
    {
        public IActionResult Index()
        {
            Type type = typeof(ApplicationDbContext);
            IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties(); //NET v4.0 BindingFlags.Public | BindingFlags.CreateInstance
            Dictionary<string, List<string>> model = new Dictionary<string, List<string>>();
            foreach (var item in properties)
            {
                if (item.Name == "Products")
                {
                    model.Add("Product", GetProperties(typeof(ProductController)));
                }
                else if (item.Name == "Categories")
                {
                    model.Add("Categories", GetProperties(typeof(Category)));
                }

            }

            return View(model);
        }

        public List<string> GetProperties(Type type)
        {

            IEnumerable<PropertyInfo> properties = type.GetRuntimeProperties(); //NET v4.0 BindingFlags.Public | BindingFlags.CreateInstance
            List<string> data = new();

            foreach (var item in properties)
            {
                if (item.Name != "Products")
                {
                    data.Add(item.Name);
                }

            }

            return data;
        }
    }
}
