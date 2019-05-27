using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Walton_Happy_Travel.Data;
using Walton_Happy_Travel.Models;
using Walton_Happy_Travel.Services;
using Stripe;
using Walton_Happy_Travel.DatabaseSeeders;
using jsreport.AspNetCore;
using jsreport.Local;
using jsreport.Binary;

namespace Walton_Happy_Travel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "816410532546-pi1a2h38i0lspipv9cq4im7bd7jcn614.apps.googleusercontent.com";
                options.ClientSecret = "L6fQlpWlEPKslTkgeFFMzKr0";
            });
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddTransient<Seeder>();
            services.AddMvc();
            services.AddJsReport(new LocalReporting()
                .UseBinary(JsReportBinary.GetBinary())
                .AsUtility()
                .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            Seeder.Initialise().Wait();
        }
    }
}
