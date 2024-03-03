using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using Models.Roles;
using System.Reflection;
using System.Text;
using University.WebApi.Contexts;
using University.WebApi.Services;

namespace University.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

            builder.Services.AddTransient<IAuthService, AuthService>();

            builder.Services.AddTransient<IApplicationUserService, ApplicationUserService>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    RequireExpirationTime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                    ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
                };
            });


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            CreateRoles(app.Services).GetAwaiter().GetResult();

            app.MapControllers();

            app.Run();
        }

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Create a scope to resolve the scoped service RoleManager<IdentityRole>
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedProvider = scope.ServiceProvider;
                var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Add roles here
                var roles = new List<string> { Roles.Admin, Roles.User, Roles.Lecturer, Roles.HeadOfDepartment, Roles.EducationDepartment, Roles.GuarantorOfSpeciality };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }

    }
}
