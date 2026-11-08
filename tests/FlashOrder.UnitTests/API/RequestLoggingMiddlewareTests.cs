using FlashOrder.API.Middleware;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FlashOrder.UnitTests.API;

public class RequestLoggingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldWriteStructuredInformationLog()
    {
        var logger = new TestLogger<RequestLoggingMiddleware>();
        var middleware = new RequestLoggingMiddleware(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status201Created;
            await Task.CompletedTask;
        }, logger);

        var context = new DefaultHttpContext();
        context.Request.Method = "GET";
        context.Request.Path = "/health";
        context.TraceIdentifier = "trace-test-123";

        await middleware.InvokeAsync(context);

        var log = Assert.Single(logger.Entries);
        Assert.Equal(LogLevel.Information, log.Level);
        Assert.Equal("HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms traceId={TraceId}", log.Template);
        Assert.Equal("GET", log.Values["Method"]);
        Assert.Equal("/health", log.Values["Path"]);
        Assert.Equal(201, log.Values["StatusCode"]);
        Assert.Equal("trace-test-123", log.Values["TraceId"]);
        Assert.True(Convert.ToDouble(log.Values["ElapsedMs"]) >= 0);
    }

    private sealed class TestLogger<T> : ILogger<T>
    {
        public List<LogEntry> Entries { get; } = [];

        public IDisposable BeginScope<TState>(TState state) where TState : notnull => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            var values = state as IReadOnlyList<KeyValuePair<string, object?>>;
            var map = values?.ToDictionary(k => k.Key, v => v.Value) ?? new Dictionary<string, object?>();
            var template = map.TryGetValue("{OriginalFormat}", out var original)
                ? original?.ToString() ?? string.Empty
                : string.Empty;

            Entries.Add(new LogEntry(logLevel, template, map));
        }
    }

    private sealed record LogEntry(LogLevel Level, string Template, Dictionary<string, object?> Values);

    private sealed class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new();

        public void Dispose()
        {
        }
    }
}
