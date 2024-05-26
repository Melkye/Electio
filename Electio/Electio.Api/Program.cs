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
//builder.Services.AddDbContext<ElectioDbContext>((sp, options) =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

builder.Services.AddDbContext<ElectioDbContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    contextLifetime: ServiceLifetime.Transient);
////contextLifetime: ServiceLifetime.Singleton);
////builder.Services.AddDbContext<ElectioDbContext>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
builder.Services.AddSingleton<IMapper>(provider => new Mapper(mapperConfig));

builder.Services.AddTransient<UnitOfWork>();

builder.Services.AddTransient<StudentRepository>();
builder.Services.AddTransient<CourseRepository>();
builder.Services.AddTransient<StudentOnCourseRepository>();

builder.Services.AddTransient<StudentService>();
builder.Services.AddTransient<CoursesService>();

//builder.Services.AddTransient<ElectioDbContext, ElectioDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options => options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();
