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
            CreateMap<AdminForRegisterDto, User>();
            CreateMap<StudentForUpdateDto, Student>();

            CreateMap<OnlineCourse, OnlineCourseDto>();
            CreateMap<OnlineCourseForCreationDto, OnlineCourse>();
            CreateMap<OnlineCourseForUpdateDto, OnlineCourse>();
            CreateMap<EnrollmentDto, Course>(); // IMPORTANT
            CreateMap<EnrollmentDto, Student>();
            CreateMap<OnDemandCourse, OnDemandCourseDto>();
            CreateMap<OnDemandCourseForCreationDto, OnDemandCourse>();
            CreateMap<OnDemandCourseForUpdateDto, OnDemandCourse>();

            CreateMap<ProductForCreateDto, Product>();
            CreateMap<PublicationForCreateDto, Publication>();

        }
    }
}
