using APIAspNetCore5.Business;
using APIAspNetCore5.Business.Implementations;
using APIAspNetCore5.Filters;
using APIAspNetCore5.Formatter;
using APIAspNetCore5.Hypermedia.Enricher;
using APIAspNetCore5.Model.Context;
using APIAspNetCore5.Repository.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace APIAspNetCore5
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        ///readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.PersonBusinessImplementation
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["MySqlConnection:MySqlConnectionString"];
            services.AddDbContext<MySQLContext>(options => options.UseMySql(connection));

            if (Environment.IsDevelopment())
            {
                MigrateDatabase(connection);
            }

            services.AddControllers();

            var serializer = (Serializer) new SerializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var deserializer = (Deserializer) new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();

            services.AddMvc(options =>
            {
                //options.EnableEndpointRouting = false;
                options.RespectBrowserAcceptHeader = true;

                options.InputFormatters.Add(new YamlInputFormatter(deserializer));
                options.OutputFormatters.Add(new YamlOutputFormatter(serializer));
                options.FormatterMappings.SetMediaTypeMappingForFormat("yaml", MediaTypeHeaderValues.ApplicationYaml);

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

            })
            .AddXmlSerializerFormatters();

            //Define as opções do filtro HATEOAS
            var filterOptions = new HyperMediaFilterOptions();
            filterOptions.ContentResponseEnricherList.Add(new PersonEnricher());
            filterOptions.ContentResponseEnricherList.Add(new BookEnricher());
            services.AddSingleton(filterOptions);

            services.AddApiVersioning();

            //Add Swagger Service
            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "REST API's RESTFul do 0 à Azure Com ASP.NET Core 5 e Docker",
                        Version = "v1",
                        Description = "API RESTful desenvolvida no curso 'REST API's RESTFul do 0 à Azure Com ASP.NET Core 5 e Docker'",
                        Contact = new OpenApiContact
                        {
                            Name = "Leandro Costa",
                            Url = new Uri("https://github.com/leandrocgsi")
                        }
                    });
            });
            services.AddCors(options =>
            {
                options.AddPolicy("FOO",
                    builder =>
                    {
                        builder.WithOrigins("http://www.erudio.com.br",
                                            "http://www.semeru.com.br",
                                            "http://localhost/")
                               .WithMethods("POST", "PUT", "PATCH", "DELETE", "GET");
                    });

                options.AddPolicy("AnotherPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://www.erudio.com.br",
                                            "http://localhost/")
                                            .AllowAnyHeader()
                                            .AllowAnyMethod()
                               .WithMethods("POST", "PUT", "PATCH", "DELETE", "GET");
                    });
            });

            //Dependency Injection
            services.AddScoped<IBookBusiness, BookBusinessImplementation>();
            services.AddScoped<IPersonBusiness, PersonBusinessImplementation>();
            services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("FOO");

            app.UseAuthorization();

            //Enable Swagger
            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API's RESTFul do 0 à Azure Com ASP.NET Core 5 e Docker - V1");
            });

            //Starting our API in Swagger page
            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("DefaultApi", "{controller=Values}/{id?}");
            });
        }

        private static void MigrateDatabase(string connection)
        {
            try
            {
                var evolveConnection = new MySql.Data.MySqlClient.MySqlConnection(connection);

                var evolve = new Evolve.Evolve(evolveConnection, msg => Log.Information(msg))
                {
                    Locations = new List<string> { "db/migrations", "db/dataset" },
                    IsEraseDisabled = true,
                };

                evolve.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error("Database migration failed.", ex);
                throw;
            }
        }

    }
}

// https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1
// https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-3.1#cpo