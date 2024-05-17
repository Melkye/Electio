using AutoMapper;
using Electio.BusinessLogic.Profiles;
using Electio.BusinessLogic.Services;
using Electio.DataAccess;
using Electio.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register ElectioDbContext to DI container
builder.Services.AddDbContext<ElectioDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.Services.AddDbContext<ElectioDbContext>(
//    optionsBuilder => optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
//    contextLifetime: ServiceLifetime.Transient);
////contextLifetime: ServiceLifetime.Singleton);
////builder.Services.AddDbContext<ElectioDbContext>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
builder.Services.AddSingleton<IMapper>(provider => new Mapper(mapperConfig));

builder.Services.AddTransient<StudentService, StudentService>();
builder.Services.AddTransient<StudentRepository, StudentRepository>();

builder.Services.AddTransient<CourseRepository, CourseRepository>();

builder.Services.AddTransient<StudentOnCourseRepository, StudentOnCourseRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
