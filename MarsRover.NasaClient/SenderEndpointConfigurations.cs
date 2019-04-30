using MarsRover.Persistence.EFCore.Context;
using MarsRover.Persistence.EFCore.Repositories;
using MarsRover.Rover.Handlers;
using MarsRover.Rover.Persistence;
using MarsRover.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence.Sql;
using NServiceBus.Serilog;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MarsRover.NasaClient
{
    public class SenderEndpointConfigurations
    {
        private static readonly string TransportConnectionString =
            ApplicationConfiguration.Instance.GetValue<string>("ServiceBus:TransportConnectionString");

        private static readonly string ContextDbConnectionString =
            ApplicationConfiguration.Instance.GetValue<string>("MarsRoverContext:DatabaseConnectionString");

        private static readonly string ServiceBusDbConnectionString =
            ApplicationConfiguration.Instance.GetValue<string>("ServiceBus:DatabaseConnectionString");

        public static EndpointConfiguration GetDefaultEndpointConfiguration()
        {
            LogManager.Use<SerilogFactory>();

            var endpointName =
                ApplicationConfiguration.Instance.GetValue<string>("Nasa:EndpointName");
            var errorQueueName =
                ApplicationConfiguration.Instance.GetValue<string>("Nasa:ErrorQueueName");

            var endpointConfiguration = new EndpointConfiguration(endpointName);


            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo(errorQueueName);
            endpointConfiguration.EnableDurableMessages();

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>().UseConventionalRoutingTopology();
            transport.ConnectionString(TransportConnectionString);


            #region Persistence

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            //persistence.SqlVariant(SqlVariant.MsSqlServer);
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                () => new SqlConnection(ServiceBusDbConnectionString));

            #endregion


            #region Excluded types, assemblies
            var scanner = endpointConfiguration.AssemblyScanner();

            scanner.ExcludeTypes(typeof(MoveCommandHandler));

            var roverCommands = Assembly.Load("MarsRover.Contracts").GetTypes()
                .Where(t => t.IsClass && t.Namespace == "MarsRover.Contracts.Commands").ToArray();
            scanner.ExcludeTypes(roverCommands);
            #endregion

            RegisterServiceDepencies(endpointConfiguration);

            return endpointConfiguration;
        }

        private static void RegisterServiceDepencies(EndpointConfiguration endpointConfiguration)
        {

            var roverDbContextBuilder = new DbContextOptionsBuilder<RoverContext>();
            roverDbContextBuilder.UseSqlServer(ContextDbConnectionString);

            endpointConfiguration.RegisterComponents(configureComponents =>
            {

                configureComponents.ConfigureComponent<IRoverRepository>(
                    () => new RoverRepository(
                        new RoverContext(roverDbContextBuilder.Options)),
                    DependencyLifecycle.InstancePerUnitOfWork);

                configureComponents.ConfigureComponent<IPlateauRepository>(
                    () => new PlateauRepository(
                        new RoverContext(roverDbContextBuilder.Options)),
                    DependencyLifecycle.InstancePerUnitOfWork);

            });

        }
    }
}
