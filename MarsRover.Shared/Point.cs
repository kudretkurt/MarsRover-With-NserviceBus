using System;
using System.Collections.Generic;

namespace MarsRover.Shared
{
    public class Point : ValueObjectBase
    {
        public int XPosition { get; private set; }
        public int YPosition { get; private set; }

        public Point(int xPosition, int yPosition)
        {

            if (xPosition < 0)
            {
                throw new InvalidOperationException($"{nameof(xPosition)} should not be less than 0 ");
            }

            if (yPosition < 0)
            {
                throw new InvalidOperationException($"{nameof(yPosition)} should not be less than 0 ");
            }

            XPosition = xPosition;
            YPosition = yPosition;
        }
        public static Point Empty => new Point(0, 0);

        // <summary>
        /// Added to bypass EFCore's lack of `nullable Owned Types`. 
        /// </summary>
        /// <returns></returns>
        public static Point DefaultInstance() => new Point();

        private Point()
        {

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return new[] { XPosition, YPosition };
        }
    }
}
