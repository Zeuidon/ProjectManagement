using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.DAL;
using ProjectManagement.Models;
using ProjectManagement.Models.ViewModels;

namespace ProjectManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectContext projectDbContext;

        public ProjectController(ProjectContext ProjectDbContext)
        {
            projectDbContext = ProjectDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await projectDbContext.Projects.ToListAsync();
            return View(projects);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddProjectViewModel addProjectRequest)
        {
            var project = new Project()
            {
                ProjectID = Guid.NewGuid(),
                ProjectName = addProjectRequest.ProjectName,
                Status = addProjectRequest.Status,
            };

            await projectDbContext.Projects.AddAsync(project);
            await projectDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var project = await projectDbContext.Projects.FirstOrDefaultAsync(x => x.ProjectID == id);

            if (project != null)
            {
                var viewModel = new UpdateProjectViewModel()
                {
                    ProjectID = project.ProjectID,
                    ProjectName = project.ProjectName,
                    Status = project.Status,
                };
                return await Task.Run(() => View("View", viewModel));
            }
             return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateProjectViewModel model)
        {
            var project = await projectDbContext.Projects.FindAsync(model.ProjectID);

            if(project != null)
            {
                project.ProjectName = model.ProjectName;
                project.Status = model.Status;

                await projectDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProjectViewModel model)
        {
            var project = await projectDbContext.Projects.FindAsync(model.ProjectID);

            if (project != null)
            {
                projectDbContext.Projects.Remove(project);
                await projectDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

    }
}
