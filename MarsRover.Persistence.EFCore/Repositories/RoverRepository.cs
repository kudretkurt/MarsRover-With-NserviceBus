using AutoMapper;
using MarsRover.Persistence.EFCore.Context;
using MarsRover.Rover.Persistence;
using MarsRover.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DomainRover = MarsRover.Rover.Domain.RoverX;
using PersistenceRover = MarsRover.Persistence.EFCore.Entities.Rover;

namespace MarsRover.Persistence.EFCore.Repositories
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
            var persistenceRover = _context.Rovers.Include(t => t.Plateau).FirstOrDefault(t => t.Id == roverId);
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
            var persistenceRover = _context.Rovers.Include(t => t.Plateau).First(t => t.Id == rover.Id);
            persistenceRover.Point = rover.Point;
            persistenceRover.Direction = rover.Direction;
            persistenceRover.PlateauId = rover.PlateauId;
            persistenceRover.IsLocked = rover.IsLocked;
            return await _context.SaveChangesAsync();
        }
    }
}
