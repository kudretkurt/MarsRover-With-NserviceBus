using AutoMapper;
using MarsRover.Shared;
using DomainPlateau = MarsRover.Rover.Domain.LandBase;
using DomainRover = MarsRover.Rover.Domain.Rover;
using PersistencePlateau = MarsRover.RoverPersistence.Entities.Plateau;
using PersistenceRover = MarsRover.RoverPersistence.Entities.Rover;
namespace MarsRover.RoverPersistence
{
    public class RoverMapperProfile : Profile
    {
        public RoverMapperProfile()
        {
            CreateMap<PersistencePlateau, DomainPlateau>()
                .ForMember(t => t.Size, opt => opt.MapFrom(t => new Size(t.Size.Width, t.Size.Height)));

            CreateMap<DomainPlateau, PersistencePlateau>()
                .ForMember(t => t.Size, opt => opt.MapFrom(t => new Size(t.Size.Width, t.Size.Height)));

            CreateMap<PersistenceRover, DomainRover>()
                .ForMember(t => t.Point, opt => opt.MapFrom(t => new Point(t.Point.XPosition, t.Point.YPosition)));

            CreateMap<DomainRover, PersistenceRover>()
                .ForMember(t => t.Point, opt => opt.MapFrom(t => new Point(t.Point.XPosition, t.Point.YPosition)));
        }

    }
}
