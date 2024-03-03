using Microsoft.AspNetCore.Identity;
using Models.Models;
using System.Security.Claims;
using University.WebApi.Contexts;

namespace University.WebApi.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly AppDbContext appDbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUserService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

        public async Task<Lecturer?> GetLecturer(string userEmail)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userEmail);
            Lecturer? lect = await appDbContext.Lecturers.FindAsync(user);
            return lect;
        }

        public async Task<HeadOfDepartment?> GetHeadOfDepartment(string userEmail)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(userEmail);
            HeadOfDepartment? head = await appDbContext.HeadsOfDepartments.FindAsync(user);
            return head;
        }
    }
}
