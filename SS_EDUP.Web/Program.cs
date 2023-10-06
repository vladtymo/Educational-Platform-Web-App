using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SS_EDUP.Core;
using SS_EDUP.Core.Entities;
using SS_EDUP.Infrastructure;
using SS_EDUP.Infrastructure.Context;
using SS_EDUP.Infrastructure.Initializers;
using System;

var builder = WebApplication.CreateBuilder(args);

string connStr = builder.Configuration.GetConnectionString("DefaultConnection");
// Database context
builder.Services.AddDbContext(connStr);

// Add Repositories
builder.Services.AddRepositories();
// service configurations
builder.Services.AddCategoryAndCourseServices();

// Add Infrastructure Service  configurations
builder.Services.AddInfrastructureServices();

// Add Core Service  configurations
builder.Services.AddCoreServices();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add auto mapper
builder.Services.AddMapping();

// Add razor pages
builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10000);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
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
app.UseSession();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


await AppDbInitializer.SeedUsersAndRoles(app);
app.Run();
