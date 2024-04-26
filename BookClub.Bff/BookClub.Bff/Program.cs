using BookClub.Bff;


var configBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    // This does not support environment-specific configuration files.
    // Instead, per-environment logging configuration should be accomplished by
    // modifying appsettings.json during packaging or releasing.
    .AddEnvironmentVariables();

var Configuration = configBuilder.Build();

Console.WriteLine("Building and running WebHost");

try
{
    var builder = Host.CreateDefaultBuilder(args);
    var build = builder
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
            webBuilder.UseConfiguration(Configuration);
            // Leaving the web root directory unspecified defaults to this same value ("wwwroot").
            // However, just letting it default means that if the directory doesn't exist
            // _at startup time_,  the Static Files middleware won't serve any files at all! By
            // specifying it, the directory will be created on startup if necessary and files
            // that appear in it later will be served. This is useful during local development
            // because webpack can be putting new files in there.
            webBuilder.UseWebRoot("wwwroot");
        })
        .Build();


    build.Run();
    Console.WriteLine("WebHost exiting normally");
}
catch (Exception ex)
{
    Console.WriteLine("WebHost terminated unexpectedly.", ex.Message);
    return 1;
}
finally
{
    Console.WriteLine("Close and Flush");
}
return 0;

