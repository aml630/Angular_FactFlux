
using FactFluxV3.Attribute;
using FactFluxV3.Models;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FactFlux
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            staticConfig = configuration;
        }

        public IConfiguration Configuration { get; }

        public static IConfiguration staticConfig { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connection = Configuration["ConnectionStrings:FactFluxConnection"];

            services.AddMemoryCache();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSession();

            services.AddHttpContextAccessor();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDbContext<DB_A41BC9_aml630Context>(options => options.UseSqlServer(connection));

            services.AddSingleton<IConfiguration>(Configuration);

            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connection);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor httpContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.Use(async (context, next) =>

            {
                //*check the website-content and all elements like images

                string sHost = context.Request.Host.HasValue == true ? context.Request.Host.Value : "";  //domain without :80 port .ToString();

                sHost = sHost.ToLower();

                string sPath = context.Request.Path.HasValue == true ? context.Request.Path.Value : "";

                string sQuerystring = context.Request.QueryString.HasValue == true ? context.Request.QueryString.Value : "";

                if (!context.Request.IsHttps)

                {
                    string new_https_Url = "https://" + sHost;

                    if (sPath != "")

                    {
                        new_https_Url = new_https_Url + sPath;
                    }

                    if (sQuerystring != "")

                    {
                        new_https_Url = new_https_Url + sQuerystring;
                    }

                    context.Response.Redirect(new_https_Url);

                    return;
                }

                if (sHost.IndexOf("www.") == 0)
                {
                    string new_Url_without_www = "https://" + sHost.Replace("www.", "");

                    if (sPath != "")

                    {
                        new_Url_without_www = new_Url_without_www + sPath;
                    }

                    if (sQuerystring != "")

                    {
                        new_Url_without_www = new_Url_without_www + sQuerystring;
                    }

                    context.Response.Redirect(new_Url_without_www);

                    return;

                }
                await next();
            });

            app.UseStaticFiles();

            app.UseSpaStaticFiles();

            app.UseAuthentication();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseHangfireServer();

            var options = new DashboardOptions()
            {
                Authorization = new[] { new AllowAllDashboardAuthorizationFilter(httpContext) }
            };

            app.UseHangfireDashboard("/hangfire", options);

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
