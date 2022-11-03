using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{

    public class Startup
    {
        // Read value from appSettings.json file
        private IConfiguration _config;


        public Startup(IConfiguration config)
        {
            _config = config;
        }           
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            //app.UseFileServer();
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello1 from use");
            //    next();
            //});

            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("Hello2 from use");
            //    next();
            //});




            app.Map("/test1", MyCustomMiddleware);
                

            app.Map("/test", 
                context =>context.Run(context1 => context1.Response.WriteAsync("test is being called")
                 ));

            app.Map("/newbranch", a => {
                a.Map("/branch1", brancha => brancha
                    .Run(c => c.Response.WriteAsync("Running from the newbranch/branch1 branch!")));
                a.Map("/branch2", brancha => brancha
                    .Run(c => c.Response.WriteAsync("Running from the newbranch/branch2 branch!")));

                a.Run(c => c.Response.WriteAsync("Running from the newbranch branch!"));
            });


            app.Run(async context =>
            {
                //context.Response.WriteAsync("Hello");
                context.Response.WriteAsync(_config["Message"]);
            });

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello1 in Between");
            //});

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync("Hello2 in Between");
            //});
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        await context.Response.WriteAsync("Hello World!");
            //    });
            //});

               }

        private void MyCustomMiddleware(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await context.Response.WriteAsync("Custom Middleware called");
                next();
            });
        }

    }
}
