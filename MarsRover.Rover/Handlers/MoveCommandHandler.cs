using MarsRover.Contracts.Commands;
using MarsRover.Rover.Persistence;
using MarsRover.Shared.Utilities;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace MarsRover.Rover.Handlers
{
    public class MoveCommandHandler : IHandleMessages<MoveCommand>, IHandleMessages<EmergencyCall>
    {
        public IRoverRepository RoverRepository { get; set; }
        public IPlateauRepository PlateauRepository { get; set; }
        public Task Handle(MoveCommand message, IMessageHandlerContext context)
        {
            var rover = RoverRepository.GetRover(
                Guid.Parse(EncryptionUtils.Instance.Decrypt(message.EncryptedRoverId)));

            var moveCommands = EncryptionUtils.Instance.Decrypt(message.EncryptedMoveCommand).ToCharArray();

            foreach (var moveCommand in moveCommands)
            {
                switch (moveCommand)
                {
                    case 'L':
                        rover.TurnLeft();
                        break;
                    case 'R':
                        rover.TurnRight();
                        break;
                    case 'M':
                        rover.Move();
                        break;
                    default:
                        throw new Exception("UnExpected command");
                }
            }

            RoverRepository.UpdateRover(rover);

            return Task.CompletedTask;
        }

        public Task Handle(EmergencyCall message, IMessageHandlerContext context)
        {
            var rover = RoverRepository.GetRover(
                Guid.Parse(EncryptionUtils.Instance.Decrypt(message.EncryptedRoverId)));

            rover.Lock();

            RoverRepository.UpdateRover(rover);

            return Task.CompletedTask;
        }
    }
}
