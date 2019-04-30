using AutoMapper;
using MarsRover.Persistence.EFCore;
using MarsRover.Persistence.EFCore.Context;
using MarsRover.Persistence.EFCore.Repositories;
using MarsRover.Rover.Persistence;
using MarsRover.Shared.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarsRover.Tests
{
    public class RepositoryFixture
    {
        private readonly ServiceProvider _serviceProvider;


        public RepositoryFixture()
        {
            var connectionString =
                ApplicationConfiguration.Instance.GetValue<string>("MarsRoverContext:DatabaseConnectionString");

            var sc = new ServiceCollection();

            #region DbContexts
            sc.AddDbContext<RoverContext>(options =>
            //enable this if you want an in-memoryDB
            options.UseInMemoryDatabase(databaseName: "inmemoryDBInstance").ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))

            //enable this if you want a concrete DB
            //options.UseSqlServer(connectionString,
            //  sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(5)), ServiceLifetime.Transient
            );


            #endregion

            #region Repositories

            sc.AddScoped<IRoverRepository>(provider => new RoverRepository(provider.GetRequiredService<RoverContext>()));

            sc.AddScoped<IPlateauRepository>(provider => new PlateauRepository(provider.GetRequiredService<RoverContext>()));





            #endregion

            _serviceProvider = sc.BuildServiceProvider();

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
        }

        public IRoverRepository RoverRepository => _serviceProvider.GetRequiredService<IRoverRepository>();
        public IPlateauRepository PlateauRepository => _serviceProvider.GetRequiredService<IPlateauRepository>();



    }
}
