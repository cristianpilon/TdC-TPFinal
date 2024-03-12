using GestorCV.API.Infraestructura;
using GestorCV.API.Infraestructura.Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GestorCV.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Inicializa variables estáticos para configuraciones generales.
            // NOTA: Esto podría generarse por medio del registro de servicios para inyección de dependencias
            // nativo de NET Core pero perdería visibilidad y complejizaría el modelo para inyectar variables
            // muy sencillas. Si bien esto se considera una mala práctica (los desarrolladores podrían acceder
            // fácilmente a estos valores desde cualquier parte de la aplicación) la finalidad es no perder
            // visibilidad de donde se guardan estos valores e inicializarlos solo al comienzo.
            _ = new AppConfiguration(configuration);

            // Se crea carpeta de backups si no existe
            System.IO.Directory.CreateDirectory(AppConfiguration.RutaRespaldos);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.CustomSchemaIds(type => type.ToString());
            });
            services.AddControllersWithViews(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
            });

            Logger.ConfigureLogger();

            // Agrego configuracion para autorizar solo usuarios con token
            var jwtIssuer = AppConfiguration.FirmaToken;
            var jwtKey = AppConfiguration.ClaveToken;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = jwtIssuer,
                     ValidAudience = jwtIssuer,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                 };
             });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gestor de Curriculums");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<InterceptorAuditoriaAcciones>()
                .UseMiddleware<ErrorHandlerMiddleware>()
                .UseMiddleware<AutorizacionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowAnyOrigin()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
