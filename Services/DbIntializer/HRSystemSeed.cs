using HR_Management.DAL.Context;
using HR_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.DbIntializer
{
    public static class HRSystemSeed
    {
        public static async Task SeedAsync(HRSystemDbContext dbContext) 
        {
            if (!dbContext.Departments.Any())
            {
                var DepartmentsData = File.ReadAllText("../Services/DataSeed/Departments.json");
                var Departments = JsonSerializer.Deserialize<List<Department>>(DepartmentsData);
                if (Departments?.Count > 0)
                {
                    foreach (Department department in Departments)
                    {
                        await dbContext.Set<Department>().AddAsync(department);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.Employees.Any())
            {
                var EmployeesData = File.ReadAllText("../Services/DataSeed/Employees.json");
                var Employees = JsonSerializer.Deserialize<List<Employee>>(EmployeesData);
                if (Employees?.Count > 0)
                {
                    foreach (Employee employee in Employees)
                    {
                        await dbContext.Set<Employee>().AddAsync(employee);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (dbContext.Departments.Any() && dbContext.Set<Department>().FirstOrDefault()?.ManagerId == null)
            {
                var employees = dbContext.Set<Employee>().ToList();
                var departments = dbContext.Set<Department>();
                foreach (var department in departments)
                {
                    var manager = employees.FirstOrDefault(e => e.DepartmentId == department.Id);
                    if (manager != null)
                    {
                        department.ManagerId = manager.Id;
                    }
                }
                dbContext.Departments.UpdateRange(departments);
                dbContext.SaveChanges();
            }
            if (!dbContext.Attendances.Any())
            {
                var AttendancesData = File.ReadAllText("../Services/DataSeed/Attendance.json");
                var Attendances = JsonSerializer.Deserialize<List<Attendance>>(AttendancesData);
                if (Attendances?.Count > 0)
                {
                    foreach (Attendance attendance in Attendances)
                    {
                        await dbContext.Set<Attendance>().AddAsync(attendance);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
