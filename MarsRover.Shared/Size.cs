using System;
using System.Collections.Generic;

namespace MarsRover.Shared
{
    public class Size : ValueObjectBase
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Size(int width, int height)
        {
            if (width <= 0)
            {
                throw new InvalidOperationException($"{nameof(width)} should not be less or equal than 0 ");
            }

            if (height <= 0)
            {
                throw new InvalidOperationException($"{nameof(height)} should not be less or equal than 0 ");
            }

            Width = width;
            Height = height;
        }
        public static Size Empty => new Size(0, 0);

        /// <summary>
        /// Added to bypass EFCore's lack of `nullable Owned Types`. 
        /// </summary>
        /// <returns></returns>
        public static Size DefaultInstance() => new Size();

        private Size()
        {

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return new[] { Width, Height };
        }
    }
}
