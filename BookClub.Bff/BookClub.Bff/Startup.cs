using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using BookClub.Bff.Books;
using BookClub.Bff.Exceptions;
using BookClub.Bff.Storage;
using MongoDB.Driver;

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
        services.AddSingleton<IMongoClient>(sp => Configuration["MongoConnectionString"] == null
        ? null
        : new MongoClient(Configuration["MongoConnectionString"]));
        services.AddScoped<IBooksService, BooksService>();
        services.AddScoped<IBooksRepo, MongoBooksRepo>();
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
