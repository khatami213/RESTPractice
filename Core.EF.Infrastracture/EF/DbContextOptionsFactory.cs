using Microsoft.EntityFrameworkCore;

namespace Core.EF.Infrastracture.EF;

public static class DbContextOptionsFactory
{
    public static DbContextOptions GetInstance<T>(string connectionString) where T : DbContext
    {
        var builder = new DbContextOptionsBuilder<T>();
        builder.UseSqlServer(connectionString)
            .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);

        builder.EnableSensitiveDataLogging();

        return builder.Options;
    }
}
