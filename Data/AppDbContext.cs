using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using SmartGym.API.Models;

namespace SmartGym.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            :base(options)
        {
            
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<GymClass> GymClasse { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<MemberTrainer> MemberTrainers { get; set; }
        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GymClass>()
                .HasOne(g => g.Employee)
                .WithMany(e => e.GymClasses)
                .HasForeignKey(g => g.IdTrainer)
                .IsRequired();

            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Roles)
                .WithMany(r => r.Employees)
                .HasForeignKey(e => e.IdRole)
                .IsRequired();

            modelBuilder.Entity<Member>()
                .HasOne(m => m.Roles)
                .WithMany(r => r.Members)
                .HasForeignKey(m => m.IdRole)
                .IsRequired();

            modelBuilder.Entity<MemberTrainer>()
                .HasOne(mt => mt.Member)
                .WithMany(m => m.MemberTrainers)
                .HasForeignKey(mt => mt.IdMember)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<MemberTrainer>()
                .HasOne(mt => mt.Employee)
                .WithMany(e => e.MemberTrainers)
                .HasForeignKey(mt => mt.IdTrainer)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Properts
            modelBuilder.Entity<MemberTrainer>()
                .Property(mt => mt.IdMember)
                .HasColumnName("Member_Id");

            modelBuilder.Entity<MemberTrainer>()
                .Property(mt => mt.IdTrainer)
                .HasColumnName("Employee_Id");

            // Index Unique

            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();




            base.OnModelCreating(modelBuilder);
        }
    }
}
