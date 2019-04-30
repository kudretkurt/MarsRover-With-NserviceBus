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

            rover.ApplyMoveCommand(message.EncryptedMoveCommand);

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
