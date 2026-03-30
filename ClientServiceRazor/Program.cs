using ClientServiceRazor.Features.Data;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddRazorPages(options =>
        {
            /*
            options.RootDirectory = "/";
            Вказує кореневу папку для пошуку сторінок Razor Pages.
            За замовчуванням сторінки шукаються в "/Pages".
            "/" встановлює кореневу папку проєкту як базову для пошуку Razor Pages.
            */
            options.RootDirectory = "/";
            /*
            Прибирання службової частини шаблонів маршрутів Razor Pages, наприклад:
            Старий шаблон -> Новий шаблон
            /Pages/ ->
            /Pages/Index -> Index
            /Pages/Privacy -> Privacy
            /Features/Clients/Pages/ -> Clients/
            /Features/Clients/Pages/Index -> Clients/Index
            /Features/Clients/Pages/Details -> Clients/Details
            */
            options.Conventions.AddFolderRouteModelConvention(
                "/",
                model =>
                {
                    foreach (var selector in model.Selectors)
                    {
                        selector.AttributeRouteModel.Template =
                            selector.AttributeRouteModel.Template
                                .Replace("Features/", "")
                                .Replace("Pages/", "")
                                .Replace("Pages", "");
                    }
                }
            );
            /*
            Явне налаштування маршрутів для Razor Pages (залишено закоментованим)
            */
        });
        
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        var app = builder.Build();
        
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();

        // Redirect root to /Clients
        app.MapGet("/", context =>
        {
            context.Response.Redirect("/Clients");
            return Task.CompletedTask;
        });

        app.Run();
    }
}