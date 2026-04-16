using EmployeeCRM.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<EmployeeClient> EmployeeClients { get; set; }
        public DbSet<EmployeeTask> EmployeeTasks { get; set; }
        public DbSet<PerformanceReview> PerformanceReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AppUser
            modelBuilder.Entity<AppUser>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Username).IsUnique();
                e.HasOne(x => x.Employee)
                 .WithOne(x => x.AppUser)
                 .HasForeignKey<AppUser>(x => x.EmployeeId)
                 .OnDelete(DeleteBehavior.SetNull);
            });

            // Employee
            modelBuilder.Entity<Employee>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
                e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
                e.Property(x => x.Email).HasMaxLength(200).IsRequired();
                e.Property(x => x.Department).HasMaxLength(100);
                e.Property(x => x.Designation).HasMaxLength(100);
            });

            // Client
            modelBuilder.Entity<Client>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
                e.Property(x => x.ContactPerson).HasMaxLength(100).IsRequired();
            });

            // EmployeeClient (many-to-many link)
            modelBuilder.Entity<EmployeeClient>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Employee)
                 .WithMany(x => x.EmployeeClients)
                 .HasForeignKey(x => x.EmployeeId)
                 .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Client)
                 .WithMany(x => x.EmployeeClients)
                 .HasForeignKey(x => x.ClientId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            // EmployeeTask
            modelBuilder.Entity<EmployeeTask>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).HasMaxLength(200).IsRequired();
                e.Property(x => x.Status).HasMaxLength(50);
                e.Property(x => x.Priority).HasMaxLength(50);

                e.HasOne(x => x.AssignedTo)
                 .WithMany(x => x.AssignedTasks)
                 .HasForeignKey(x => x.AssignedToEmployeeId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.AssignedBy)
                 .WithMany(x => x.CreatedTasks)
                 .HasForeignKey(x => x.AssignedByEmployeeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // PerformanceReview
            modelBuilder.Entity<PerformanceReview>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.ReviewPeriod).HasMaxLength(100);

                e.HasOne(x => x.Employee)
                 .WithMany(x => x.PerformanceReviews)
                 .HasForeignKey(x => x.EmployeeId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Reviewer)
                 .WithMany(x => x.ReviewsGiven)
                 .HasForeignKey(x => x.ReviewerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
