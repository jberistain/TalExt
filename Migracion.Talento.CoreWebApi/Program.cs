using Migracion.Talento.WebAPI;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cultura predeterminada a "es-ES"
var cultureInfo = new CultureInfo("es-ES");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var startup = new Startup(builder.Configuration);
startup.ConfiurationServices(builder.Services);

var app = builder.Build();
var ServiceLogger = app.Services.GetService(typeof(ILogger<Startup>)) as ILogger<Startup>;
startup.Configure(app, app.Environment,ServiceLogger);
app.Run();
