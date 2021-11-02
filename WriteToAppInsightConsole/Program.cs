using System;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;

namespace WriteToAppInsightConsole
{
    class Program
    {
        private static Logger _logger;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var builder = new ConfigurationBuilder()
                .AddEnvironmentVariables();

            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = "==>APP INSIGHT KEY<==";

            var config = builder.Build();
            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .WriteTo.ApplicationInsights(telemetryConfiguration, TelemetryConverter.Traces)
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();

            Console.Write("Antwoord {0}", DoSomething(20, 0));
            
            Console.ReadLine();
        }

        private static decimal DoSomething(decimal x, decimal y)
        {
            _logger.Information($"Do something with {x} and {y}");
            
            try
            {
                return x / y;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Cannot divide {x} by {y}");
            }

            return -1;
        }
    }
}