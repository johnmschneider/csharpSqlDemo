using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ADODB;
using jobDemo1.Models;

namespace jobDemo1
{
    public class Startup
    {
        private static Connection dbConnection;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
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
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            ConfigureSql();
            TestSql();
        }

        private void ConfigureSql()
        {
            // normally, the password shouldn't be hardcoded
            string connectionStr = "Data Source=.\\SQLEXPRESS; Initial Catalog=dbJobDemo1; User Id=sa; Password=LehtMehEen";
            dbConnection = new Connection();
            dbConnection.Open(connectionStr);
        }

        public static Connection GetConnectionReference()
        {
            return dbConnection;
        }

        private void TestSql()
        {
            var conn = GetConnectionReference();
            var test = new Album(conn);

            test.Name = "Fear Inoculum";
            test.Retrieve();

            Console.WriteLine("[Startup.cs, TestSql]: Retrieved Album.. year == " + test.ReleaseYear);

            test.Close();

            // we're done with the database now
            conn.Close();
        }
    }
}
