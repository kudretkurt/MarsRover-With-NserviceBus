using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Shared;

namespace MarsRover.Rover.Domain
{
    public abstract class LandBase : EntityBase
    {
        public Size Size { get; }
        public string Name { get; }
        public IReadOnlyCollection<Rover> Rovers { get; }
        protected LandBase(Size size, string name, Guid id = default)
        {
            if (id != default)
            {
                Id = id;
            }

            Size = size;
            Name = name;
            Rovers = new List<Rover>();
        }
        protected LandBase()
        {
            Rovers = new List<Rover>();
        }
        public void AddRover(Rover rover)
        {
            if (Rovers.Any(t => t.Id == rover.Id))
            {
                throw new Exception("Aynı araç tekrar gönderilemez");
            }

            if (Rovers.Any(t => t.Point.XPosition == rover.Point.XPosition && t.Point.YPosition == rover.Point.YPosition))
            {
                throw new Exception("Platoya göndermek istediğiniz aracın x ve y koordinatlarında başka bir uzay aracı bulunmaktadır.Bu yüzden lütfen ilgili aracın başlangıç koordinatlarını değiştiriniz");
            }

            Rovers.ToList().Add(rover);
        }
    }
}
