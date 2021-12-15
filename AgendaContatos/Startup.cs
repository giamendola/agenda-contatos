using AgendaContatos.DbRepository;
using AgendaContatos.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace AgendaContatos
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            string mySqlConnection = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContextPool<AppDbContext>(options => 
                options.UseMySql(mySqlConnection, 
                ServerVersion.AutoDetect(mySqlConnection)));            

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "AgendaContatos", 
                    Version = "v1", 
                    Contact = new OpenApiContact 
                    { 
                      Name = "Giovanni Amêndola Fonseca", 
                      Email = "giovanniamendola_0@yahoo.com.br", 
                      Url = new Uri("https://www.linkedin.com/in/giovanni-amendola/") 
                    }
                    
                });

                c.CustomSchemaIds(x => x.FullName);
                c.EnableAnnotations();
                c.DocumentFilter<JsonPatchDocumentFilter>();
                
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgendaContatos v1"));
            }

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
