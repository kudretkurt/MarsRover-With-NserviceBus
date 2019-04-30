using MarsRover.Rover.CustomExceptions;
using MarsRover.Rover.Domain;
using MarsRover.Shared;
using MarsRover.Shared.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MarsRover.Tests
{
    public class RoverRepositoryTests : IClassFixture<RepositoryFixture>
    {
        private readonly RepositoryFixture _fixture;
        public RoverRepositoryTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [Trait("RoverRepositoryTests", "InsertRover")]
        public async Task Insert_Rover()
        {
            var plateauId = Guid.NewGuid();
            await _fixture.PlateauRepository.SavePlateau(new Plateau(new Size(5, 5), "PlateauX", plateauId));
            var plateau = _fixture.PlateauRepository.GetPlateau(plateauId);
            var roverId = Guid.NewGuid();

            var insertedRoverX = new RoverX(Direction.East, new Point(1, 1), roverId);
            insertedRoverX.SendToPlateau(plateau);
            await _fixture.RoverRepository.SaveRover(insertedRoverX);

            var rover = _fixture.RoverRepository.GetRover(roverId);
            plateau = _fixture.PlateauRepository.GetPlateau(plateauId);

            Assert.Equal(roverId, rover.Id);
            Assert.Equal(roverId, plateau.Rovers.First().Id);
        }

        [Fact]
        [Trait("RoverRepositoryTests", "UpdateRover")]
        public async Task Update_Rover_Should_Work_Correctly()
        {
            var roverId = Guid.NewGuid();
            var insertedRoverX = new RoverX(Direction.North, new Point(1, 1), roverId);
            var affectedRows = await _fixture.RoverRepository.SaveRover(insertedRoverX);

            var rover = _fixture.RoverRepository.GetRover(roverId);

            rover.Lock();

            Assert.True(rover.IsLocked);

            await _fixture.RoverRepository.UpdateRover(rover);

            Assert.Throws<LockException>(() => rover.Move());
        }


    }
}
