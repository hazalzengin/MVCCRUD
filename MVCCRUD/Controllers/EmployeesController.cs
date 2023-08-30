using Microsoft.AspNetCore.Mvc;
using MVCCRUD.Models;
using MVCCRUD.Models.Domain;
using MVCCRUD.Data;
using Microsoft.EntityFrameworkCore;
using MVCCRUD.Data;
using MVCCRUD.Models.Domain;
using MVCCRUD.Models;

namespace ASPNETMVCCRUD.Controllers
{
    public class Employees : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;
        public Employees(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Name = addEmployeeRequest.Name,
                Surname = addEmployeeRequest.Surname,
                Departman = addEmployeeRequest.Departman
            };
            mvcDemoDbContext.Employees.Add(employee);
            mvcDemoDbContext.SaveChanges();
            return RedirectToAction("Index");

        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if (employee != null)
            {
                var viewmodel = new UpdateEmployeeViewModels()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Surname = employee.Surname,
                    Departman = employee.Departman
                };
                return await Task.Run(() => View("View", viewmodel));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModels model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Surname = model.Surname;
                employee.Departman = model.Departman;

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(UpdateEmployeeViewModels model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);
            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


    }
}
