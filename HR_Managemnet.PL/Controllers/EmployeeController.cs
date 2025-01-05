using AutoMapper;
using HR_Management.DAL.Context;
using HR_Management.DAL.Entities;
using HR_Managemnet.PL.Helpers;
using HR_Managment.BLL.Interfaces;
using HR_Managment.BLL.Specification.EmployeeSpecs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace HR_Managemnet.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly HRSystemDbContext dbContext;

        public EmployeeController(IMapper mapper,IUnitOfWork unitOfWork,HRSystemDbContext dbContext)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index(EmployeeSpecParams Params)
        {
            var departments = await unitOfWork.GenerateGenericRepo<Department>().GetAllAsync();
            var spec = new EmployeeSpecification(Params);
            var Employees = await unitOfWork.GenerateGenericRepo<Employee>().GetAllWithSpecAsync(spec);

            var specForAll = new EmployeeFilterCount(Params);
            var AllEmps = await unitOfWork.GenerateGenericRepo<Employee>().GetAllWithSpecAsync(specForAll);

            ViewBag.Departments = new SelectList(departments, "Id", "Name");
            var pageRes = new empPageResult
            {
                Data = Employees,
                PageNumber = Params.pageNumber,
                PageSize = Params.PageSize,
                TotalItems = AllEmps.Count(),
                search = Params.Search,
                departmentId = Params.DepartmentId,
            };

            return View(pageRes);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var deps = await unitOfWork.GenerateGenericRepo<Department>().GetAllAsync();
            ViewBag.Departments = new SelectList(deps, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Employee model)
        {
            await unitOfWork.GenerateGenericRepo<Employee>().AddAsync(model);
            await unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest();
            var spec = new EmployeeSpecification(id.Value);
            var emp = await unitOfWork.GenerateGenericRepo<Employee>().GetByIdWithSpecAsync(spec);
            if (emp is null) return NotFound();
            return View(viewName, emp);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var deps = await unitOfWork.GenerateGenericRepo<Department>().GetAllAsync();
            ViewBag.Departments = new SelectList(deps, "Id", "Name");
            return await Details(id, "Edit");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Employee emp)
        {
            unitOfWork.GenerateGenericRepo<Employee>().Update(emp);
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
            var emp = await unitOfWork.GenerateGenericRepo<Employee>().GetByIdAsync(Id);
            unitOfWork.GenerateGenericRepo<Employee>().Delete(emp);
            await unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UploadEmployees(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a valid Excel file.");
                return RedirectToAction("UploadEmployees");
            }

            // Parse and save employees (same logic as before)
            var employees = new List<Employee>();

            using (var stream = new MemoryStream())
            {
                await excelFile.CopyToAsync(stream);

                // Set the license context
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // First worksheet
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var firstName = worksheet.Cells[row, 1].Text;
                        var lastName = worksheet.Cells[row, 2].Text;
                        var email = worksheet.Cells[row, 3].Text;
                        var phoneNumber = worksheet.Cells[row, 4].Text;
                        // Safely parse DateTime
                        DateTime hireDate;
                        if (!DateTime.TryParse(worksheet.Cells[row, 5].Text, out hireDate))
                        {
                            ModelState.AddModelError("", $"Invalid date format in row {row}, column 5.");
                            continue; // Skip this row
                        }

                        // Safely parse decimal
                        decimal salary;
                        if (!decimal.TryParse(worksheet.Cells[row, 6].Text, out salary))
                        {
                            ModelState.AddModelError("", $"Invalid salary format in row {row}, column 6.");
                            continue; // Skip this row
                        }

                        // Safely parse department ID
                        int departmentId;
                        if (!int.TryParse(worksheet.Cells[row, 8].Text, out departmentId))
                        {
                            ModelState.AddModelError("", $"Invalid department ID in row {row}, column 8.");
                            continue; // Skip this row
                        }
                        var jobTitle = worksheet.Cells[row, 7].Text;

                        employees.Add(new Employee
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Email = email,
                            PhoneNumber = phoneNumber,
                            HireDate = hireDate,
                            Salary = salary,
                            JobTitle = jobTitle,
                            DepartmentId = departmentId
                        });
                    }
                }
            }

            dbContext.Employees.AddRange(employees);
            await dbContext.SaveChangesAsync();

            TempData["Message"] = "Employees successfully uploaded!";
            return RedirectToAction("Index");
        }

    }
}
