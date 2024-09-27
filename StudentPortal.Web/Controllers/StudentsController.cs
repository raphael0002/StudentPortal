using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public StudentsController(ApplicationDbContext dbContext)
        {
			this.dbContext = dbContext;
		}


		[HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            var student = new Student { Name = viewModel.Email , Email = viewModel.Email, Phone = viewModel.Phone, Subscribed = viewModel.Subscribed };
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync(); 
            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
          var students = await dbContext.Students.ToListAsync();

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student model)
        {
           var student =  await dbContext.Students.FindAsync(model.Id);

            if (student != null)
            {
                student.Name = model.Name;
				student.Email = model.Email;
				student.Phone = model.Phone;
				student.Subscribed = model.Subscribed;
                
                await dbContext.SaveChangesAsync();            
            } 
            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student model) 
        { 
            var student = await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);

            if (student is not null)
            {
                dbContext.Students.Remove(student);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Students");
        }

    }

}
