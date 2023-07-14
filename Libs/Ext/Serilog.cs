using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System.Reflection;

namespace MyFinanceFy.Libs.Ext
{
    public static class Serilog
    {
        public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder webApplicationBuilder)
        {
            FileLog();
            return webApplicationBuilder;
        }
        private static void FileLog()
        {
            string? dirLog = Path.Combine($"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}", $"log_{DateTime.Now:dd-MM-yyyy}.log");
            Log.Logger = ConfigBase()
                .WriteTo.Async(w =>
                    w.File(path: dirLog,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"))
                .CreateLogger();
        }
        private static LoggerConfiguration ConfigBase()
        {
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithClientIp()
                .Enrich.WithCorrelationId()
                .WriteTo.Console();
        }
    }
}
