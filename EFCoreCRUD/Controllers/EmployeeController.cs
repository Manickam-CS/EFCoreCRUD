using EFCoreCodeFirstLib;
using EFCoreCRUD.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace EFCoreCRUD.Controllers
{
    public class EmployeeController : Controller
    {
        DatabaseContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private string UploadedFile(IFormFile file)
        {
            try
            {
                string uniqueFileName = null;

                if (file != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                return uniqueFileName;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public EmployeeController(DatabaseContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {     
            ViewBag.Departments = _db.Departments.ToList();
            var employees = (from emp in _db.Employees
                             where emp.EmpId > 0
                             select emp).ToList();          
            return View(employees);
        }

        public IActionResult Create()
        {         
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        public IActionResult Details (int id)
        {
            Employee employee = _db.Employees.Find(id);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            try
            {
                string uniqueFileName = UploadedFile(form.Files["ProfileImage"]);

                Employee model = new Employee();
                model.Name = form.ContainsKey("Name") ? form["Name"].First() : null;
                model.Address = form.ContainsKey("Address") ? form["Address"].First() : null;
                model.DeptId = form.ContainsKey("DeptId") ? Convert.ToInt32(form["DeptId"].First()) : 0;
                model.ImagePath = uniqueFileName;

                if (ModelState.IsValid)
                {
                    _db.Employees.Add(model);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        public IActionResult Edit(int id)
        {
            Employee employee = _db.Employees.Find(id);
            ViewBag.Departments = _db.Departments.ToList();
            return View("Create", employee);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {           
            Employee model = new Employee();
            try
            {
                string uniqueFileName = UploadedFile(form.Files["ProfileImage"]);              
                model.EmpId = form.ContainsKey("EmpId") ? Convert.ToInt32(form["EmpId"].First()) : 0;
                model.Name = form.ContainsKey("Name") ? form["Name"].First() : null;
                model.Address = form.ContainsKey("Address") ? form["Address"].First() : null;
                model.DeptId = form.ContainsKey("DeptId") ? Convert.ToInt32(form["DeptId"].First()) : 0;
                model.ImagePath = uniqueFileName ?? (form.ContainsKey("ImagePath") ? form["ImagePath"].First() : null);

                if (ModelState.IsValid)
                {
                    _db.Employees.Update(model);
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

            }
            ViewBag.Departments = _db.Departments.ToList();
            return View("Create", model);
        }    


        public IActionResult Delete(int id)
        {
            Employee employee = _db.Employees.Find(id);
            if (employee != null)
            {
                _db.Employees.Remove(employee);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
