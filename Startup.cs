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
<<<<<<< HEAD
=======
using Stripe;
using Walton_Happy_Travel.DatabaseSeeders;
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e

namespace Walton_Happy_Travel
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
<<<<<<< HEAD
=======
            StripeConfiguration.SetApiKey(Configuration.GetSection("Stripe")["SecretKey"]);
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "816410532546-pi1a2h38i0lspipv9cq4im7bd7jcn614.apps.googleusercontent.com";
                options.ClientSecret = "L6fQlpWlEPKslTkgeFFMzKr0";
            });
<<<<<<< HEAD
=======
            services.Configure<StripeSettings>(Configuration.GetSection("Stripe"));
            services.AddTransient<Seeder>();
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
<<<<<<< HEAD
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
=======
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, Seeder seeder)
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
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
<<<<<<< HEAD
=======
            //seeder.Seed();
>>>>>>> c089588605b4ee3cede64435b177a54f071bfe1e
        }
    }
}
