using MarsRover.Rover.CustomExceptions;
using MarsRover.Shared;
using MarsRover.Shared.Enums;
using System;

namespace MarsRover.Rover.Domain
{
    public class RoverX : Rover
    {

        public RoverX(Direction direction, Point point, Guid roverId) : base(direction, point, roverId)
        {
        }

        public RoverX()
        {

        }
        public override void TurnLeft()
        {
            if (IsLocked)
            {
                throw new LockException();
            }

            var expectedDirection = default(Direction);

            if (Direction == Direction.West)
            {
                expectedDirection = Direction.South;
            }
            else if (Direction == Direction.South)
            {
                expectedDirection = Direction.East;
            }
            else if (Direction == Direction.East)
            {
                expectedDirection = Direction.North;
            }
            else if (Direction == Direction.North)
            {
                expectedDirection = Direction.West;
            }

            SetDirection(expectedDirection);
        }
        public override void TurnRight()
        {
            if (IsLocked)
            {
                throw new LockException();
            }

            var expectedDirection = default(Direction);

            if (Direction == Direction.West)
            {
                expectedDirection = Direction.North;
            }
            else if (Direction == Direction.South)
            {
                expectedDirection = Direction.West;
            }
            else if (Direction == Direction.East)
            {
                expectedDirection = Direction.South;
            }
            else if (Direction == Direction.North)
            {
                expectedDirection = Direction.East;
            }

            SetDirection(expectedDirection);
        }
        public override void Move()
        {
            if (IsLocked)
            {
                throw new LockException();
            }

            switch (Direction)
            {
                case Direction.East:
                    SetPositions(new Point(Point.XPosition + 1, Point.YPosition));
                    break;
                case Direction.West:
                    SetPositions(new Point(Point.XPosition - 1, Point.YPosition));
                    break;
                case Direction.North:
                    SetPositions(new Point(Point.XPosition, Point.YPosition + 1));
                    break;
                case Direction.South:
                    SetPositions(new Point(Point.XPosition, Point.YPosition - 1));
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
