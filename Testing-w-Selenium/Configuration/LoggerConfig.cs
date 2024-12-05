using Serilog;
using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;

namespace Testing_w_Selenium.Configuration
{
    public enum TestFramework
    {
        NUnit,
        XUnit
    }

    public static class LoggerConfig
    {
        private static readonly string LogTemplate = 
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{ThreadId}] [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static void ConfigureLogger(TestFramework framework)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var solutionDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
            var logsDir = Path.Combine(solutionDir ?? Directory.GetCurrentDirectory(), "logs");
            var archiveDir = Path.Combine(logsDir, "archive");
            
            Directory.CreateDirectory(logsDir);
            Directory.CreateDirectory(archiveDir);

            Log.Logger = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: LogTemplate)
                .WriteTo.File(
                    Path.Combine(logsDir, $"{framework}-{timestamp}.log"),
                    rollingInterval: RollingInterval.Infinite,
                    retainedFileCountLimit: 7,
                    fileSizeLimitBytes: 10 * 1024 * 1024, // 10MB
                    rollOnFileSizeLimit: true,
                    outputTemplate: LogTemplate)
                .CreateLogger();

            Log.Information(
                "Test run started - Framework: {Framework}, ID: {TestRunId}, OS: {OS}, Machine: {Machine}, User: {User}",
                framework,
                timestamp,
                Environment.OSVersion,
                Environment.MachineName,
                Environment.UserName);
                
            ArchiveOldLogs(logsDir, archiveDir);
        }

        private static void ArchiveOldLogs(string logsDir, string archiveDir)
        {
            var oldLogs = Directory.GetFiles(logsDir, "*.log")
                .Where(f => File.GetCreationTime(f) < DateTime.Now.AddDays(-7));

            foreach (var log in oldLogs)
            {
                var fileName = Path.GetFileName(log);
                var archivePath = Path.Combine(archiveDir, fileName);
                File.Move(log, archivePath, true);
                Log.Information("Archived old log file: {FileName}", fileName);
            }
        }

        public static void CloseAndFlush()
        {
            Log.Information("Test run completed");
            Log.CloseAndFlush();
        }

        public static void LogFatal(Exception ex, string message, params object[] propertyValues)
        {
            Log.Fatal(ex, "FATAL ERROR: " + message, propertyValues);
            CloseAndFlush();
        }
    }
}