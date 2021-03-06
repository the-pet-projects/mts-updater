﻿namespace IntegrationTests
{
    using System.Collections.Generic;

    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Newtonsoft.Json;
    using PetProjects.Framework.Consul;
    using PetProjects.Framework.Consul.Store;

    public class AppSettings
    {
        static AppSettings()
        {
            var assembly = typeof(AssemblyInitialize).GetTypeInfo().Assembly;
            var fileName = "IntegrationTests.appsettings.json";
            using (var stream = assembly.GetManifestResourceStream(fileName))
            {
                var jsonString = new StreamReader(stream).ReadToEnd();
                var kvpList = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonString);

                var builder = new ConfigurationBuilder()
                    .AddInMemoryCollection(kvpList)
                    .AddEnvironmentVariables("MTS_APP_SETTINGS_");
                AppSettings.Configuration = builder.Build();
            }

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<ILoggerFactory>(NullLoggerFactory.Instance);
            serviceCollection.AddSingleton<ILoggerProvider>(NullLoggerProvider.Instance);
            serviceCollection.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
            serviceCollection.AddSingleton<ILogger>(NullLogger.Instance);
            serviceCollection.AddPetProjectConsulServices(AppSettings.Configuration, true);
            using (var provider = serviceCollection.BuildServiceProvider())
            {
                var store = provider.GetRequiredService<IStringKeyValueStore>();

                const BindingFlags BindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
                var properties = typeof(AppSettings).GetProperties(BindFlags);
                AppSettings.Current = new AppSettings();

                foreach (var prop in properties)
                {
                    prop.SetValue(AppSettings.Current, store.Get(prop.Name));
                }
            }
        }

        public static AppSettings Current { get; private set; }

        public static IConfiguration Configuration { get; private set; }

        public string KafkaBrokers { get; set; }

        public string KafkaTopicEnvironment { get; set; }

        public string CassandraContactPoints { get; set; }

        public string CassandraKeyspace { get; set; }
    }
}