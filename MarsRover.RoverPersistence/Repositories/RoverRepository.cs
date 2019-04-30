using AutoMapper;
using MarsRover.Rover.Persistence;
using MarsRover.RoverPersistence.Context;
using MarsRover.Shared;
using System;
using System.Linq;
using System.Threading.Tasks;
using DomainRover = MarsRover.Rover.Domain.Rover;
using PersistenceRover = MarsRover.RoverPersistence.Entities.Rover;

namespace MarsRover.RoverPersistence.Repositories
{
    public class RoverRepository : IRoverRepository
    {
        private readonly RoverContext _context;
        public RoverRepository(RoverContext context)
        {
            _context = context;
        }
        public Rover.Domain.Rover GetRover(Guid roverId)
        {
            var persistenceRover = _context.Rovers.First(t => t.Id == roverId);
            return Mapper.Map<DomainRover>(persistenceRover);
        }

        public async Task<int> SaveRover(Rover.Domain.Rover rover)
        {
            var persistenceRover = Mapper.Map<PersistenceRover>(rover);
            _context.Rovers.Add(persistenceRover);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRover(Rover.Domain.Rover rover)
        {
            var persistenceRover = _context.Rovers.First(t => t.Id == rover.Id);
            persistenceRover.Point = new Point(rover.Point.XPosition, rover.Point.YPosition);
            return await _context.SaveChangesAsync();
        }
    }
}
