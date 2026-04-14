using AutoMapper;
using NetFoodia.Domain.Entities.InspectionModule;
using NetFoodia.Shared.InspectionDTOs;

namespace NetFoodia.Services.MappingProfiles
{
    public class InspectionProfile : Profile
    {
        public InspectionProfile()
        {
            CreateMap<FoodInspection, FoodInspectionDTO>()
            .ForMember(dest => dest.SafetyStatus, opt => opt.MapFrom(src => src.SafetyStatus.ToString()))
            .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.ToString()))
            .ForMember(d => d.FoodType, o => o.MapFrom(s => s.Donation.FoodType))
            .ForMember(d => d.ImageUrl, o => o.MapFrom<InspectionUrlResolver>());
        }
    }
}
