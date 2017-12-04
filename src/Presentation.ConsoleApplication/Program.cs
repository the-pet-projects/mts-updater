namespace PetProjects.MicroTransactionsUpdater.Presentation.ConsoleApplication
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using PetProjects.Framework.Consul;
    using PetProjects.Framework.Consul.Store;
    using PetProjects.Framework.Logging.Producer;
    using Serilog.Events;
    using KafkaConfiguration = PetProjects.MicroTransactionsUpdater.Presentation.ConsoleApplication.Configuration.KafkaConfiguration;

    public class Program
    {
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            // Setup Configuration with appsettings
            Program.SetupConfiguration();

            // Setup DI container
            var serviceCollection = new ServiceCollection();
            Program.SetupServices(serviceCollection, Program.GetConfigurationKeyValueStore());

            // Do the actual work here
            using (var parentServiceProvider = serviceCollection.BuildServiceProvider())
            {
                Program.Run(parentServiceProvider);
            }
        }

        private static IStringKeyValueStore GetConfigurationKeyValueStore()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddPetProjectConsulServices(Program.Configuration, true);
            serviceCollection.AddSingleton<ILogger>(NullLogger.Instance);

            using (var tempProvider = serviceCollection.BuildServiceProvider())
            {
                return tempProvider.GetRequiredService<IStringKeyValueStore>();
            }
        }

        private static void SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables("MTS_APP_SETTINGS_");

            Program.Configuration = builder.Build();
        }

        private static void SetupServices(IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            Program.SetupLogging(serviceCollection, configStore);

            serviceCollection.AddPetProjectConsulServices(Program.Configuration, true);
            serviceCollection.AddTransient<KafkaConfiguration>(sp =>
            {
                var store = sp.GetRequiredService<IStringKeyValueStore>();
                return new KafkaConfiguration
                {
                    Brokers = store.GetAndConvertValue<string>("ApplicationKafkaConfiguration/Brokers").Split(',')
                };
            });
        }

        private static void SetupLogging(IServiceCollection serviceCollection, IStringKeyValueStore configStore)
        {
            var kafkaConfig = new Framework.Logging.Producer.KafkaConfiguration
            {
                Brokers = configStore.GetAndConvertValue<string>("Logging/KafkaConfiguration/Brokers").Split(','),
                Topic = configStore.GetAndConvertValue<string>("Logging/KafkaConfiguration/Topic")
            };

            var sinkConfig = new PeriodicSinkConfiguration
            {
                BatchSizeLimit = configStore.GetAndConvertValue<int>("Logging/BatchSizeLimit"),
                Period = TimeSpan.FromMilliseconds(configStore.GetAndConvertValue<int>("Logging/PeriodMs"))
            };

            var logLevel = configStore.GetAndConvertValue<LogEventLevel>("Logging/LogLevel");
            var logType = configStore.GetAndConvertValue<string>("Logging/LogType");

            serviceCollection.AddLogging(builder => builder.AddPetProjectLogging(logLevel, sinkConfig, kafkaConfig, logType, true));
        }

        private static void Run(IServiceProvider scopedProvider)
        {
            var logger = scopedProvider.GetService<ILoggerFactory>().CreateLogger<Program>();

            logger.LogCritical("Starting MicroTransactionsUpdater...");
            
            Console.CancelKeyPress += (sender, eArgs) =>
            {
                Program.QuitEvent.Set();
                eArgs.Cancel = true;
            };

            Program.QuitEvent.WaitOne();

            logger.LogCritical("LogAggregator MicroTransactionsUpdater...");

            // wait 2 seconds for previous log to reach the sink
            Task.Delay(TimeSpan.FromMilliseconds(2000)).Wait();
        }
    }
}