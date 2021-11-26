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
using RestWithASPNet.Services;
using RestWithASPNet.Services.Implementations;
using RestWithASPNet.Repository.User;
using RestWithASPNet.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using RestWithASPNet.Repository.Person;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;

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

            /* CONFIGURAÇÕES PARA SEGURANÇA E TOKEN ------------------- INICIO */

            var tokenConfigurations = new TokenConfiguration();

            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                Configuration.GetSection("TokenConfigurations")
                )
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = tokenConfigurations.Issuer,
                        ValidAudience = tokenConfigurations.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(
                                    tokenConfigurations.Secret
                                    )
                            )
                    };
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser().Build()
                    );
            });

            /* CONFIGURAÇÕES PARA SEGURANÇA E TOKEN ------------------- FIM */

            //services para Configurar CORS - CROSS ORIGIN RESOURCE SERVICE
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));

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

            //provê conexão com banco de dados MySQL
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

            //provê versionamento de código pelo pacote nuget
            services.AddApiVersioning();

            //SERVICES PARA DOCUMENTAÇÃO SWAGGER
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

            //injeção de dependência para Person
            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();

            //injeção de dependência para Book
            services.AddScoped<IBookBusiness, BookBusinessImplementation>();

            //Implementação do generic repository
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));


            //injeção para LOGIN e AUTENTICAÇÃO
            services.AddScoped<ILoginBusiness, LoginBusinessImplementation>();
            
            //injeção para USER 
            services.AddScoped<IUserRepository, UserRepository>();

            //Injeção para TOKEN Services
            services.AddTransient<ITokenService, TokenService>();
            
            //Injeção para PATCH de IPersonRepository
            services.AddScoped<IPersonRepository, PersonRepository>();

            //Injeção para sistema de uploads de arquivos
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>(); //<== para configurações de http, usadas na interface 
            services.AddScoped<IFileBusiness, FileBusinessImplementation>();

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

            //PARA HABILITAR CORS (DEVE FICAR APÓS "UseHttpsRedirection" E "UseRouting" ASSIM COMO ANTES DE "UseEndpoints"
            app.UseCors();

            /* CONFIGURAÇÕES PARA DOCUMENTAÇÃO DE SWAGGER --------- INICIO */
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API With ASP.Net Core 5 and Docker - V1");
            });

            var option = new RewriteOptions();

            option.AddRedirect("^$", "swagger");

            app.UseRewriter(option);
            /* CONFIGURAÇÕES PARA DOCUMENTAÇÃO DE SWAGGER -------- FIM */

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // adicionado para HATEOAS
                endpoints.MapControllerRoute("DefaultApi", "{controller=values}/{id?}");
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
