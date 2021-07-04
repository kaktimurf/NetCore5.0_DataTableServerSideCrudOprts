using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RandomGen;
using ServerSide.Data;
using ServerSide.Extensions;
using ServerSide.Models.AuxiliaryModels;
using ServerSide.Models.DatabaseModels;
using ServerSide.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerSide.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
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

            var result = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.Description != null && r.Description.ToUpper().Contains(searchBy.ToUpper())
                                                      );
            }

            result = orderAscendingDirection ? result.OrderByDynamic(orderCriteria, DtOrderDir.Asc) : result.OrderByDynamic(orderCriteria, DtOrderDir.Desc);

            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = await result.CountAsync();
            var totalResultsCount = await _context.Categories.CountAsync();

            return Json(new DtResult<Category>
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
            if (!_context.Categories.Any())
            {
                for (var i = 0; i < 1000; i++)
                {
                    await _context.Categories.AddAsync(new Category
                    {
                        Name = i % 2 == 0 ? Gen.Random.Names.Male()() : Gen.Random.Names.Female()(),
                        Description = Gen.Random.Text.Short()(),
                        CreationDate = Gen.Random.Time.Dates(DateTime.Now.AddYears(-100), DateTime.Now)()
                    });
                }

                await _context.SaveChangesAsync();
            }
        }

        public IActionResult GetModal()
        {
            return PartialView("_PartialAddCategory");
        }

        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
            if (ModelState.IsValid)
            {
                var result = await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
                if (result.State==EntityState.Added)               
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        //Category = category,
                        PartialAddCategory = await this.RenderViewToStringAsync("_PartialAddCategory", category)
                    });
                    return Json(categoryAddAjaxModel);
                }
            }
            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                PartialAddCategory = await this.RenderViewToStringAsync("_PartialAddCategory", category)
            });
            return Json(categoryAddAjaxErrorModel);

        }

        [HttpPost]
        public void Delete(int categoryId)
        {
            var category = _context.Categories.Where(c => c.Id == categoryId).ToList();
            var result =  _context.Categories.Remove(category[0]);
             _context.SaveChanges();
           
        }

        [HttpGet]
        public IActionResult Update(int categoryId)
        {
            var result =  _context.Categories.Where(c=>c.Id==categoryId).ToList();
            if (result.Count>0)
            {
                return PartialView("_PartialUpdateCategory", result[0]);
            }
            else
            {
                return NotFound();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Update(Category category)
        {
            if (ModelState.IsValid)
            {
                
                var result = _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                if (result.State == EntityState.Modified)
                {
                    var categoryUpdateAjaxModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
                    {
                        PartialUpdateCategory = await this.RenderViewToStringAsync("_PartialUpdateCategory", result.Entity)
                    });
                    return Json(categoryUpdateAjaxModel);
                }
            }
            var categoryUpdateAjaxErrorModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel
            {
                PartialUpdateCategory = await this.RenderViewToStringAsync("_CategoryUpdatePartial", category)
            });
            return Json(categoryUpdateAjaxErrorModel);

        }
    }
}
