using BrightWeb.Extensions;
using BrightWeb_DAL.Models;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
               policy =>
               {
                   policy.SetIsOriginAllowed(origin => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
               });
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("StudentOnly", policy =>
    {
        policy.RequireRole("Student");
    });
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireRole("Admin");
    });
});

builder.Services.ConfigureLifeTime();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureIdentity<User>();
builder.Services.ConfigureIdentity<Student>();
builder.Services.ConfigureJwt(builder.Configuration);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddHttpClient("YouTubeAPI", client =>
//{
//    client.BaseAddress = new Uri("https://www.googleapis.com/youtube/v3/");
//});

builder.Services.AddControllers().AddJsonOptions(
  opt =>
      opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Place to add JWT with Bearer",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement()
{

    {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            },
            Name = "Bearer",
        },
        new List<string>()
    }
});
});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
