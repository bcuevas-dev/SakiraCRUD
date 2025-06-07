using Microsoft.EntityFrameworkCore;
using SakilaWebCRUD.Datos;
using SakilaWebCRUD.Models;

var builder = WebApplication.CreateBuilder(args);

// Configura el contexto de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("SakilaConnection"),
        new MySqlServerVersion(new Version(8, 0, 36)) 
    )
);

// Agrega servicios MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configura el middleware (no toques esto si ya funciona)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configura la ruta por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Film}/{action=Index}/{id?}"
);

app.Run();
