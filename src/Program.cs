using Microsoft.AspNetCore.Authentication.Cookies;
using System.Data;
using VetVaxManager.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddScoped<IDbConnection>((sp) => new Npgsql.NpgsqlConnection(connectionString));

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddTransient<IAnimalRepository, AnimalRepository>();
builder.Services.AddTransient<IVaccineRepository, VaccineRepository>();
builder.Services.AddTransient<ICalendarRepository, CalendarRepository>();
builder.Services.AddTransient<IOwnerRepository, OwnerRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

// Configuração da autenticação e cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/User/Login"; // Caminho para a página de login
    options.LogoutPath = "/User/Logout"; // Caminho para a página de logout
});

// Habilitar sessões
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar autenticação e sessões
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute( name: "default", pattern: "{controller=Animal}/{action=MyAnimals}/{id?}");
app.Run();
