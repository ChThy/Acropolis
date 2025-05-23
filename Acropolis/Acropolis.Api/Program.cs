using Acropolis.Api.Endpoints;
using Acropolis.Api.Extensions;
using Acropolis.Api.HostedServices;
using Scalar.AspNetCore;

namespace Acropolis.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddHostedService<DatabaseMigrator>();

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(p =>
            {
                p.WithOrigins("http://localhost:5173");
            });
        });

        builder.Services.AddServices(builder.Configuration, builder.Environment);

        var app = builder.Build();


        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthorization();

        app.MapOpenApi();
        app.MapScalarApiReference("");

        app.MapDownloadEndpoints();
        app.MapVideoEndpoints();
        app.MapPagesEndpoints();
        app.MapMigrationEndpoints();

        app.Run();
    }
}