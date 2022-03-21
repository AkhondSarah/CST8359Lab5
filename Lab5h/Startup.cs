using Azure.Storage.Blobs;
using Lab5h.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab5h
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
            /* Define services here */
            var connection = Configuration.GetConnectionString("DefaultDBConnection");
            services.AddDbContext<AnswerImageDataContext>(options => options.UseSqlServer(connection));

            var blobConnection = Configuration.GetConnectionString("AzureBlobStorage");
            services.AddSingleton(new BlobServiceClient(blobConnection));

            services.AddRazorPages();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            /* Define routing here */
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}