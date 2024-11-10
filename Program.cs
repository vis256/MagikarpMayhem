using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MagikarpMayhem.Data;
using MagikarpMayhem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MagikarpMayhemContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("MagikarpMayhemContext") ?? throw new InvalidOperationException("Connection string 'MagikarpMayhemContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{   options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/AccessDenied";
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ArenaService>();
builder.Services.AddScoped<PokemonService>();
builder.Services.AddScoped<PokedexService>();
builder.Services.AddScoped<IPokemonTypeService, PokemonTypeService>();

// Add services to the container.
builder.Services.AddControllersWithViews();


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

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();