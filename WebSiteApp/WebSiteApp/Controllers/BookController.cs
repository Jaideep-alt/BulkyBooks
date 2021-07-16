using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebSiteApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WebSiteApp.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _Db;

        public BookController(ApplicationDbContext _db)
        {
            _Db = _db;
        }

        [BindProperty]
        public Book Book { get; set; }

        [HttpGet]
        public IActionResult GetBooks()
        {
            var BookList = _Db.Book.ToList<Book>();
            return View(BookList);
        }

        public IActionResult Create(Book Obj)
        {

            return View(Obj);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(Book Obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Obj.ID == 0)
                    {
                        await _Db.Book.AddAsync(Obj);
                        await _Db.SaveChangesAsync();
                    }
                    else
                    {
                        _Db.Entry(Obj).State = EntityState.Modified;
                        await _Db.SaveChangesAsync();
                    }
                }
                return RedirectToAction("GetBooks");

            }
            catch (Exception)
            {
                return RedirectToAction("GetBooks");
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            
            if(id == null)
            {
                return RedirectToAction("GetBooks");
            }

            var bookFromDb = await _Db.Book.FindAsync(id);
            return View(bookFromDb);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Book model)
        {
            if (ModelState.IsValid)
            {
                _Db.Update(model);
                await _Db.SaveChangesAsync();
                return RedirectToAction("GetBooks");
            }
            return RedirectToAction("GetBooks");
        }

        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var Book = await _Db.Book.FindAsync(id);
                if (Book != null)
                {
                    _Db.Book.Remove(Book);
                    await _Db.SaveChangesAsync();
                }
                else
                {
                    return NotFound();
                }
                return RedirectToAction("GetBooks");
            }
            catch (Exception)
            {
                return RedirectToAction("GetBooks");
            }
            
        }



        //-------------JSON Return-----------------



        [HttpGet]
        public async Task<IActionResult> GetAllDataApiJson()
        {
            return Json(new { data = await _Db.Book.ToListAsync() });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteByDataApiJson(int id)
        {
            var rowdata = await _Db.Book.FindAsync(id);
            if(rowdata == null)
            {
                return Json(new { success = false, message = "Data Not Found!!" });
            }
            _Db.Book.Remove(rowdata);
            await _Db.SaveChangesAsync();
            return Json(new { success = true, message = "Successfully Deleted!!" });
        }

    }
}