using ChatSignalR.Hubs;
using ChatSignalR.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatSignalR
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    builder.AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed((host) => true)
                            .AllowCredentials();
                }));
            services.AddSignalR();
            services.AddControllers();
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ChatContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // services.AddDbContext<ChatContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddSingleton(c => Configuration);
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat/signalr");
                endpoints.MapControllers();
            });
        }
    }
}
