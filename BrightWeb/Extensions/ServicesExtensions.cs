using BrightWeb_DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using BrightWeb_BAL.Repositories;
using BrightWeb_BAL.Contracts;
using BrightWeb_DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Identity.Web;
using BrightWeb_BAL.Services;

namespace BrightWeb.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration)
         => services.AddDbContext<AppDbContext>(opts =>
                                    opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")
                                    , b => b.MigrationsAssembly("BrightWeb")));

        public static void ConfigureIdentity<T>(this IServiceCollection services) where T : User
        {
            var authBuilder = services.AddIdentityCore<T>
                (o =>
                {
                    o.Password.RequireDigit = true;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 10;
                    o.User.RequireUniqueEmail = true;
                });
            authBuilder = new IdentityBuilder(authBuilder.UserType, typeof(IdentityRole), services);
            authBuilder.AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }
        public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes("BrightWaySecretAPIKeyValueToStoreInServer");
            services.AddAuthentication(opt =>
            {
                //opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(
                options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
                        ValidAudience = jwtSettings.GetSection("validAudience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                    };
                })
                .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAd"), "jwtBearerScheme2");

        }

        public static void ConfigureLifeTime(this IServiceCollection services)
        {
            services.AddScoped<User, Student>();
            //services.AddScoped<Course,OnlineCourse>();
            //services.AddScoped<Course,OnDemandCourse>();
            services.AddScoped<IRepositoryBase<Student>,StudentRepository>();
            services.AddScoped<IRepositoryBase<OnlineCourse>,OnlineCourseRepository>();
            services.AddScoped<IRepositoryBase<OnDemandCourse>,OnDemandCoursesRepository>();


            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStudentRepository,StudentRepository>();
            services.AddScoped<IOnlineCourseRepository,OnlineCourseRepository>();
            services.AddScoped<IOnDemandCoursesRepository,OnDemandCoursesRepository>();
            services.AddScoped<IFilesManager, FilesManager>();
           


            services.AddScoped<IProductsRepository, ProductsRepository>();
            services.AddScoped<RepositoryBase<Product>, ProductsRepository>();


            services.AddScoped<IPublicationRepository, PublicationRepository>();
            services.AddScoped<RepositoryBase<Publication>, PublicationRepository>();

            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<RepositoryBase<Package>, PackageRepository>();
            
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<RepositoryBase<Section>, SectionRepository>();


            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<RepositoryBase<Project>, ProjectsRepository>();

            services.AddSingleton<YouTubeService>();

            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IStaticRepository, StaticRepository>();

        }
    }
}
