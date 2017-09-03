using NLog;
using NLog.Config;
using NLog.Targets;

namespace SharedLibrary
{
    public static class Logg
    {
        public static Logger logger;
        static Logg()
        {
            // Step 1. Create configuration object

            LoggingConfiguration config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration

            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            FileTarget fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);

            //// Step 3. Set target properties

            consoleTarget.Layout = "${date:format=HH\\:MM\\:ss} || ${callsite} || ${message}";
            fileTarget.FileName = "${basedir}/logs/log_${date:format=MM\\-dd\\-yyyy}.txt";
            fileTarget.Layout = "${date:format=HH\\:MM\\:ss} || ${callsite} || ${message}";
            fileTarget.ArchiveFileName = "${basedir}/archives/log.{#####}.txt";


            // Step 4. Define rules

            LoggingRule rule1 = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(rule1);

            LoggingRule rule2 = new LoggingRule("*", LogLevel.Fatal, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration

            LogManager.Configuration = config;
            logger = LogManager.GetCurrentClassLogger();
        }
    }
}
