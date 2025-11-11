using Azure.Identity;
using GestionDeGastos.AccesoADatos.Entidades;
using GestionDeGastos.Repositorio;
using GestionDeGastos.Servicio;
using GestionDeGastos.Servicio.GastoEspecifico;
using GestionDeGastos.Servicio.Seguridad;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Key vaults azure
var keyVaultUrl = new Uri("https://el-llavero.vault.azure.net/");
builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());


builder.Services.AddDbContext<GestionDeGastosBdContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

//Registra el servicio de la sesion
builder.Services.AddScoped<IUsuarioSession, UsuarioSession>();

//servicios
builder.Services.AddScoped<IHomeService, HomeServicio>();
builder.Services.AddScoped<IUsuarioService, UsuarioServicio>();
builder.Services.AddScoped<IPresupuestoServicio, PresupuestoServicio>();
builder.Services.AddScoped<IAutenticacionServicio, AutenticacionServicio>();
builder.Services.AddScoped<IGastoServicio, GastoServicio>();
builder.Services.AddScoped<ICategoriaServicio, CategoriaServicio>();
builder.Services.AddScoped<IMetodoDePagoServicio, MetodoDePagoServicio>();
builder.Services.AddScoped<IVerTodosLosGastos, VerTodosLosGastosServicio>();
builder.Services.AddScoped<IContraseniaHasher, ContraseniaHasher>();
builder.Services.AddScoped<IGastoEspecificoServicio, GastoEspecificoServicio>();
builder.Services.AddScoped<ILimiteDePresupuestoServicio, LimiteDePresupuestoServicio>();

// Servicios Azure
builder.Services.AddScoped<IBlobAzureServicio, BlobAzureServicio>();
builder.Services.AddScoped<IDocumentIntelligenceServicio, DocumentIntelligenceServicio>();

//repositorios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IPresupuestoRepositorio, PresupuestoRepositorio>();
builder.Services.AddScoped<IGastoRepositorio, GastoRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IMetodoDePagoRepositorio, MetodoDePagoRepositorio>();
builder.Services.AddScoped<IGastoEspecificoRepositorio, GastoEspecificoRepositorio>();




//Habilitar sesiones
builder.Services.AddSession(options =>
{
   options.IdleTimeout = TimeSpan.FromMinutes(30);
   options.Cookie.HttpOnly = true;
   options.Cookie.IsEssential = true;
});

//Azure functions
builder.Services.AddHttpClient("Functions", client =>
{
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

var app = builder.Build();

var cultureInfo = new CultureInfo("en-US");
cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
cultureInfo.NumberFormat.NumberGroupSeparator = ",";

CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

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
