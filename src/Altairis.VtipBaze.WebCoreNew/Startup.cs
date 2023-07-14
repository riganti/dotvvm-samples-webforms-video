using System.Net;
using System.Net.Mail;
using Altairis.VtipBaze.Data;
using DotVVM.Framework.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Altairis.VtipBaze.WebCore
{
    public class Startup
    {

        public IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();
            services.AddAuthorization();
            services.AddWebEncoders();
            services.AddAuthentication();

            services.AddDotVVM<DotvvmStartup>();
            services.AddSignalR();

            services.AddDbContext<VtipBazeContext>(options =>
            {
                options.UseSqlServer("Data Source=.\\SQLEXPRESS; Initial Catalog=VtipBaze; Integrated Security=true; Trust Server Certificate=true");
            });
            
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<VtipBazeContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
            });

            services.AddTransient(_ =>
            {
                var client = new SmtpClient();
                Configuration.GetSection("Smtp").Bind(client);
                return client;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseRouting();

			app.UseAuthentication();
            app.UseAuthorization();

            // use DotVVM
            var dotvvmConfiguration = app.UseDotVVM<DotvvmStartup>(env.ContentRootPath);
            dotvvmConfiguration.AssertConfigurationIsValid();
            
            // use static files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(env.WebRootPath)
            });

            app.UseEndpoints(endpoints => 
            {
                endpoints.MapDotvvmHotReload();

                // register ASP.NET Core MVC and other endpoint routing middlewares
            });
        }
    }
}
