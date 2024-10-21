
using MarketCreator.Application.Services.Implementations;
using MarketCreator.Application.Services.Interfaces;
using MarketCreator.DataLayer.Context;
using MarketCreator.DataLayer.Entities.Account;
using MarketCreator.DataLayer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


#region Config Services
  

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService,UserService>();

#endregion

#region Config Database
builder.Services.AddDbContext<MarketCreatorDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ,
        b=>b.MigrationsAssembly("MarketCreator.Web")
        );
});
#endregion

#region Config Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme= CookieAuthenticationDefaults.AuthenticationScheme;   
    options.DefaultChallengeScheme= CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme= CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    options.LoginPath = "/login";
    options.LogoutPath = "/log-out";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(43200);
});

#endregion

#region Html Encoder

builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(new[] {
    UnicodeRanges.BasicLatin, UnicodeRanges.Arabic
}));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
