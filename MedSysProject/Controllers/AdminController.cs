﻿using MedSysProject.Models;
using MedSysProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Linq;
using System.IO;
using NuGet.Protocol;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MedSysProject.Controllers
{
    public class AdminController : Controller
    {
        private MedSysContext _db;
        private readonly IWebHostEnvironment _host;
       
       


        public AdminController(MedSysContext db, IWebHostEnvironment host)
        {
            _db = db;
            _host = host;
        }

        public IActionResult Service()
        {

            return View();
        }


        public IActionResult Index()
        {
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_EMPLOYEE_LOGIN))
                return View();

            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(CLoginViewModel vm)
        {

            Employee? emp = _db.Employees.FirstOrDefault(
                t => t.EmployeeEmail.Equals(vm.txtEmail) && t.EmployeePassWord.Equals(vm.txtPassWord));

            if (emp != null && emp.EmployeePassWord.Equals(vm.txtPassWord))
            {
                string json = JsonSerializer.Serialize(emp);
                HttpContext.Session.SetString(CDictionary.SK_EMPLOYEE_LOGIN, json);
                return RedirectToAction("Index");
            }
            return View();
        }
    
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult EmpManager(CKeywordViewModel vm)
        {
            IEnumerable<Employee> datas = null;

            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                //datas = from t in _db.Employees.Include(p=>p.EmployeeClass)
                datas = from t in _db.Employees.Include(p => p.EmployeeClass)
                        select t;
            }

            else
                datas = _db.Employees.Where(p => p.EmployeeName.Contains(vm.txtKeyword) ||
                p.EmployeePhoneNum.Contains(vm.txtKeyword) ||
                p.EmployeeEmail.Contains(vm.txtKeyword));
            return View(datas);
        }

        public IActionResult EmpCreate()
        {
            return View();
        }



        public IActionResult EmpClass()
        {
            return View();
        }

        public IActionResult BlogIndex(int? id,CKeywordViewModel input) 
        {
            IEnumerable<Blog> blogs = null;
            if (id == null)
            {
                if (string.IsNullOrEmpty(input.txtKeyword))
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee)
                                   .Include(blog => blog.ArticleClass)
                            select blog;
                }
                else
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee).Include(blog => blog.ArticleClass)
                            where blog.Title.Contains(input.txtKeyword) ||
                                  blog.Employee.EmployeeName.Contains(input.txtKeyword) ||
                                  blog.ArticleClass.BlogCategory1.Contains(input.txtKeyword)
                            select blog;
                }
            }
            else 
            {
                if (string.IsNullOrEmpty(input.txtKeyword))
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee)
                          .Include(blog => blog.ArticleClass)
                            where blog.EmployeeId == id
                            select blog;
                }
                else 
                {
                    blogs = from blog in _db.Blogs.Include(blog => blog.Employee)
                                  .Include(blog => blog.ArticleClass)
                            where (blog.EmployeeId == id) && (blog.Title.Contains(input.txtKeyword) ||
                                                          blog.Employee.EmployeeName.Contains(input.txtKeyword) ||
                                                          blog.ArticleClass.BlogCategory1.Contains(input.txtKeyword))
                            select blog;
                }
            }
            return View(blogs);
        }

        
        public IActionResult BlogList(int? id,CKeywordViewModel vm) 
        {//
            IEnumerable<CBlogModel> blogs = null;
            if (id == null)
            {
                if (string.IsNullOrEmpty(vm.txtKeyword))
                {
                    blogs = from blog in _db.Blogs
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }
                else 
                {
                    blogs = from blog in _db.Blogs
                            where blog.Title.Contains(vm.txtKeyword) ||
                                  blog.Employee.EmployeeName.Contains(vm.txtKeyword) ||
                                  blog.ArticleClass.BlogCategory1.Contains(vm.txtKeyword)
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }
                
            }
            else {
                if (string.IsNullOrEmpty(vm.txtKeyword))
                {
                    blogs = from blog in _db.Blogs
                            where blog.EmployeeId == id
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }
                else 
                {
                    blogs = from blog in _db.Blogs
                            where (blog.EmployeeId == id)&&(blog.Title.Contains(vm.txtKeyword) ||
                                  blog.Employee.EmployeeName.Contains(vm.txtKeyword) ||
                                  blog.ArticleClass.BlogCategory1.Contains(vm.txtKeyword))
                            select new CBlogModel
                            {
                                BlogID = blog.BlogId,
                                Title = blog.Title,
                                ArticleClassID = blog.ArticleClassId,
                                Category = blog.ArticleClass.BlogCategory1,
                                Views = blog.Views,
                                CreatedAt = blog.CreatedAt,
                                Content = blog.Content,
                                BlogImage = blog.BlogImage,
                                AuthorID = blog.EmployeeId,
                                AuthorName = blog.Employee.EmployeeName
                            };
                }
                
            }
            return View(blogs);
        }

        public IActionResult Product(CKeywordViewModel vm)
        {
            var datas = _db.Products.AsQueryable();

            if (!string.IsNullOrEmpty(vm.txtKeyword))
            {
                var keyword = vm.txtKeyword.Trim();
                datas = datas.Where(p =>
                    p.ProductName.Contains(keyword) ||
                    p.Ingredient.Contains(keyword) ||
                    p.License.Contains(keyword)||
                    p.Description.Contains(keyword));
            }

            var defaultImagePath = "/img-product/default-image.jpg";

            if (vm.txtMinPrice.HasValue)
            {
                datas = datas.Where(p => p.UnitPrice.HasValue && p.UnitPrice.Value >= vm.txtMinPrice.Value);
            }

            if (vm.txtMaxPrice.HasValue)
            {
                datas = datas.Where(p => p.UnitPrice.HasValue && p.UnitPrice.Value <= vm.txtMaxPrice.Value);
            }


            var viewModel = datas.Select(product => new CProductsWrap
            {
                Product = product,
                ImagePath = product.FimagePath != null
                    ? Path.Combine("/img-product", product.FimagePath)
                    : defaultImagePath
            }).ToList();

            return View(viewModel);
        }



        public IActionResult Order()
        {
            //string keyword = "";
            IEnumerable<Order> datas = null;

            //if (string.IsNullOrEmpty(keyword))
            datas = from t in _db.Orders.Include(m => m.Member).Include(s => s.State)
                        select t;
            //else
            //    datas = db.Orders.Where(p => p.OrderDate.Contains(keyword));
            return View(datas);
        }

        public IActionResult Data()
        {
            return View();
        }

        public IActionResult Report(CKeywordViewModel vm)
        {
            IEnumerable<ReportDetail> datas = null;
            //List<CReportWrap> datas2 = null;
            //datas2 = new CReportWrap().Report();
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from s in _db.ReportDetails.Include(p => p.Item)
                        orderby s.ReportId
                         select s;
             
            else
                datas = _db.ReportDetails.Where(p =>
                p.ReportId.Equals(Convert.ToInt32(vm.txtKeyword)));
            return View(datas);
          

        }

        public IActionResult ReportDelete(int? id)
        {
            ReportDetail rd = _db.ReportDetails.FirstOrDefault(p => p.ReportDetailId == id);
            if (rd != null)
            {
                _db.ReportDetails.Remove(rd);
                _db.SaveChanges();
            }
            return RedirectToAction("Report");
        }

        public IActionResult ReportEdit(int? id)
        {
          
            ReportDetail rd = _db.ReportDetails.FirstOrDefault(p => p.ReportDetailId == id);
            if (rd == null)
                return RedirectToAction("Report");
            return View(rd);
        }

        [HttpPost]
        public IActionResult ReportEdit(CReportWrap pIn)
        {
            ReportDetail rdDb = _db.ReportDetails.FirstOrDefault(p => p.ReportDetailId == pIn.ReportDetailId);
            if (rdDb != null)
            {
                rdDb.ReportDetailId = pIn.ReportDetailId;
                rdDb.ItemId = pIn.ItemId;
                rdDb.Result = pIn.Result;
                _db.SaveChanges();
            }
            return RedirectToAction("Report");
        }


        public IActionResult GetImage(int id)
        {
            Product product = _db.Products.Find(id);

            if (product != null && !string.IsNullOrEmpty(product.FimagePath))
            {
                string imagePath = Path.Combine(_host.WebRootPath, "img-product", product.FimagePath);

                if (System.IO.File.Exists(imagePath))
                {
                    return PhysicalFile(imagePath, "image/jpeg");
                }
            }

            return NotFound();
        }






        public IActionResult Create()
        {
            ViewBag.Categories = _db.ProductsCategories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product, IFormFile formFile, int[] CategoriesIds)
        {
            // 檢查並創建目錄
            string imagePath = Path.Combine(_host.WebRootPath, "img-product");
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            if (formFile != null && formFile.Length > 0)
            {
                // 生成唯一的檔案名稱
                string photoName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);

                // 構建完整的檔案路徑
                string filePath = Path.Combine(imagePath, photoName);

                // 寫入檔案
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(fileStream);
                }

                // 更新資料庫中的路徑
                product.FimagePath = photoName;
            }

            // 儲存到資料庫
            _db.Products.Add(product);
            _db.SaveChanges();

            // 將產品與所選分類關聯
            if (CategoriesIds != null)
            {
                foreach (var categoryId in CategoriesIds)
                {
                    var classification = new ProductsClassification
                    {
                        ProductId = product.ProductId,
                        CategoriesId = categoryId
                    };

                    _db.ProductsClassifications.Add(classification);
                }

                _db.SaveChanges();
            }

            return RedirectToAction("Product");
        }

        //ajax版本待補

        //public IActionResult Create()
        //{
        //    return View();
        //}


        //[HttpPost]
        //public IActionResult Create(Product product)
        //{
        //    // 假設你已經在前端將圖片上傳到伺服器的路徑存在 product.FimagePath 中
        //    _db.Products.Add(product);
        //    _db.SaveChanges();

        //    return Json(new { productId = product.ProductId });
        //}

  

        public IActionResult Edit(int? id)
        {
            Product x = _db.Products.FirstOrDefault(p => p.ProductId == id);

            if (x == null)
                return RedirectToAction("Product");

            return View(x);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CProductsWrap productId, IFormFile formFile)
        {

            Product pDb = _db.Products.FirstOrDefault(p => p.ProductId == productId.WrappedProductId);
            if (pDb != null)
            {
                if (formFile != null)
                {
                    // 生成唯一的檔案名稱
                    string photoName = Guid.NewGuid().ToString() + ".jpg";

                    // 將圖片存放在 wwwroot/img-product 資料夾中
                    string filePath = Path.Combine(_host.WebRootPath, "img-product", photoName);

                    // 將圖片路徑存入資料庫
                    pDb.FimagePath = "/img-product/" + photoName;

                    // 寫入圖片檔案
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }
                }

                pDb.ProductName = productId.WrappedProductName;
                pDb.UnitsInStock = productId.WrappedUnitsInStock;
                pDb.License = productId.WrappedLicense;
                pDb.UnitPrice = productId.WrappedUnitPrice;
                pDb.Ingredient = productId.WrappedIngredient;
                pDb.Description = productId.WrappedDescription;
                pDb.Discontinued = productId.WrappedDiscontinued;

                _db.Products.Update(pDb);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // 處理異常，或輸出到日誌
                    Console.WriteLine(ex.Message);
                }
            }

            return RedirectToAction("Product");
        }

        public IActionResult Delete(int? id)
        {


            Product x = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (x != null)
            {
                // 刪除相關聯的 ProductsClassification 記錄
                var relatedClassification = _db.ProductsClassifications.FirstOrDefault(pc => pc.ProductId == id);
                if (relatedClassification != null)
                {
                    _db.ProductsClassifications.Remove(relatedClassification);
                }

                _db.Products.Remove(x);
                _db.SaveChanges();
            }
            return RedirectToAction("Product");

        }

        public IActionResult GetImageByte(int? id)
        {
           Employee emp = _db.Employees.Find(id);
            byte[]? img = emp?.EmployeePhoto;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public string GetMemberName(int? id)
        {
            Member member = _db.Members.Find(id);

            if (member != null)
            {
                return member.MemberName;
            }

            return "";
        }

        [HttpPost]
        public IActionResult ToggleDiscontinued(int productId, bool discontinued)
        {
            // 從資料庫中檢索產品
            Product product = _db.Products.Find(productId);

            if (product != null)
            {
                // 切換「上架」狀態
                product.Discontinued = !discontinued;

                // 將更改保存到資料庫
                _db.SaveChanges();

                // 返回 JSON 回應，指示新的狀態
                return Json(new { discontinued = product.Discontinued });
            }

            // 返回 JSON 回應，指示錯誤
            return Json(new { error = "找不到產品" });
        }

    }


}
