﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using University.WebApi.Contexts;

#nullable disable

namespace University.WebApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240215120352_PersonAdded")]
    partial class PersonAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DisciplinePublication", b =>
                {
                    b.Property<int>("DisciplinesId")
                        .HasColumnType("integer");

                    b.Property<int>("PublicationsPublicationId")
                        .HasColumnType("integer");

                    b.HasKey("DisciplinesId", "PublicationsPublicationId");

                    b.HasIndex("PublicationsPublicationId");

                    b.ToTable("DisciplinePublication");
                });

            modelBuilder.Entity("LecturerPublication", b =>
                {
                    b.Property<int>("LecturersId")
                        .HasColumnType("integer");

                    b.Property<int>("PublicationsPublicationId")
                        .HasColumnType("integer");

                    b.HasKey("LecturersId", "PublicationsPublicationId");

                    b.HasIndex("PublicationsPublicationId");

                    b.ToTable("LecturerPublication");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Models.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Models.Models.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DepartmentId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DepartmentId");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            DepartmentId = 1,
                            Name = "Кафедра диференціальних рівнянь, геометрії та топології"
                        },
                        new
                        {
                            DepartmentId = 2,
                            Name = "Кафедра комп'ютерних систем та технологій"
                        },
                        new
                        {
                            DepartmentId = 3,
                            Name = "Кафедра комп’ютерної алгебри та дискретної математики"
                        },
                        new
                        {
                            DepartmentId = 4,
                            Name = "Кафедра математичного аналізу"
                        },
                        new
                        {
                            DepartmentId = 5,
                            Name = "Кафедра математичного забезпечення комп’ютерних систем"
                        },
                        new
                        {
                            DepartmentId = 6,
                            Name = "Кафедра методів математичної фізики"
                        },
                        new
                        {
                            DepartmentId = 7,
                            Name = "Кафедра механіки, автоматизації та інформаційних технологій"
                        },
                        new
                        {
                            DepartmentId = 8,
                            Name = "Кафедра оптимального керування та економічної кібернетики"
                        },
                        new
                        {
                            DepartmentId = 9,
                            Name = "Кафедра фізики та астрономії"
                        });
                });

            modelBuilder.Entity("Models.Models.Discipline", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Disciplines");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Організація бази даних"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Інженерія програмного забезпечення"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Введення в систему підтримки прийняття рішень"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Технологія тестування програмного забезпечення"
                        });
                });

            modelBuilder.Entity("Models.Models.LecturerDiscipline", b =>
                {
                    b.Property<int>("LecturerId")
                        .HasColumnType("integer");

                    b.Property<int>("DisciplineId")
                        .HasColumnType("integer");

                    b.Property<DateOnly>("BeginDate")
                        .HasColumnType("date");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.HasKey("LecturerId", "DisciplineId");

                    b.HasIndex("DisciplineId");

                    b.ToTable("LecturerDisciplines");
                });

            modelBuilder.Entity("Models.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MiddleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Person", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Models.Models.Plan", b =>
                {
                    b.Property<int>("PlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PlanId"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("PlanId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Plans");
                });

            modelBuilder.Entity("University.WebApi.Models.Publication", b =>
                {
                    b.Property<int>("PublicationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PublicationId"));

                    b.Property<string>("Abstract")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string[]>("Keywords")
                        .HasColumnType("text[]");

                    b.Property<int?>("Language")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("PublicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Volume")
                        .HasColumnType("integer");

                    b.Property<bool>("isPublished")
                        .HasColumnType("boolean");

                    b.HasKey("PublicationId");

                    b.ToTable("Publications", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Models.Models.HeadOfDepartment", b =>
                {
                    b.HasBaseType("Models.Models.Person");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("DepartmentId")
                        .IsUnique();

                    b.ToTable("HeadsOfDepartments", (string)null);
                });

            modelBuilder.Entity("University.WebApi.Models.Lecturer", b =>
                {
                    b.HasBaseType("Models.Models.Person");

                    b.Property<int>("AcademicTitle")
                        .HasColumnType("integer");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("text");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("integer");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Lecturers", (string)null);
                });

            modelBuilder.Entity("Models.Models.MethodologicalPublication", b =>
                {
                    b.HasBaseType("University.WebApi.Models.Publication");

                    b.Property<string>("CloudStorageGuid")
                        .HasColumnType("text");

                    b.Property<int?>("PlanId")
                        .HasColumnType("integer");

                    b.Property<int?>("Type")
                        .HasColumnType("integer");

                    b.HasIndex("PlanId");

                    b.ToTable("MethodologicalPublications", (string)null);
                });

            modelBuilder.Entity("Models.Models.ScientificPublication", b =>
                {
                    b.HasBaseType("University.WebApi.Models.Publication");

                    b.Property<string>("DOI")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("JournalDetails")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("JournalType")
                        .HasColumnType("integer");

                    b.ToTable("ScientificPublications", (string)null);
                });

            modelBuilder.Entity("DisciplinePublication", b =>
                {
                    b.HasOne("Models.Models.Discipline", null)
                        .WithMany()
                        .HasForeignKey("DisciplinesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("University.WebApi.Models.Publication", null)
                        .WithMany()
                        .HasForeignKey("PublicationsPublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LecturerPublication", b =>
                {
                    b.HasOne("University.WebApi.Models.Lecturer", null)
                        .WithMany()
                        .HasForeignKey("LecturersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("University.WebApi.Models.Publication", null)
                        .WithMany()
                        .HasForeignKey("PublicationsPublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Models.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Models.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Models.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Models.LecturerDiscipline", b =>
                {
                    b.HasOne("Models.Models.Discipline", "Discipline")
                        .WithMany("LecturerDisciplines")
                        .HasForeignKey("DisciplineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("University.WebApi.Models.Lecturer", "Lecturer")
                        .WithMany("LecturerDisciplines")
                        .HasForeignKey("LecturerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Discipline");

                    b.Navigation("Lecturer");
                });

            modelBuilder.Entity("Models.Models.Plan", b =>
                {
                    b.HasOne("Models.Models.Department", "Department")
                        .WithMany("Plans")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Models.Models.HeadOfDepartment", b =>
                {
                    b.HasOne("Models.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Models.Models.Department", "Department")
                        .WithOne("HeadOfDepartment")
                        .HasForeignKey("Models.Models.HeadOfDepartment", "DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Models.Person", null)
                        .WithOne()
                        .HasForeignKey("Models.Models.HeadOfDepartment", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("University.WebApi.Models.Lecturer", b =>
                {
                    b.HasOne("Models.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Models.Models.Department", "Department")
                        .WithMany("Lecturers")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Models.Person", null)
                        .WithOne()
                        .HasForeignKey("University.WebApi.Models.Lecturer", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Department");
                });

            modelBuilder.Entity("Models.Models.MethodologicalPublication", b =>
                {
                    b.HasOne("Models.Models.Plan", "Plan")
                        .WithMany("MethodologicalPublications")
                        .HasForeignKey("PlanId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("University.WebApi.Models.Publication", null)
                        .WithOne()
                        .HasForeignKey("Models.Models.MethodologicalPublication", "PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Plan");
                });

            modelBuilder.Entity("Models.Models.ScientificPublication", b =>
                {
                    b.HasOne("University.WebApi.Models.Publication", null)
                        .WithOne()
                        .HasForeignKey("Models.Models.ScientificPublication", "PublicationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Models.Department", b =>
                {
                    b.Navigation("HeadOfDepartment")
                        .IsRequired();

                    b.Navigation("Lecturers");

                    b.Navigation("Plans");
                });

            modelBuilder.Entity("Models.Models.Discipline", b =>
                {
                    b.Navigation("LecturerDisciplines");
                });

            modelBuilder.Entity("Models.Models.Plan", b =>
                {
                    b.Navigation("MethodologicalPublications");
                });

            modelBuilder.Entity("University.WebApi.Models.Lecturer", b =>
                {
                    b.Navigation("LecturerDisciplines");
                });
#pragma warning restore 612, 618
        }
    }
}
