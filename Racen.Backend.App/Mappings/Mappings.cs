using AutoMapper;
using Racen.Backend.App.DTOs;
using Racen.Backend.App.DTOs.Motorcycle;
using Racen.Backend.App.Models.MotorcycleRelated;
namespace Racen.Backend.App.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<MotorcycleCreateDto, Motorcycle>();
            CreateMap<MotorcycleUpdateDto, Motorcycle>();
            CreateMap<Motorcycle, MotorcycleReadDto>();
            CreateMap<Items, ItemReadDto>();
        }
    }
}