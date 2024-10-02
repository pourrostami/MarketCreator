
using MarketCreator.DataLayer.Entities.Account;
using MarketCreator.DataLayer.Repository;
using MarketCreator.Web.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


#region Config Services

//??????? ???? ?? ???? ???? ?? ??? ???? ???????
//???? ???? ???????? ????? ???? ??? ? ???? ??

//builder.Services.AddScoped<IGenericRepository<User>,GenericRepository<User>>(); 

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
