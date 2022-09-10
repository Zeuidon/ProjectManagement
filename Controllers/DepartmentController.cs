using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL;
using ProjectManagement.Models;
using ProjectManagement.Models.ViewModels;

namespace ProjectManagement.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ProjectContext projectDbContext;

        public DepartmentController(ProjectContext ProjectDbContext)
        {
            projectDbContext = ProjectDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var departments = await projectDbContext.Departments.ToListAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddDepartmentViewModel addDepartmentRequest)
        {
            var department = new Department()
            {
                DepartmentID = Guid.NewGuid(),
                DepartmentName = addDepartmentRequest.DepartmentName,
                DepartmentHead = addDepartmentRequest.DepartmentHead,
            };

            await projectDbContext.Departments.AddAsync(department);
            await projectDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var department = await projectDbContext.Departments.FirstOrDefaultAsync(x => x.DepartmentID == id);

            if (department != null)
            {
                var viewModel = new UpdateDepartmentViewModel()
                {
                    DepartmentID = department.DepartmentID,
                    DepartmentName = department.DepartmentName,
                    DepartmentHead = department.DepartmentHead,
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateDepartmentViewModel model)
        {
            var department = await projectDbContext.Departments.FindAsync(model.DepartmentID);

            if (department != null)
            {
                department.DepartmentName = model.DepartmentName;
                department.DepartmentHead = model.DepartmentHead;

                await projectDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> Delete(UpdateDepartmentViewModel model)
        {
            var department = await projectDbContext.Departments.FindAsync(model.DepartmentID);

            if(department != null)
            {
                projectDbContext.Departments.Remove(department);
                await projectDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
