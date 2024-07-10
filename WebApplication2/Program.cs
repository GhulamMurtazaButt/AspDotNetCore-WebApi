using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataLibrary.Repositry.Impl;
using DataLibrary.Data;
using WebApplication2.Services.AuthService.Impl;
using WebApplication2.Services.AuthService;
using WebApplication2.Services.EmailService.Impl;
using WebApplication2.Services.EmailService;
using WebApplication2.Services.TokenService;
using WebApplication2.Services.UserService.Impl;
using WebApplication2.Services.UserService;
using WebApplication2.Utilities.Base64Methods;
using WebApplication2.Utilities.Base64Methods.Impl;
using WebApplication2.Utilities;
using WebApplication2.Services.ProductService;
using WebApplication2.Services.ProductService.Impl;
using WebApplication2.Services.CategoriesService;
using WebApplication2.Services.CategoriesService.Impl;
using WebApplication1.Services.TokenService.Impl;
using Microsoft.Extensions.FileProviders;
using WebApplication2.Services.FileService;
using WebApplication2.Services.FileService.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
   x => x.MigrationsAssembly(typeof(DataContext).Assembly.FullName)));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddScoped(typeof(DataLibrary.Repositry.IGenericRepositry<>), typeof(GenericRepositry<>));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IBase64, Base64>();
builder.Services.AddIdentity<DataLibrary.Models.Users, IdentityRole>()
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection(Constants.TokenSection).Value)),
        ValidateIssuer = false,
        ValidateAudience = false,

    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
//app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(builder.Configuration.GetSection(Constants.imagePath).Value),
    RequestPath = Constants.checking
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
