using Models.Models;

namespace University.WebApi.Services
{
    public interface IApplicationUserService
    {
        Task<HeadOfDepartment?> GetHeadOfDepartment(string userEmail);
        Task<Lecturer?> GetLecturer(string userEmail);
    }
}