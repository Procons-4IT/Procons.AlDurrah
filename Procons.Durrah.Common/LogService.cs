namespace Procons.Durrah.Common
{
    using NLog;
    using NLog.Config;
    using NLog.Targets;
    using System;
    using System.Diagnostics;
    using System.Text;

    public class LogService: ILoggingService
    {
        public string TargetName { get; set; }
        private Logger _siteLogger;

        public LogService()
        {
            TargetName = "Durra";
            var template = new StringBuilder("-------------- ${level} (${longdate}) --------------${newline}");
            template.Append("${newline}");
            template.Append("Call Site: ${callsite}${newline}");
            template.Append("Exception Type: ${exception:format=Type}${newline}");
            template.Append("Exception Message:${exception:format=Message}${newline}");
            template.Append("Stack Trace: ${exception:format=StackTrace}${newline}");
            template.Append("Additional Info: ${message}${newline}");

            var target = new FileTarget();
            target.Name = TargetName;
            target.FileName = $"c:\\Durra logs\\{Guid.NewGuid()}.log";
            target.Layout = template.ToString();

            var config = new LoggingConfiguration();
            config.AddTarget(TargetName, target);

            var rule = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;

            _siteLogger = LogManager.GetLogger(TargetName);
        }

        public void LogException(Exception ex, string message)
        {
            _siteLogger.Error(ex, message);
        }
        public void LogException(Exception ex)
        {
            //LogException(ex, $"{ new StackTrace(ex).GetFrame(0).GetMethod().Name}: {ex.InnerException}");
            LogException(ex, $"Error: {ex.InnerException}");
        }
    }
}
