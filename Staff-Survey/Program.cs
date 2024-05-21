using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Staff_Survey.Models.DataBaseContext;
using Staff_Survey.Models.Entities;
using Staff_Survey.Models.Interfaces;
using System.Net;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAuthentication(option =>
{
    option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = new PathString("/Signin");
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
    options.AccessDeniedPath = new PathString("/Signin");
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Signin";
});

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("User", policy =>
    {
        policy.RequireClaim("user");
    });

    option.AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
var ConnectionStrings = builder.Configuration.GetConnectionString("Survey-Connection");
builder.Services.AddDbContext<DataBaseContext>(option => option.UseSqlServer(ConnectionStrings));

builder.Services.AddScoped<IDbContext, DataBaseContext>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IUserLoginService, UserLoginService>();

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<DataBaseContext>()
               .AddDefaultTokenProviders();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Authentication}/{action=signin}/{id?}");

app.Run();
