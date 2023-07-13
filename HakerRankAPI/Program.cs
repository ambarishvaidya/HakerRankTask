using HackerRankClient;
using HackerRankClient.HttpImplementation;

namespace HakerRankAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;
            var dataUrl = configuration.GetValue<string>("HackerNewsUrl");
            var dataApiVersion = configuration.GetValue<string>("HackerNewsApiVersion");

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IHttpHackerRank, HttpHackerRankImpl>(p => new HttpHackerRankImpl(dataUrl, dataApiVersion));
            builder.Services.AddScoped<IHackerRankWebClient, HackerRankWebClientImplementation>();

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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}