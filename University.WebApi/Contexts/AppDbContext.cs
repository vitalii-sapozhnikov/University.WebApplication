using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace University.WebApi.Contexts
{
    public class AppDbContext: IdentityDbContext<ApplicationUser>
    {
        
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<ScientificPublication> ScientificPublications { get; set; }
        public DbSet<MethodologicalPublication> MethodologicalPublications { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<HeadOfDepartment> HeadsOfDepartments { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<LecturerDiscipline> LecturerDisciplines { get; set; }
        public DbSet<Speciality> Specialities { get; set; }
        public DbSet<WorkPlan> WorkPlans { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MethodologicalPublication>()
            .HasOne(p => p.Plan)
            .WithMany(p => p.MethodologicalPublications)
            .HasForeignKey(p => p.PlanId)
            .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<LecturerDiscipline>().HasKey(ld => new { ld.LecturerId, ld.DisciplineId });

            builder.Entity<LecturerDiscipline>()
                .HasOne(ld => ld.Lecturer)
                .WithMany(l => l.LecturerDisciplines)
                .HasForeignKey(ld => ld.LecturerId);

            builder.Entity<LecturerDiscipline>()
                .HasOne(ld => ld.Discipline)
                .WithMany(d => d.LecturerDisciplines)
                .HasForeignKey(ld => ld.DisciplineId);

            SeedData(builder);


            builder.Entity<Publication>().ToTable("Publications");
           

            builder.Entity<Person>().ToTable("Person");
            builder.Entity<MethodologicalPublication>().ToTable("MethodologicalPublications");
            builder.Entity<ScientificPublication>().ToTable("ScientificPublications");

            builder.Entity<Lecturer>().ToTable("Lecturers");
            builder.Entity<HeadOfDepartment>().ToTable("HeadsOfDepartments");

            builder.Entity<WorkPlan>()
                .HasMany(wp => wp.Disciplines)
                .WithMany(); // Assuming you don't need additional navigation properties or configuration here

            base.OnModelCreating(builder);
        }

        private static void SeedData(ModelBuilder builder)
        {
            builder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Name = "Кафедра диференціальних рівнянь, геометрії та топології" },
                new Department { DepartmentId = 2, Name = "Кафедра комп'ютерних систем та технологій" },
                new Department { DepartmentId = 3, Name = "Кафедра комп’ютерної алгебри та дискретної математики" },
                new Department { DepartmentId = 4, Name = "Кафедра математичного аналізу" },
                new Department { DepartmentId = 5, Name = "Кафедра математичного забезпечення комп’ютерних систем" },
                new Department { DepartmentId = 6, Name = "Кафедра методів математичної фізики" },
                new Department { DepartmentId = 7, Name = "Кафедра механіки, автоматизації та інформаційних технологій" },
                new Department { DepartmentId = 8, Name = "Кафедра оптимального керування та економічної кібернетики" },
                new Department { DepartmentId = 9, Name = "Кафедра фізики та астрономії" }
            );

            builder.Entity<Discipline>().HasData(
                new Discipline { Id = 1, Name = "Організація бази даних" },
                new Discipline { Id = 2, Name = "Інженерія програмного забезпечення" },
                new Discipline { Id = 3, Name = "Введення в систему підтримки прийняття рішень" },
                new Discipline { Id = 4, Name = "Технологія тестування програмного забезпечення" }
                );
        }
        public DbSet<Models.Models.EducationYear> EducationYear { get; set; } = default!;
    }
}
