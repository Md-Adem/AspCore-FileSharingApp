using ClosedXML.Excel;
using FileSharingApp.Areas.Admin.Services;
using FileSharingApp.Data;
using FileSharingApp.Helpers.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(option =>
    {
        option.ResourcesPath = "Resources";
    });



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddTransient<IMailHelper,MailHelper>();

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddTransient<IContactUsService, ContactUsService>();

// Export To Excel Service From Nuget ClosedXML
builder.Services.AddTransient<IXLWorkbook, XLWorkbook>();

builder.Services.AddLocalization();



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


var supportedCulture = new[] { "en-US", "ar-SA" };
app.UseRequestLocalization(r =>
{
    r.AddSupportedUICultures(supportedCulture);
    r.AddSupportedCultures(supportedCulture);
    r.SetDefaultCulture("en-US");
});

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



using ( var scope = app.Services.CreateScope())
{
    var provider = scope.ServiceProvider;


    // seeding Admin User in production 
    var userService = provider.GetRequiredService<IUserService>();
    await userService.Initialize();
}

app.Run();
