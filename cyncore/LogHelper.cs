using NLog;

namespace CloudSync.Core
{
    public class LogHelper
    {
        /// <summary>
        ///  When verbose mode is specified, we add an additional colored-console target
        /// </summary>
        public static void MakeConsoleVerbose()
        {
            var config = LogManager.Configuration;
            // Find the 'consolelog' target and modify its level to Info
            foreach(var rr in config.LoggingRules)
            {
                foreach(var tt in rr.Targets)
                {
                    if(tt.Name == "consolelog")
                    {
                        rr.EnableLoggingForLevel(LogLevel.Debug);
                        break;
                    }
                }
            }
            //var ct = new ColoredConsoleTarget();
            //ct.Layout = "${message}";
            //ct.Name = "consolelog";
            //config.AddTarget(ct);
            //config.LoggingRules.Add(new NLog.Config.LoggingRule("*", LogLevel.Info, ct));
            LogManager.ReconfigExistingLoggers();
        }
    }
}
