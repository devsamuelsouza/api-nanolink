using DotNetEnv;
using nanolink.Routes;

class Program
{
    public static void Main()
    {
        Env.Load();
        Console.WriteLine();
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddCors(config =>
        {
            config.AddPolicy("Default", policy =>
            {
                policy.WithHeaders("Content-Type", "Authorization")
                      .WithMethods("GET", "UPDATE", "DELETE", "POST");
            });
        });

        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(10);
        });

        var app = builder.Build();

        app.UseSession();
        app.Rotas();
        app.Run();
    }
}