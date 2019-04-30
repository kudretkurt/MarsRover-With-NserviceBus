using System;
using System.Collections.Generic;
using MarsRover.Shared;

namespace MarsRover.Persistence.EFCore.Entities
{
    public class Plateau
    {
        public Guid Id { get; set; }
        public Size Size { get; set; }
        public string Name { get; set; }
        public ICollection<Rover> Rovers { get; set; }

        public Plateau()
        {
            Rovers = new HashSet<Rover>();
        }
    }
}
