using AutoMapper;
using Racen.Backend.App.DTOs;
using Racen.Backend.App.DTOs.MotorcycleDTOs;
using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Models.User;
namespace Racen.Backend.App.Mappings
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<MotorcycleUpdateDto, Motorcycle>();
            CreateMap<Motorcycle, MotorcycleReadDto>();
            CreateMap<Items, ItemReadDto>();

            CreateMap<MotorcycleCreateDto, Motorcycle>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => new ApplicationUser { Id = src.OwnerId }));

            
        }
    }
}