using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RestWithASPNet.Business;
using RestWithASPNet.Business.Implementations;
using RestWithASPNet.Model.Context;
using RestWithASPNet.Repository;
using RestWithASPNet.Repository.Generic;
using Serilog;
using System;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;
using RestWithASPNet.Hypermedia.Filters;
using RestWithASPNet.Hypermedia.Abstract;
using RestWithASPNet.Hypermedia.Enricher;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Rewrite;

namespace RestWithASPNet
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Enviorment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;

            Enviorment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            //services para Content Negotiation
            services.AddMvc(
                options =>
                {
                    options.RespectBrowserAcceptHeader = true;

                    options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                    options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

                }
             )
            .AddXmlSerializerFormatters();

            //prov� conex�o com banco de dados MySQL
            var connection = Configuration["MySQLConnection:MySQLConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection, 
                ServerVersion.AutoDetect(connection)));

            //Migrations Implementation
            if (Enviorment.IsDevelopment())
            {
                MigrateDataBase(connection);
            }

            //HATEOAS IMPLEMENTATIONS
            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
            services.AddSingleton(filterOptions);

            //prov� versionamento de c�digo pelo pacote nuget
            services.AddApiVersioning();

            //SERVICES PARA DOCUMENTA��O SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "REST API With ASP.Net Core 5 and Docker",
                        Version = "v1",
                        Description = "REST API With ASP.Net Core 5 and Docker With SimpleCRUD",
                        Contact = new OpenApiContact
                        {
                            Name = "Wellington Mendes",
                            Url = new Uri ("https://github.com/wellmmjr")
                        }
                    });
            });

            //inje��o de depend�ncia para Person
            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();

            //inje��o de depend�ncia para Book
            services.AddScoped<IBookBusiness, BookBusinessImplementation>();

            //Implementa��o do generic repository
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            /* CONFIGURA��ES PARA DOCUMENTA��O DE SWAGGER --------- INICIO */
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API With ASP.Net Core 5 and Docker - V1");
            });

            var option = new RewriteOptions();

            option.AddRedirect("^$", "swagger");

            app.UseRewriter(option);
            /* CONFIGURA��ES PARA DOCUMENTA��O DE SWAGGER -------- FIM */

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}"); // adicionado para HATEOAS
            });
        }

        private void MigrateDataBase(string connection)
        {
            try
            {
                var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);
                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset"},
                    IsEraseDisabled = true,
                };
                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("DataBase Migration failed ", ex);
                throw;
            }
        }
    }
}
