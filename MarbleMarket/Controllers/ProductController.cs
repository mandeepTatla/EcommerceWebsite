using MarbleMarket.Data;
using MarbleMarket.Models;
using MarbleMarket.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarbleMarket.Controllers
{
    [Authorize(Roles =WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            // what i did first i get list of all product and the with help of
            // foreach i get the category id and application id this will result in
            // to much database call making application slower **** Therefore used Eager loading**** 
            IEnumerable<Product> objList = _db.Product.Include(u => u.Category).Include(u => u.ApplicationType);
        

            //foreach(var obj in objList)
            //{
            //    obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            //    obj.ApplicationType = _db.ApplicationType.FirstOrDefault(u => u.Id == obj.ApplicationTypeId);
               
            //};
            return View(objList);
        }

        //GET - Upsert
        public IActionResult Upsert(int? id)
        {
            ////reterving all of the categories from the database  and then converting them into selectList Item so that we can
            //// have them in enumberable object and the n dispaly them in dropdown
            //IEnumerable<SelectListItem> CategoryDropDown = _db.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()
            //});
            //// pass the categoryDropDown to the view 
            //ViewBag.dropdown = CategoryDropDown;



            //create product object to check id is null, 
            // Product product = new Product();


            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                
                CatergorySelectList = _db.Category.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ApplicationTypeList = _db.ApplicationType.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })



            };
            if (id == null)
            {
                //this is for create as we don't pass the id as it is autogenrated and we can 
                // return an empty view or empty product
                return View(productVM);
            }
            else
            {
                // if id is not null we need to retrieve the input data from database and pass it to the view 
             productVM.Product = _db.Product.Find(id);
                if(productVM.Product == null)
                {
                    return NotFound();
                
                }
                else
                {
                    return View(productVM);
                }
              
            }
          
          
        }
        
       
        // post-Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj)
        {
            if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(obj.Product.Id==0)
                {
                    //creating
                    string upload = webRootPath + WC.Imagepath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    obj.Product.ImageUrl = fileName + extension;

                    _db.Product.Add(obj.Product);

                }
                else
                {
                    // updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == obj.Product.Id);

                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.Imagepath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);
                        var oldfile = Path.Combine(upload, objFromDb.ImageUrl);

                        if (System.IO.File.Exists(oldfile))
                        {
                            System.IO.File.Delete(oldfile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        obj.Product.ImageUrl = fileName + extension;
                    }
                    else
                    {
                        obj.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                    _db.Product.Update(obj.Product);

                }
                _db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            obj.CatergorySelectList = _db.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

       
            return View(obj);
        
        }


      
     

      


        //GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product =  _db.Product.Find(id);

       
            if (product == null)
            {
                return NotFound();
            }



            return View(product);
        }

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.Product.Find(id);
           if(obj == null)
            {
                return NotFound();
            }

            string upload =_webHostEnvironment.WebRootPath + WC.Imagepath;
           
           
            var oldfile = Path.Combine(upload, obj.ImageUrl);

            if (System.IO.File.Exists(oldfile))
            {
                System.IO.File.Delete(oldfile);
            }

            _db.Product.Remove(obj);
                _db.SaveChanges();
        
            return RedirectToAction("Index");



        }




    }



}
