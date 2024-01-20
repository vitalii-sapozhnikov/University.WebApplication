using Microsoft.AspNetCore.Authentication.JwtBearer;
using University.Web.Services;
using University.Web.Services.Contracts;

namespace University.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<IHttpClientService, HttpClientService>()
                .ConfigureHttpClient((services, client) =>
                {
                    var baseUrl = services.GetRequiredService<IConfiguration>().GetValue<string>("BaseApiUrl");
                    client.BaseAddress = new Uri(baseUrl);
                });

            builder.Services.AddScoped<IApiService, ApiService>();
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(120); // Set session timeout as needed
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddScoped<ICookieService, CookieService>();

            builder.Services.AddScoped<ISessionService, SessionService>();


            builder.Services.AddScoped<ICloudStorageService, GoogleDriveStorageService>();

            builder.Services.AddSingleton<IDocumentService, DocumentService>();

            builder.Services.AddScoped<IAuthService, AuthService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
