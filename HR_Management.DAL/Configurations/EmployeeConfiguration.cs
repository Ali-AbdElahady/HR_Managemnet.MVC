using HR_Management.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR_Management.DAL.Configurations
{
    internal class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasOne(e => e.Department) 
            .WithMany(d => d.Employees) 
            .HasForeignKey(e => e.DepartmentId) 
            .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}
