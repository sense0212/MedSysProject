﻿using MedSysProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedSysProject.Controllers
{
    public class BlogsController : Controller
    {
        private readonly MedSysContext _db;

        public BlogsController(MedSysContext db)
        {
            _db = db;
        }

        /// <summary>
        /// 主畫面
        /// </summary>
        /// <returns></returns>
        public IActionResult IndexOld() 
        {
            

            return View();
        }

        public IActionResult SinglePost(int? BlogID) 
        {
            return View();
        }
        public IActionResult SelectBlogCategory(int? CategoryID) 
        {//
         //var q = _db.BlogCategories.Where(c => c.BlogClassId == CategoryID);
         //var q = _db.Blogs.Include(e => e.Employee).Include(e => e.ArticleClass.BlogCategory1).Where(c => c.ArticleClassId == CategoryID);
            IEnumerable<Blog> q = null;

            if (CategoryID == null)
            {
                q = from blog in _db.Blogs.Include(e => e.Employee)
                    .Include(e => e.ArticleClass)
                    select blog;

                ViewBag.Cate = "";
            }
            else
            {
                q = _db.Blogs.Include(e => e.Employee).Include(e => e.ArticleClass).Where(c => c.ArticleClassId == CategoryID);
                ViewBag.Cate = "分類：" + (q.ToList())[0].ArticleClass.BlogCategory1;
            }
            

            return View(q);
        }

        public IActionResult GetBlogImageByte(int? id)
        {
            Blog emp = _db.Blogs.Find(id);
            byte[]? img = emp?.BlogImage;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

        public IActionResult GetEmpImageByte(int? id)
        {
            Employee emp = _db.Employees.Find(id);
            byte[]? img = emp?.EmployeePhoto;

            if (img != null)
            {
                return File(img, "image/jpeg");
            }
            return NotFound();
        }

    }
}
