using AutoMapper;
using MarsRover.Persistence.EFCore.Context;
using MarsRover.Rover.Domain;
using MarsRover.Rover.Persistence;
using MarsRover.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using PersistencePlateau = MarsRover.Persistence.EFCore.Entities.Plateau;

namespace MarsRover.Persistence.EFCore.Repositories
{
    public class PlateauRepository : IPlateauRepository
    {
        private readonly RoverContext _context;
        public PlateauRepository(RoverContext context)
        {
            _context = context;
        }
        public async Task<int> SavePlateau(LandBase plateau)
        {
            var persistencePlateau = Mapper.Map<PersistencePlateau>(plateau);
            _context.Plateaus.Add(persistencePlateau);
            return await _context.SaveChangesAsync();
        }

        public LandBase GetPlateau(Guid plateauId)
        {
            var persistencePlateau = _context.Plateaus.Include(t => t.Rovers).First(t => t.Id == plateauId);
            return Mapper.Map<Plateau>(persistencePlateau);
        }

        public async Task<int> UpdatePlateau(LandBase plateau)
        {
            var persistencePlateau = _context.Plateaus.Include(t => t.Rovers).First(t => t.Id == plateau.Id);
            persistencePlateau.Size = new Size(plateau.Size.Width, plateau.Size.Height);

            return await _context.SaveChangesAsync();
        }
    }
}
