using CommonTools.DTOs;
using CommonTools.DTOs.Query;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Migracion.Talento.CoreWebApi.Filters;
using Migracion.Talento.CoreWebApi.Interfaces;
using Migracion.Talento.CoreWebApi.MiddleWares;
using Migracion.Talento.CoreWebApi.Services;
using Migracion.Talento.WebAPI.DataConnection;

namespace Migracion.Talento.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration) 
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; set; }

        public void ConfiurationServices(IServiceCollection services)
        {
            services.AddResponseCaching();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ExceptionFilter));
            });
            //Conecction azure  Temporal
            var stringConnection= Configuration.GetConnectionString("defaultConnection");

            services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer(stringConnection)
                );
            //Inyecion de dependencia transitorio Envio correo
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IDocumentsEvents, DocumentsEvents>();



            //inyection of config of data email
            services.Configure<EmailSenderOptions>(Configuration.GetSection("EmailSenderOptions"));
            services.Configure<ApplicationProperties>(Configuration.GetSection("ApplicationProperties"));

            //services.AddTransient();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //mapper
            services.AddAutoMapper(typeof(Startup));
        }
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env, ILogger<Startup>? logger)
        {

            app.UseLoggerResponse();
            //app.Run(async contexto =>
            //await contexto.Response.WriteAsync("MiddleWare"));
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //swagger production
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

