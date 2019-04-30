using MarsRover.Shared;
using System;
using System.Collections.Generic;

namespace MarsRover.Rover.Domain
{
    public class Plateau : LandBase
    {
        public Plateau(Size size, string name, Guid id = default) : base(size, name, id)
        {
        }

        public Plateau()
        {
            
        }
    }
}
