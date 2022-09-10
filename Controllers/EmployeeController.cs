using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL;
using ProjectManagement.Models;
using ProjectManagement.Models.ViewModels;

namespace ProjectManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ProjectContext projectDbContext;

        public EmployeeController(ProjectContext ProjectDbContext)
        {
            projectDbContext = ProjectDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await projectDbContext.Employees.ToListAsync();
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
                ID = Guid.NewGuid(),
                FirstName = addEmployeeRequest.FirstName,
                LastName = addEmployeeRequest.LastName,
                EmailId = addEmployeeRequest.EmailId,
                PhoneNo = addEmployeeRequest.PhoneNo,
                Position = addEmployeeRequest.Position,
                JoiningDate = addEmployeeRequest.JoiningDate,
            };

            await projectDbContext.Employees.AddAsync(employee);
            await projectDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await projectDbContext.Employees.FirstOrDefaultAsync(x => x.ID == id);

            if (employee != null)
            {
                var viewModel = new UpdateEmployeeViewModel()
                {
                    ID = employee.ID,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    EmailId = employee.EmailId,
                    PhoneNo = employee.PhoneNo,
                    Position = employee.Position,
                    JoiningDate = employee.JoiningDate,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> View(UpdateEmployeeViewModel model)
        {
            var employee = await projectDbContext.Employees.FindAsync(model.ID);
            
            if (employee != null)
            {
                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.EmailId = model.EmailId;
                employee.PhoneNo = model.PhoneNo;
                employee.Position = model.Position;
                employee.JoiningDate = model.JoiningDate;

                await projectDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateEmployeeViewModel model)
        {
            var employee = await projectDbContext.Employees.FindAsync(model.ID);

            if (employee != null)
            {
                projectDbContext.Employees.Remove(employee);
                await projectDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
