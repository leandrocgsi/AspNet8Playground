using System;
using System.Collections.Generic;
using Microsoft.Net.Http.Headers;
using APIAspNetCore5.Business;
using APIAspNetCore5.Business.Implementations;
using APIAspNetCore5.Model.Context;
using APIAspNetCore5.Repository.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using APIAspNetCore5.Formatter;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace APIAspNetCore5
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
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
                options.RespectBrowserAcceptHeader = true;

                options.InputFormatters.Add(new YamlInputFormatter(deserializer));
                options.OutputFormatters.Add(new YamlOutputFormatter(serializer));
                options.FormatterMappings.SetMediaTypeMappingForFormat("yaml", MediaTypeHeaderValues.ApplicationYaml);

                options.FormatterMappings.SetMediaTypeMappingForFormat("xml", MediaTypeHeaderValue.Parse("application/xml"));
                options.FormatterMappings.SetMediaTypeMappingForFormat("json", MediaTypeHeaderValue.Parse("application/json"));

            })
            .AddXmlSerializerFormatters();

            services.AddApiVersioning();

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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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
