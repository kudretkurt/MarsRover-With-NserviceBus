using NServiceBus;
using RabbitMQ.Client.Exceptions;
using System;
using System.Threading.Tasks;
using AutoMapper;
using MarsRover.Contracts.Commands;
using MarsRover.Persistence.EFCore;
using MarsRover.Persistence.EFCore.Context;
using MarsRover.Persistence.EFCore.Repositories;
using MarsRover.Rover.Domain;
using MarsRover.Rover.Persistence;
using MarsRover.Shared;
using MarsRover.Shared.Configuration;
using MarsRover.Shared.Enums;
using MarsRover.Shared.Utilities;
using Microsoft.EntityFrameworkCore;

namespace MarsRover.NasaClient
{
    public class Program
    {
        private static IEndpointInstance _nasaEndpointInstance;

        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            Console.Title = "NasaClient Console ";
            try
            {
                _nasaEndpointInstance = await Endpoint.Start(SenderEndpointConfigurations.GetDefaultEndpointConfiguration()).ConfigureAwait(false);

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
                Console.WriteLine("NasaClient running...");

                await InitializeTest();
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
                if (_nasaEndpointInstance != null)
                {
                    await _nasaEndpointInstance.Stop();
                }
            }
        }

        public static async Task InitializeTest()
        {
            var contextDbConnectionString =
                ApplicationConfiguration.Instance.GetValue<string>("MarsRoverContext:DatabaseConnectionString");

            var roverDbContextBuilder = new DbContextOptionsBuilder<RoverContext>();
            roverDbContextBuilder.UseSqlServer(contextDbConnectionString);

            var roverRepository = new RoverRepository(
                    new RoverContext(roverDbContextBuilder.Options));

            var plateauRepository = new PlateauRepository(
                new RoverContext(roverDbContextBuilder.Options));

            //Test Input:
            //5 5
            //1 2 N
            //LMLMLMLMM
            //3 3 E
            //MMRMMRMRRM
            //Expected Output:
            //1 3 N
            //5 1 E

            //PS:kudretkurt
            //Plato yaratılır.Rover yaratılır. Yaratılan rover ilgili platoya gönderilir(SendToPlateau). Daha sonra bu uzay aracının nasıl hareket edeceği bilgisi şifrelenerek (koskoca nasa mesajı şifreler muhtemelen:)) ilgili queue ya consume edilmesi için gönderilir.

            var plateauId = Guid.NewGuid();
            var plateau = new Plateau(new Size(5, 5), "FirstPlateau", plateauId);
            await plateauRepository.SavePlateau(plateau);

            var roverId = Guid.NewGuid();
            var rover = new RoverX(Direction.North, new Point(1, 2), roverId);
            rover.SendToPlateau(plateau);
            await roverRepository.SaveRover(rover);


            var sendOptions = new SendOptions();
            sendOptions.SetDestination(ApplicationConfiguration.Instance.GetValue<string>("MarsRoverContext:EndpointName"));

            //await _nasaEndpointInstance.Send(new MoveCommand()
            //{
            //    EncryptedMoveCommand = EncryptionUtils.Instance.Encrypt("LMLMLMLMM"),
            //    EncryptedRoverId = EncryptionUtils.Instance.Encrypt(roverId.ToString())
            //}, sendOptions);


            roverId = Guid.NewGuid();
            rover = new RoverX(Direction.East, new Point(3, 3), roverId);
            rover.SendToPlateau(plateau);
            await roverRepository.SaveRover(rover);

            sendOptions = new SendOptions();
            sendOptions.SetDestination(ApplicationConfiguration.Instance.GetValue<string>("MarsRoverContext:EndpointName"));

            await _nasaEndpointInstance.Send(new MoveCommand()
            {
                EncryptedMoveCommand = EncryptionUtils.Instance.Encrypt("MMRMMRMRRM"),
                EncryptedRoverId = EncryptionUtils.Instance.Encrypt(roverId.ToString())
            }, sendOptions);

        }
    }
}
