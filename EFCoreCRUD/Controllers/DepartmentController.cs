using EFCoreCodeFirstLib;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreCRUD.Controllers
{
    public class DepartmentController : Controller
    {
        DatabaseContext _db;
        public DepartmentController(DatabaseContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var departments = (from dept in _db.Departments
                               where dept.DeptId > 0
                               select dept).ToList();
            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department model)
        {
            try
            {
                _db.Departments.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch { }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Department department = _db.Departments.Find(id);
            return View("Create", department);
        }

        [HttpPost]
        public IActionResult Edit(Department model)
        {
            try
            {
                _db.Departments.Update(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch { }
            return View("Create", model);
        }

        public IActionResult Delete(int id)
        {
            Department department = _db.Departments.Find(id);
            if (department != null)
            {
                _db.Departments.Remove(department);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
