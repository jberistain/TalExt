using Migracion.Talento.WebAPI;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);
startup.ConfiurationServices(builder.Services);

var app = builder.Build();
var ServiceLogger = app.Services.GetService(typeof(ILogger<Startup>)) as ILogger<Startup>;
startup.Configure(app, app.Environment,ServiceLogger);
app.Run();
