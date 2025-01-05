using HR_Management.BLL.Specification;
using HR_Management.DAL.Entities;
using HR_Managemnet.PL.Helpers;
using HR_Managment.BLL.Interfaces;
using HR_Managment.BLL.Specification.DepartmentSpecs;
using HR_Managment.BLL.Specification.EmployeeSpecs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HR_Managemnet.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(EntitySpecParams Params)
        {
            var spec = new DepartmentSpecification(Params);
            var Departments = await unitOfWork.GenerateGenericRepo<Department>().GetAllWithSpecAsync(spec);

            var specForAll = new DepartmentsFilterCount(Params);
            var AllDeps = await unitOfWork.GenerateGenericRepo<Department>().GetAllWithSpecAsync(specForAll);

            var pageRes = new DepPageResult
            {
                Data = Departments,
                PageNumber = Params.pageNumber,
                PageSize = Params.PageSize,
                TotalItems = AllDeps.Count(),
                search = Params.Search,
            };

            return View(pageRes);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var emps = await unitOfWork.GenerateGenericRepo<Employee>().GetAllAsync();
            ViewBag.Employees = new SelectList(emps.Select(e => new
            {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.LastName}"
            }),"Id", "FullName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            await unitOfWork.GenerateGenericRepo<Department>().AddAsync(department);
            await unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Details(int? id,string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var spec = new DepartmentSpecification(id.Value);
            var dep = await unitOfWork.GenerateGenericRepo<Department>().GetByIdWithSpecAsync(spec);
            if (dep is null) return NotFound();
            return View(viewName, dep);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var emps = await unitOfWork.GenerateGenericRepo<Employee>().GetAllAsync();
            ViewBag.Employees = new SelectList(emps.Select(e => new
            {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.LastName}"
            }), "Id", "FullName");
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Department dep)
        {
            unitOfWork.GenerateGenericRepo<Department>().Update(dep);
            await unitOfWork.CompleteAsync(); // Ensure the changes are saved

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, int Id)
        {
            if (Id != id) return BadRequest();
            var dep = await unitOfWork.GenerateGenericRepo<Department>().GetByIdAsync(Id);
            unitOfWork.GenerateGenericRepo<Department>().Delete(dep);
            await unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
