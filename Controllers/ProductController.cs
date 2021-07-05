using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RandomGen;
using ServerSide.Data;
using ServerSide.Extensions;
using ServerSide.Models.AuxiliaryModels;
using ServerSide.Models.DatabaseModels;
using ServerSide.Models.ViewModels.ProductModels;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ServerSide.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await SeedData();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody] DtParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            //search parametresi boş ise Id ye göre artan sıralama yapacağız
            var orderCriteria = "Id";
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {

                //bu örnekte sadece 1. sütunda varsayılan sıralama yapıyoruz
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }

            var result = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||  
                                           r.Description != null && r.Description.ToUpper().Contains(searchBy.ToUpper())||
                                           r.Price.ToString() !=null &&r.Price.ToString().ToUpper().Contains(searchBy.ToUpper())||
                                           r.Stock.ToString() != null && r.Stock.ToString().ToUpper().Contains(searchBy.ToUpper())
                                                      );
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _context.Products.CountAsync();

            return Json(new DtResult<Product>
            {
                Draw = dtParameters.Draw,
                RecordsTotal = totalResultsCount,
                RecordsFiltered = filteredResultsCount,
                Data = await result
                    .Skip(dtParameters.Start)
                    .Take(dtParameters.Length)
                    .ToListAsync()
            });
        }

        public async Task SeedData()
        {
            if (!_context.Products.Any())
            {
                for (var i = 0; i < 1000; i++)
                {
                    await _context.Products.AddAsync(new Product
                    {
                        Name = i % 2 == 0 ? Gen.Random.Names.Male()() : Gen.Random.Names.Female()(),
                        Description = Gen.Random.Text.Short()(),
                        Price=Gen.Random.Numbers.Decimals(0,1000)(),
                        Stock = Gen.Random.Numbers.Integers(0, 100)(),
                        CreationDate = Gen.Random.Time.Dates(DateTime.Now.AddYears(-100), DateTime.Now)(),
                        CategoryId = Gen.Random.Numbers.Integers(1, 20)()
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        public IActionResult GetModal()
        {
            var categories = _context.Categories.Where(c=>c.Id<10).ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return PartialView("_PartialAddProduct",new Product());
        }


        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
           
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.Id < 10).ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {
                
                var result = await _context.Products.AddAsync(product);
                if (result.State == EntityState.Added)
                {
                    await _context.SaveChangesAsync();
                    var productAddAjaxModel = JsonSerializer.Serialize(new ProductAddAjaxViewModel
                    {
                        //Category = category,
                        PartialAddProduct = await this.RenderViewToStringAsync("_PartialAddProduct", product)
                    });
                    return Json(productAddAjaxModel);
                }
            }
            var productAddAjaxErrorModel = JsonSerializer.Serialize(new ProductAddAjaxViewModel
            {
                PartialAddProduct = await this.RenderViewToStringAsync("_PartialAddProduct", product)
            });
            return Json(productAddAjaxErrorModel);

        }

        [HttpPost]
        public void Delete(int productId)
        {
            var product = _context.Products.Where(p => p.Id == productId).ToList();
            var result = _context.Products.Remove(product[0]);
            _context.SaveChanges();

        }

        [HttpGet]
        public IActionResult Update(int productId)
        {
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.Id < 10).ToList(), "Id", "Name");
            var result = _context.Products.Where(c => c.Id == productId).ToList();
            if (result.Count > 0)
            {
                return PartialView("_PartialUpdateProduct", result[0]);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            ViewBag.Categories = new SelectList(_context.Categories.Where(c => c.Id < 10).ToList(), "Id", "Name");
            if (ModelState.IsValid)
            {

                var result = _context.Products.Update(product);
               
                if (result.State == EntityState.Modified)
                {
                    await _context.SaveChangesAsync();
                    var productUpdateAjaxModel = JsonSerializer.Serialize(new ProductUpdateAjaxViewModel
                    {
                        PartialUpdateProduct = await this.RenderViewToStringAsync("_PartialUpdateProduct", result.Entity)
                    });
                    return Json(productUpdateAjaxModel);
                }
            }
            var productUpdateAjaxErrorModel = JsonSerializer.Serialize(new ProductUpdateAjaxViewModel
            {
                PartialUpdateProduct = await this.RenderViewToStringAsync("_CategoryUpdatePartial", product)
            });
            return Json(productUpdateAjaxErrorModel);

        }
    }
}
