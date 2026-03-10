using AutoMapper;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Shared.MembershipDTOs;
using MembershipStatus = NetFoodia.Shared.MembershipDTOs.MembershipStatus;

namespace NetFoodia.Services.MappingProfiles
{
    public class VolunteerMembershipProfile : Profile
    {
        public VolunteerMembershipProfile()
        {
            CreateMap<VolunteerMembership, VolunteerMembershipDTO>()
                .ForMember(dest => dest.CharityName,
               opt => opt.MapFrom(src => src.Charity.OrganizationName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (MembershipStatus)(int)src.Status));

            CreateMap<VolunteerMembership, ListVolunteerMembershipDTO>()
                .ForMember(dest => dest.VolunteerName,
                opt => opt.MapFrom(src => src.Volunteer.User.FullName))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Volunteer.Address))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (MembershipStatus)(int)src.Status));
        }
    }
}
