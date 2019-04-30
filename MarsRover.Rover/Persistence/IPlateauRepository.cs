using System;
using System.Threading.Tasks;

namespace MarsRover.Rover.Persistence
{
    public interface IPlateauRepository
    {
        Task<int> SavePlateau(Domain.LandBase plateau);
        Domain.LandBase GetPlateau(Guid plateauId);
        Task<int> UpdatePlateau(Domain.LandBase plateau);
    }
}
