using System;
using System.Threading.Tasks;

namespace MarsRover.Rover.Persistence
{
    public interface IRoverRepository
    {
        Task<int> SaveRover(Domain.Rover rover);
        Domain.Rover GetRover(Guid roverId);
        Task<int> UpdateRover(Domain.Rover rover);
    }
}
