using BTP.Models;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;// ilaina @ auth

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();

//ilaina @ session 
//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); afaka atsona any  @ vue fa mila alefa am constructeur
//builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    // Configurer les options de la session ici
    //options.Cookie.IsEssential = true; // Assure que le cookie de session est essentiel
    options.IdleTimeout = TimeSpan.FromMinutes(120);
});
//ilaina @ auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Auth/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });

// Configuration de la connexion � la base de donn�es PostgreSQL
var ConnectionStrings = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BtpDBContext>(options =>
    options.UseNpgsql(ConnectionStrings));



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

//ilaina @ Auht
app.UseAuthentication();

app.UseAuthorization();

//ilaina @ session

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=LoginClient}/{id?}");

app.UseRotativa();//ampiasaina refa manao export pdf de mametraka dossier Rotativa ao @ root

app.Run();
