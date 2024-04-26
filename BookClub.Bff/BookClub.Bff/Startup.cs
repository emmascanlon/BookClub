using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using BookClub.Bff.Books;
using BookClub.Bff.Exceptions;

namespace BookClub.Bff;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration {get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IBooksService, BooksService>();
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
    {
        app.UseCustomExceptionHandler(env, loggerFactory);
        app.UseRouting();
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
