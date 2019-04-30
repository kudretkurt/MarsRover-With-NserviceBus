using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MarsRover.Rover.Models;
using DomainPlateau = MarsRover.Rover.Domain.Plateau;
using DomainRover = MarsRover.Rover.Domain.RoverX;
using PersistencePlateau = MarsRover.Persistence.EFCore.Entities.Plateau;
using PersistenceRover = MarsRover.Persistence.EFCore.Entities.Rover;

namespace MarsRover.Persistence.EFCore
{
    public class RoverMapperProfile : Profile
    {
        public RoverMapperProfile()
        {
            //CreateMap<DomainPlateau, PlateauModel>().ForMember(t => t,
            //    opt => opt.MapFrom(t => PlateauModel.CreateNew(t.Rovers.ToDictionary(rover => rover.Id, rover => new KeyValuePair<int, int>(rover.Point.XPosition, rover.Point.YPosition)), t.Size, t.Id, t.Name)));

            //CreateMap<PlateauModel, DomainPlateau>().ForMember(t => t,
            //    opt => opt.MapFrom(t => new DomainPlateau(t.Size, t.PlateauName, t.PlateauId)));

            CreateMap<DomainPlateau, PersistencePlateau>();

            CreateMap<DomainRover, PersistenceRover>().ForMember(t => t.Plateau,
                    opt => opt.Ignore())
                .ForMember(t => t.PlateauId, opt => opt.MapFrom(t => t.PlateauId));

            //CreateMap<DomainRover, PersistenceRover>()
            //    .ForMember(t => t.PlateauId, opt => opt.MapFrom(t => t.PlateauId));

            //CreateMap<DomainRover, PersistenceRover>();

            //CreateMap<PersistenceRover, DomainRover>().ForMember(t => t.Plateau, opt => opt.MapFrom(t => Mapper.Map<PersistencePlateau, DomainPlateau>(t.Plateau)));

            CreateMap<PersistenceRover, DomainRover>().ForMember(t => t.Plateau,
                opt => opt.MapFrom(t => PlateauModel.CreateNew(t.Plateau.Rovers.ToDictionary(rover => rover.Id, rover => new KeyValuePair<int, int>(rover.Point.XPosition, rover.Point.YPosition)), t.Plateau.Size, t.Plateau.Id, t.Plateau.Name)));

            CreateMap<PersistencePlateau, DomainPlateau>().ForMember(t => t.Rovers, opt => opt.MapFrom(t => t.Rovers.Select(Mapper.Map<PersistenceRover, DomainRover>)));

            //CreateMap<PersistencePlateau, DomainPlateau>();


        }
    }
}
