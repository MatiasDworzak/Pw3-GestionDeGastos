


using GestionDeGastos;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio;
using GestionDeGastos.Servicio.Seguridad;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//cadena de conexion del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestionDeGastosBdContext>(options =>
options.UseSqlServer(connectionString));




builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddScoped<IContraseniaHasher, ContraseniaHasher>();

//Habilitar sesiones
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
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();


app.UseAuthorization();


//validar que el usuario no pueda acceder al Home si no esta registrado o con la sesion iniciada,
//deberia ir a Home/Inicio
//por ahora Ingreso/Register

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Inicio}/{id?}");

app.Run();
