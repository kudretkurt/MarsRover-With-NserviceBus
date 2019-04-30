using MarsRover.Rover.CustomExceptions;
using MarsRover.Shared;
using MarsRover.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using MarsRover.Rover.Models;

namespace MarsRover.Rover.Domain
{
    public abstract class Rover : EntityBase
    {
        public Direction Direction { get; private set; }
        public Point Point { get; private set; }
        public PlateauModel Plateau { get; private set; }
        public Guid PlateauId { get; private set; }
        public bool IsLocked { get; private set; }
        protected Rover(Direction direction, Point point, Guid roverId)
        {
            IsLocked = false;

            if (roverId == Guid.Empty)
            {
                throw new InvalidOperationException($"{nameof(roverId)}");
            }

            if (direction == default)
            {
                throw new InvalidOperationException($"{nameof(direction)}");
            }



            Id = roverId;
            Direction = direction;
            Point = point;
        }
        protected Rover()
        {

        }
        public abstract void TurnLeft();
        public abstract void TurnRight();
        public abstract void Move();
        public void SetDirection(Direction direction)
        {
            Direction = direction;
        }
        protected internal void SetPositions(Point point)
        {
            if (Plateau != null && Plateau.IsValidPoint(point))
            {
                Point = point;
                return;
            }

            throw new InvalidPositionException();
        }
        public void SendToPlateau(LandBase plateau)
        {
            var readonlyDictionary = plateau.Rovers.ToDictionary(rover => rover.Id, rover => new KeyValuePair<int, int>(rover.Point.XPosition, rover.Point.YPosition));

            Plateau = PlateauModel.CreateNew(readonlyDictionary, plateau.Size, plateau.Id, plateau.Name);
            PlateauId = Plateau.PlateauId;

            if (!Plateau.IsValidPoint(Point))
            {
                throw new InvalidPositionException();
            }
        }
        public void Lock()
        {
            IsLocked = true;
        }
        public void UnLock()
        {
            IsLocked = false;
        }
    }
}
