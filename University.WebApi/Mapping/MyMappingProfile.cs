using AutoMapper;
using Models.Models;
using University.WebApi.Dtos.EducationalYearDtos;
using University.WebApi.Dtos.PersonDtos;
using University.WebApi.Dtos.ScientificPublicationDto;

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
                        : null));
        }
    }
}
