using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MarsRover.Shared.Configuration
{
    public sealed class ApplicationConfiguration
    {
        private static readonly Lazy<ApplicationConfiguration> Lazy = new Lazy<ApplicationConfiguration>(() => new ApplicationConfiguration());
        public static ApplicationConfiguration Instance => Lazy.Value;
        private IConfiguration Configuration { get; }

        private const string ConfigFileName = "configuration.json";
        private const string ConfigFilePathEnvironmentVariable = "configuration_path";

        private readonly string _configPath;
        public string ConfigurationPath => Path.Combine(_configPath, ConfigFileName);

        private ApplicationConfiguration()
        {
            _configPath = GetConfigurationPath();


            var builder = new ConfigurationBuilder()
                .AddJsonFile(Path.Combine(_configPath, ConfigFileName), optional: false, reloadOnChange: false);


            builder = builder.Add(new DecryptConfigurationSource());
            Configuration = builder.Build();
        }

        public T GetValue<T>(string configurationKey) where T : IConvertible
        {
            T result;
            try
            {
                if (Configuration[configurationKey] == null)
                    throw new ArgumentNullException($"Configuration Value couldn't find for the given key: {configurationKey} . Configuration Location: {_configPath}");


                if (typeof(T).IsEnum)
                {
                    result = (T)Enum.Parse(typeof(T), Configuration[configurationKey]);
                }
                else
                {
                    result = (T)Convert.ChangeType(Configuration[configurationKey], typeof(T));
                }


                if (string.IsNullOrEmpty(Convert.ToString(result, CultureInfo.InvariantCulture)))
                    throw new ArgumentNullException(nameof(configurationKey));
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException("Couldn't convert");
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        public IConfigurationSection GetSection(string configurationKey)
        {
            return Configuration.GetSection(configurationKey);
        }

        private string GetConfigurationPath()
        {
            // expecting the path of the configuration file in the Machine level environment variable 
            var path = Environment.GetEnvironmentVariable(ConfigFilePathEnvironmentVariable, EnvironmentVariableTarget.Machine);

            if (string.IsNullOrEmpty(path))
            {
                var tmpPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\MarsRover.Shared");

                if (File.Exists(Path.Combine(tmpPath, ConfigFileName)))
                {
                    path = tmpPath;
                   
                }
            }

            return path;
        }
    }
}
