using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReenbitBooking.Infrastructure.Context;
using Testcontainers.MsSql;

namespace ReenbitBooking.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
        .WithPassword("Str0ng_Password!")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextOptionsDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (dbContextOptionsDescriptor is not null)
            {
                services.Remove(dbContextOptionsDescriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(_msSqlContainer.GetConnectionString()));

            // TestServer's ResponseBodyPipeWriter doesn't implement PipeWriter.UnflushedBytes,
            // which SystemTextJsonOutputFormatter/WriteAsJsonAsync relies on and throws on.
            // Buffer the response into a plain MemoryStream (backed by a normal StreamPipeWriter)
            // and copy it to the real body once the rest of the pipeline has finished writing.
            services.AddSingleton<IStartupFilter, BufferedResponseBodyStartupFilter>();
        });
    }

    private sealed class BufferedResponseBodyStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Use(async (context, nextMiddleware) =>
                {
                    var originalBody = context.Response.Body;
                    await using var buffer = new MemoryStream();
                    context.Response.Body = buffer;

                    try
                    {
                        await nextMiddleware();
                    }
                    finally
                    {
                        context.Response.Body = originalBody;
                        buffer.Seek(0, SeekOrigin.Begin);
                        await buffer.CopyToAsync(originalBody);
                    }
                });

                next(app);
            };
        }
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.MigrateAsync();
    }

    public new async Task DisposeAsync()
    {
        await _msSqlContainer.StopAsync();
        await base.DisposeAsync();
    }
}
