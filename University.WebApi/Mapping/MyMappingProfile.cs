using AutoMapper;
using Models.Models;
using University.WebApi.Dtos.EducationalYearDtos;
using University.WebApi.Dtos.PersonDtos;
using University.WebApi.Dtos.ScientificPublicationDto;
using University.WebApi.Dtos.WorkPlanDtos;

namespace University.WebApi.Mapping
{
    public class MyMappingProfile: Profile
    {
        public MyMappingProfile() 
        {
            // Education year
            CreateMap<AddYearDto, EducationYear>();


            // Person
            CreateMap<Person, GetPersonDto>();
            CreateMap<GetPersonDto, Person>();
            CreateMap<PostPersonDto, Person>();

            // Scientific publications
            CreateMap<PostScientificPublicationDto, ScientificPublication>()
                .ForMember(dest => dest.Keywords, opt => opt.MapFrom(src =>
                    src.Keywords != null
                        ? src.Keywords.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(k => k.Trim()).ToArray()
                        : null))
                .ForMember(dest => dest.PublicationDate, opt => opt.MapFrom(src =>
                    DateTime.SpecifyKind(src.PublicationDate, DateTimeKind.Utc)));


            // Work plans
            CreateMap<PostWorkPlan, WorkPlan>();
            CreateMap<WorkPlan, GetWorkPlan>();
        }
    }
}
