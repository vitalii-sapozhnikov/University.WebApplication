using Models.Models;
using University.WebApi.Dtos;
using University.WebApi.Dtos.PersonDtos;

namespace University.WebApi.Mapping
{
    public static class MappingExtensions
    {
        public static Lecturer ToLecturerEntity(this PostPersonDto authorDto)
        {
            return new Lecturer
            {
                FirstName = authorDto.FirstName,
                LastName = authorDto.LastName,
                MiddleName = authorDto.MiddleName
            };
        }

        public static LecturerDto ToLecturerDto(this Lecturer lecturer)
        {
            return new LecturerDto
            {
                Id = lecturer.Id,
                FirstName = lecturer.FirstName,
                LastName = lecturer.LastName,
                MiddleName = lecturer.MiddleName,
                Departments = lecturer.Departments,
                AcademicTitle = lecturer.AcademicTitle
            };
        }

    }
}
