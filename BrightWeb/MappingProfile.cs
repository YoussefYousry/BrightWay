using AutoMapper;
using BrightWeb_BAL.DTO;
using BrightWeb_DAL.Models;

namespace BrightWeb
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForLoginDto, User>();
            CreateMap<StudentForRegisterDto, Student>();
            CreateMap<Student,StudentDto>();
            CreateMap<StudentForUpdateDto, Student>();


        }
    }
}
