using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.MigrateOldData;

public static class Extensions
{
    public static void AddYoutubeDownloaderDataMigration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<YoutubeDownloaderDbContext>(o =>
        {
            o.UseSqlite(configuration.GetConnectionString("youtubedownloader"));
        });
    }
}

public class YoutubeDownloaderDbContext : DbContext
{
    public YoutubeDownloaderDbContext(DbContextOptions<YoutubeDownloaderDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseCollation("BINARY");
        
        modelBuilder.Entity<YoutubeDownload>().ToTable("Videos");
        modelBuilder.Entity<YoutubeDownload>()
            .OwnsOne(e => e.Request, ob =>
            {
                ob.Property(e => e.VideoId).IsRequired();
            });

        modelBuilder.Entity<YoutubeDownload>()
            .OwnsOne(e => e.Video, ob =>
            {
                ob.Property(e => e.StorageLocation)
                    .IsRequired();
            });
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .UseCollation("BINARY");
    }
}

public class YoutubeDownload
{
    public Guid Id { get; set; }
    public Request? Request { get; set; }
    public DownloadVideo? Video { get; set; }
}

public record Request(string VideoId, DateTimeOffset Timestamp);

public record DownloadVideo(
    string Title,
    string Author,
    DateTimeOffset UploadTimeStamp,
    string StorageLocation);