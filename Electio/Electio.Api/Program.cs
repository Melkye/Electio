using System.Text;
using AutoMapper;
using Electio.BusinessLogic.Profiles;
using Electio.BusinessLogic.Services;
using Electio.DataAccess;
using Electio.DataAccess.Identity;
using Electio.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<ElectioDbContext>(
    optionsBuilder => optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    contextLifetime: ServiceLifetime.Transient);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ElectioDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

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

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireStudentRole", policy => policy.RequireRole("Student"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ElectioDbContext>();
    context.Database.Migrate();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData(userManager, roleManager);
}

app.Run();

async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
{
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    if (!await roleManager.RoleExistsAsync("Student"))
    {
        await roleManager.CreateAsync(new IdentityRole("Student"));
    }

    var adminUser = await userManager.FindByNameAsync("admin");
    if (adminUser == null)
    {
        adminUser = new ApplicationUser { Name = "Admin User", UserName = "admin", Email = "admin@example.com" };
        await userManager.CreateAsync(adminUser, "AdminPassword123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}
