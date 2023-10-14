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
            //CreateMap<EnrollmentDto, Course>(); // IMPORTANT
            //CreateMap<EnrollmentDto, Student>();

            CreateMap<OnDemandCourse, OnDemandCourseDto>();
            CreateMap<OnDemandCourse, OnDemandCourseDto>();
            CreateMap<OnDemandCourseForCreationDto, OnDemandCourse>();
            CreateMap<OnDemandCourseForUpdateDto, OnDemandCourse>();

            CreateMap<Section,SectionDto>();
            CreateMap<SectionForCreateDto,Section>();


            CreateMap<ProductForCreateDto, Product>(); 
            CreateMap<ProductForUpdateDto, Product>();
            CreateMap<Product,ProductDto>();

            CreateMap<PublicationForCreateDto, Publication>();
            CreateMap<PublicationForUpdateDto, Publication>();

            CreateMap<Package, PackageDto>();
            CreateMap<PackageForCreateDto, Package>();
            CreateMap<PackageForUpdateDto, Package>();

            CreateMap<Enrollment, EnrollmentDto>();
            CreateMap<EnrollmentForCreateDto, Enrollment>();

        }
    }
}
