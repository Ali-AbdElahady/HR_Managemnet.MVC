using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.DAL.Entities
{
    public class Attendance
    {
        public int Id { get; set; } 
        public int EmployeeId { get; set; } 
        public Employee Employee { get; set; } 
        public DateTime Date { get; set; } 
        public TimeSpan TimeIn { get; set; } 
        public TimeSpan? TimeOut { get; set; } 
        public string? Notes { get; set; } 
    }
}
