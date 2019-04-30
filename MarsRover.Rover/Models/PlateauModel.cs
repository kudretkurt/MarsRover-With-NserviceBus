using System;
using MarsRover.Shared;
using System.Collections.Generic;
using System.Linq;

namespace MarsRover.Rover.Models
{
    public class PlateauModel
    {
        public IReadOnlyDictionary<Guid, KeyValuePair<int, int>> Rovers { get; private set; }
        public Size Size { get; private set; }
        public Guid PlateauId { get; private set; }
        public string PlateauName { get; private set; }

        public static PlateauModel CreateNew(IReadOnlyDictionary<Guid, KeyValuePair<int, int>> rovers, Size size, Guid plateauId, string plateauName)
        {
            return new PlateauModel()
            {
                PlateauId = plateauId,
                PlateauName = plateauName,
                Size = size,
                Rovers = rovers
            };
        }

        public bool IsValidPoint(Point point)
        {
            var isValidX = point.XPosition >= 0 && point.XPosition <= Size.Width;
            var isValidY = point.YPosition >= 0 && point.YPosition <= Size.Height;


            if (Rovers.Count(t => t.Value.Key == point.XPosition && t.Value.Value == point.YPosition) > 1)
            {
                return false;
                //throw new Exception("Aracın hareket etmek istediği noktada başka bir uzay aracı bulunmaktadır.Bu sebepten hareket etmemelidir.");
            }

            return isValidX && isValidY;
        }
    }
}
