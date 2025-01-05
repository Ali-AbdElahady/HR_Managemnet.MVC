

using HR_Management.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR_Management.DAL.Configurations
{
    internal class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.HasOne(A => A.Employee)
                .WithMany(E => E.Attendances)
                .HasForeignKey(A=>A.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
