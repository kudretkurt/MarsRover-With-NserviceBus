using AutoMapper;
using MarsRover.Persistence.EFCore;
using NServiceBus;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading.Tasks;

namespace MarsRover.RoverConsoleHost
{
    public class Program
    {
        private static IEndpointInstance _marsRoverContextEndpointInstance;

        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            Console.Title = "MarsRover.Rover Console Host";
            try
            {
                _marsRoverContextEndpointInstance = await Endpoint.Start(ReceiverEndpointConfigurations.GetDefaultEndpointConfiguration()).ConfigureAwait(false);


                try
                {
                    Mapper.AssertConfigurationIsValid();
                }
                catch (Exception)
                {
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddProfile<RoverMapperProfile>();
                    });
                }


                Console.WriteLine("Press Ctrl+C to shut down");
                Console.WriteLine("RoverHost running...");
                while (true)
                {
                    Console.Read();
                }
            }
            catch (BrokerUnreachableException e)
            {
                Console.WriteLine(string.Join(" ", e.Source, e.Message));
                Console.Read();

            }
            catch (Exception e)
            {
                Console.WriteLine(string.Join(" ", e.Source, e.Message));
                Console.Read();
            }
            finally
            {
                if (_marsRoverContextEndpointInstance != null)
                {
                    await _marsRoverContextEndpointInstance.Stop();
                }
            }
        }
    }
}
