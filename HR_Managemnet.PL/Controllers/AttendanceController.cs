using HR_Management.DAL.Context;
using HR_Management.DAL.Entities;
using HR_Managemnet.PL.Helpers;
using HR_Managment.BLL.Interfaces;
using HR_Managment.BLL.Specification.AttendanceSpecs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace HR_Managemnet.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AttendanceController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly HRSystemDbContext dbContext;

        public AttendanceController(IUnitOfWork unitOfWork, HRSystemDbContext dbContext)
        {
            this.unitOfWork = unitOfWork;
            this.dbContext = dbContext;
        }
        public async Task<IActionResult> Index(AttendanceSpecParams Params)
        {
            Params.PageSize = 30;
            var spec = new AttendanceSpecification(Params);
            var Attendances = await unitOfWork.GenerateGenericRepo<Attendance>().GetAllWithSpecAsync(spec);

            var specForAll = new AttendanceFilterCount(Params);
            var AllAttends = await unitOfWork.GenerateGenericRepo<Attendance>().GetAllWithSpecAsync(specForAll);

            var pageRes = new AttendancePageResult
            {
                Data = Attendances,
                PageNumber = Params.pageNumber,
                PageSize = Params.PageSize,
                TotalItems = AllAttends.Count(),
                EmployeeId = Params.EmployeeId,
                EmpName = Params.EmpName,
                DateStart = Params.DateStart,
                DateEnd = Params.DateEnd
            };

            return View(pageRes);
        }
        [Route("SearchEmployees")]
        [HttpGet]
        public async Task<IActionResult> SearchEmployees(string searchTerm)
        {
            var employees = await unitOfWork.GenerateGenericRepo<Employee>().GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                employees = employees.Where(p => $"{p.FirstName} {p.LastName}".ToLower().Contains(searchTerm.ToLower())).ToList();
            }

            // Return the filtered list of patients as a JSON response
            return Json(employees.Select(p => new { id = p.Id, name = $"{p.FirstName} {p.LastName}" }));
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var emps = await unitOfWork.GenerateGenericRepo<Employee>().GetAllAsync();
            ViewBag.Employees = new SelectList(emps.Select(e => new
            {
                Id = e.Id,
                FullName = $"{e.FirstName} {e.LastName}"
            }), "Id", "FullName");
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(Attendance model)
        {
            await unitOfWork.GenerateGenericRepo<Attendance>().AddAsync(model);
            await unitOfWork.CompleteAsync();

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> UploadAttendances(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a valid Excel file.");
                return RedirectToAction("UploadAttendances");
            }
            var attendances = new List<Attendance>();

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
                        // Validate Employee ID
                        if (!int.TryParse(worksheet.Cells[row, 1].Text, out int employeeId))
                        {
                            ModelState.AddModelError("", $"Invalid Employee ID in row {row}.");
                            continue; // Skip invalid row
                        }

                        // Validate Date
                        if (!DateTime.TryParse(worksheet.Cells[row, 2].Text, out DateTime date))
                        {
                            ModelState.AddModelError("", $"Invalid Date in row {row}.");
                            continue; // Skip invalid row
                        }

                        // Validate TimeIn
                        if (!TimeSpan.TryParse(worksheet.Cells[row, 3].Text, out TimeSpan timeIn))
                        {
                            ModelState.AddModelError("", $"Invalid TimeIn in row {row}.");
                            continue; // Skip invalid row
                        }

                        // Validate TimeOut (optional)
                        TimeSpan? timeOut = null;
                        if (!string.IsNullOrEmpty(worksheet.Cells[row, 4].Text))
                        {
                            if (!TimeSpan.TryParse(worksheet.Cells[row, 4].Text, out TimeSpan parsedTimeOut))
                            {
                                ModelState.AddModelError("", $"Invalid TimeOut in row {row}.");
                                continue; // Skip invalid row
                            }
                            timeOut = parsedTimeOut;
                        }

                        // Notes (optional)
                        var notes = worksheet.Cells[row, 5].Text;

                        attendances.Add(new Attendance
                        {
                            EmployeeId = employeeId,
                            Date = date,
                            TimeIn = timeIn,
                            TimeOut = timeOut,
                            Notes = string.IsNullOrWhiteSpace(notes) ? null : notes
                        });
                    }
                }
            }

            dbContext.Attendances.AddRange(attendances);
            await dbContext.SaveChangesAsync();

            TempData["Message"] = "Attendances successfully uploaded!";
            return RedirectToAction("Index");
        }
    }
}
