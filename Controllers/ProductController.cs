using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewWebShopApp.Models;
using System.Diagnostics;
using System.Runtime.Intrinsics.X86;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.AspNetCore.Hosting.Server;
using System.IO;
using NewWebShopApp.Areas.Identity.Data;

namespace NewWebShopApp.Controllers
{
    public class ProductController : Controller
    {
        AppDBContext db;
        IWebHostEnvironment _appEnvironment;

        public ProductController(AppDBContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;

            //Додавання даних
            if (!db.Categories.Any())
            {
                Category watermelon = new Category { name = "Кавун" };
                Category eggplant = new Category { name = "Баклажан" };
                Category melon = new Category { name = "Диня" };
                Category squash = new Category { name = "Кабачок" };

                Product watermelon1 = new Product { name = "Кавун1", img = "../img/watermelon/Фото_Кавуна1.jpg", price = 15, box = 20, Category = watermelon };
                Product eggplant1 = new Product { name = "Баклажан1", img = "../img/eggplant/Фото_Баклажану1.jpg", price = 20, box = 25, Category = eggplant };
                Product melon1 = new Product { name = "Диня1", img = "../img/melon/Фото_Дині1.jpg", price = 25, box = 15, Category = melon };
                Product squash1 = new Product { name = "Кабачок1", img = "../img/squash/Фото_Кабачку1.jpg", price = 30, box = 20, Category = squash };

                db.Categories.AddRange(watermelon, eggplant, melon, squash);
                db.Products.AddRange(watermelon1, eggplant1, melon1, squash1);
                db.SaveChanges();
            }
        }

        public async Task<IActionResult> Index(string name, int category = 0, int page = 1,
            SortState sortOrder = SortState.NameAsc)
        {
            int pageSize = 3;

            //фильтрация
            IQueryable<Product> products = db.Products.Include(x => x.Category);

            if (category != 0)
            {
                products = products.Where(p => p.CategoryId == category);
            }
            if (!string.IsNullOrEmpty(name))
            {
                products = products.Where(p => p.name!.Contains(name));
            }

            // сортировка
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    products = products.OrderByDescending(s => s.name);
                    break;
                case SortState.PriceAsc:
                    products = products.OrderBy(s => s.price);
                    break;
                case SortState.PriceDesc:
                    products = products.OrderByDescending(s => s.price);
                    break;
                case SortState.CategoryAsc:
                    products = products.OrderBy(s => s.Category!.name);
                    break;
                case SortState.CategoryDesc:
                    products = products.OrderByDescending(s => s.Category!.name);
                    break;
                default:
                    products = products.OrderBy(s => s.name);
                    break;
            }

            // пагинация
            var count = await products.CountAsync();
            var items = await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // формируем модель представления
            IndexViewModel viewModel = new IndexViewModel(
                items,
                new PageViewModel(count, page, pageSize),
                new FilterViewModel(db.Categories.ToList(), category, name),
                new SortViewModel(sortOrder)
            );
            
            return View(viewModel);
        }

        /*Додавання продукту*/
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "id", "name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product prod, IFormFile uploadedFile)
        {
            ViewBag.Categories = new SelectList(db.Categories, "id", "name");

            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/ImgFiles/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                prod.img = path;
            }

            if (prod.name != null && prod.price != 0 && prod.box != 0 && prod.img != null)
            {
                db.Products.Add(prod);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Create");
        }

        /*--------------------------------------------*/
        /*Видалення продукту*/
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                Product prod = new Product { id = id.Value };
                db.Entry(prod).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
        /*---------------------------------------------*/

        /*Редагування продукту*/
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = new SelectList(db.Categories, "name", "name");
            if (id != null)
            {
                Product? prod = await db.Products.FirstOrDefaultAsync(p => p.id == id); 
                if (prod != null) return View(prod);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product prod, IFormFile uploadedFile, int? id)
        {
            ViewBag.Categories = new SelectList(db.Categories, "name", "name");

            if (uploadedFile != null)
            {
                // путь к папке Files
                string path = "/ImgFiles/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                prod.img = path;
            }

            if (prod.name != null && prod.price != 0 && prod.box != 0)
            {
                db.Products.Update(prod);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View("Edit");
        }
        /*---------------------------------------------*/
    }
}
