<<<<<<< HEAD




=======
>>>>>>> MainTest
using GestionDeGastos;
using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio;
using GestionDeGastos.Servicio.Seguridad;
<<<<<<< HEAD
using GestionDeGastos.Servicios;

=======
>>>>>>> MainTest
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

<<<<<<< HEAD
=======


builder.Services.AddScoped<IHomeService, HomeServicio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddScoped<IPresupuestoRepositorio, PresupuestoRepositorio>();
builder.Services.AddScoped<IPresupuestoServicio, PresupuestoServicio>();
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
builder.Services.AddScoped<IContraseniaHasher, ContraseniaHasher>();
builder.Services.AddScoped<IGastoRepositorio, GastoRepositorio>();
builder.Services.AddScoped<IGastoServicio, GastoServicio>();
builder.Services.AddScoped<IVerTodosLosGastos, VerTodosLosGastosServicio>(); // lo hizo huesos
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<IMetodoDePagoRepositorio, MetodoDePagoRepositorio>();
builder.Services.AddScoped<IMetodoDePagoServicio, MetodoDePagoServicio>();
>>>>>>> MainTest

//cadena de conexion del appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestionDeGastosBdContext>(options =>
options.UseSqlServer(connectionString));




<<<<<<< HEAD
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddScoped<IContraseniaHasher, ContraseniaHasher>();
=======


>>>>>>> MainTest

builder.Services.AddScoped<IPresupuestoRepositorio, PresupuestoRepositorio>();
builder.Services.AddScoped<IPresupuestoServicio, PresupuestoServicio>();

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
    pattern: "{controller=Home}/{action=Home}/{id?}");

app.Run();
