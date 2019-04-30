using MarsRover.Shared;
using MarsRover.Shared.Enums;
using System;

namespace MarsRover.RoverPersistence.Entities
{
    public class Rover
    {
        public Guid Id { get; set; }
        public Direction Direction { get; set; }
        public Point Point { get; set; }
        public Plateau Plateau { get; set; }
        public bool IsLocked { get; set; }
    }
}
