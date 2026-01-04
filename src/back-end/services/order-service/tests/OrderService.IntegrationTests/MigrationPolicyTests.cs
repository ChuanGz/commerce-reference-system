using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderService.Infrastructure.Persistence;
using Xunit;

namespace OrderService.IntegrationTests;

public class MigrationPolicyTests {
    [Fact]
    public async Task ProductionStartup_DoesNotRunMigrations() {
        var services = new ServiceCollection();
        services.AddDbContext<OrderDbContext>(options =>
            options.UseInMemoryDatabase("order-migration-test")
        );

        var loggerProvider = new ListLoggerProvider();
        using var loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(loggerProvider));
        var logger = loggerFactory.CreateLogger("MigrationTest");

        await DatabaseInitializer.InitializeAsync(
            services.BuildServiceProvider(),
            logger,
            isDevelopment: false
        );

        Assert.Contains(
            loggerProvider.Messages,
            message => message.Contains(
                "Skipping automatic migrations for Order Service",
                StringComparison.OrdinalIgnoreCase
            )
        );
    }
}

internal sealed class ListLoggerProvider : ILoggerProvider {
    public List<string> Messages { get; } = new();

    public ILogger CreateLogger(string categoryName) => new ListLogger(Messages);

    public void Dispose() { }
}

internal sealed class ListLogger : ILogger {
    private readonly List<string> _messages;

    public ListLogger(List<string> messages) {
        _messages = messages;
    }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter
    ) {
        _messages.Add(formatter(state, exception));
    }

    private sealed class NullScope : IDisposable {
        public static readonly NullScope Instance = new();

        public void Dispose() { }
    }
}
