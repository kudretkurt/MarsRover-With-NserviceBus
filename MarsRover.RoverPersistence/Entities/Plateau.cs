using MarsRover.Shared;
using System;
using System.Collections.Generic;

namespace MarsRover.RoverPersistence.Entities
{
    public class Plateau
    {
        public Guid Id { get; set; }
        public Size Size { get; set; }
        public string Name { get; set; }
        public ICollection<Rover> Rovers { get; set; }
    }
}
