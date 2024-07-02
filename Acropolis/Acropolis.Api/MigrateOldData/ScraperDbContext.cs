using Microsoft.EntityFrameworkCore;

namespace Acropolis.Api.MigrateOldData;

public static class Extensions
{
    public static void AddYoutubeDownloaderDataMigration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ScraperDbContext>(o =>
        {
            o.UseSqlite(configuration.GetConnectionString("scraper"));
        });
    }
}

public class ScraperDbContext : DbContext
{
    public ScraperDbContext(DbContextOptions<ScraperDbContext> options) : base(options)
    {
    }

    public DbSet<ScrapedPage> Pages => Set<ScrapedPage>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.UseCollation("BINARY");
        
        modelBuilder.Entity<ScrapedPage>()
            .Property(e => e.Id)
            .ValueGeneratedNever();

        modelBuilder.Entity<ScrapedPage>()
            .OwnsOne(e => e.Request, ob =>
            {
                ob.HasIndex(e => e.Url);
            })
            .OwnsOne(e => e.PageData, ob =>
            {
                ob.HasIndex(e => new { e.Title, e.Keywords, e.Domain });
            })
            .OwnsMany(e => e.Content, ob =>
            {
                ob.HasKey("Id");
                ob.HasIndex(e => e.FileName);
            });
    }
    
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>()
            .UseCollation("BINARY");
        
        configurationBuilder.Properties<ResourceTypes>()
            .HaveConversion<string>();
    }
}

public class ScrapedPage
{
    private ScrapedPage() { }

    public Guid Id { get; private set; }
    public ScrapeRequest Request { get; private set; }
    public PageData? PageData { get; set; }
    public List<ScrapedContent> Content { get; private set; } = new();

    public static ScrapedPage CreateFromRequest(ScrapeRequest scrapeRequest, Guid? id = null)
        => new()
        {
            Id = id ?? Guid.NewGuid(),
            Request = scrapeRequest
        };
}

public record PageData(string Title, string Keywords, string Domain);
public record ScrapeRequest(string Url, List<ResourceTypes> ResourceTypes, DateTimeOffset Timestamp);
public record ScrapedContent(string FileName, DateTimeOffset ScrapedOn);


public enum ResourceTypes
{
    Png,
    Pdf
}
