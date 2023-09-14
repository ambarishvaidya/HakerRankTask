using HackerNewsClient;
using HackerNewsClient.Cache;
using HackerNewsClient.HttpImplementation;
using HackerNewsClient.Util;

namespace HackerNewsAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddLog4Net();

        ConfigurationManager configuration = builder.Configuration;

        builder.Services.AddHttpClient();
        builder.Services.AddControllers();
        builder.Services.AddSingleton<ICommonOperations, CommonOperations>();
        builder.Services.AddSingleton<IHackerNewsCache, HackerNewsCacheImpl>();
        builder.Services.AddScoped<IHttpHackerNews, HttpHackerNewsImpl>();
        builder.Services.AddScoped<IHackerNewsWebClient, HackerNewsWebClientImpl>();            
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true));
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}