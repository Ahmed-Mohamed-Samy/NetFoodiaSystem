using AutoMapper;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Domain.Entities.SharedValueObjects;
using NetFoodia.Shared.ProfileDTOs;
using DomainVehicleType = NetFoodia.Domain.Entities.ProfileModule.VehicleType;
using SharedVehicleType = NetFoodia.Shared.ProfileDTOs.VehicleType;

namespace NetFoodia.Services.MappingProfiles
{
    public class MyProfileProfile : Profile
    {
        public MyProfileProfile()
        {
            CreateMap<GeoLocation, GeoLocationDTO>().ReverseMap();
            CreateMap<DonorProfile, DonorProfileDTO>();
            CreateMap<DonorProfile, DonorDto>()
                     .ForMember(d => d.FullName, opt => opt.MapFrom(s => s.User != null ? s.User.FullName : string.Empty))
                     .ForMember(d => d.Email, opt => opt.MapFrom(s => s.User != null ? s.User.Email : string.Empty));
            CreateMap<VolunteerProfile, VolunteerProfileDTO>()
                     .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status.ToString()))
                     .ForMember(d => d.VehicleType, opt => opt.MapFrom(
                         s => s.VehicleType.HasValue
                             ? (SharedVehicleType?)(int)s.VehicleType.Value
                             : (SharedVehicleType?)null));
            CreateMap<CharityAdminProfile, CharityAdminProfileDTO>();
            CreateMap<ApplicationUser, UserInfoDTO>()
                     .ForMember(d => d.UserId, opt => opt.MapFrom(s => s.Id));

            CreateMap<DonorProfile, MyProfileDTO>()
                     .ForMember(d => d.User, opt => opt.MapFrom(s => s.User))
                     .ForMember(d => d.Role, opt => opt.MapFrom(_ => "Donor"))
                     .ForMember(d => d.Donor, opt => opt.MapFrom(s => s))
                     .ForMember(d => d.Volunteer, opt => opt.Ignore())
                     .ForMember(d => d.CharityAdmin, opt => opt.Ignore());

            CreateMap<VolunteerProfile, MyProfileDTO>()
                    .ForMember(d => d.User, opt => opt.MapFrom(s => s.User))
                    .ForMember(d => d.Role, opt => opt.MapFrom(_ => "Volunteer"))
                    .ForMember(d => d.Volunteer, opt => opt.MapFrom(s => s))
                    .ForMember(d => d.Donor, opt => opt.Ignore())
                    .ForMember(d => d.CharityAdmin, opt => opt.Ignore());

            CreateMap<CharityAdminProfile, MyProfileDTO>()
                    .ForMember(d => d.User, opt => opt.MapFrom(s => s.User))
                    .ForMember(d => d.Role, opt => opt.MapFrom(_ => "CharityAdmin"))
                    .ForMember(d => d.CharityAdmin, opt => opt.MapFrom(s => s))
                    .ForMember(d => d.Donor, opt => opt.Ignore())
                    .ForMember(d => d.Volunteer, opt => opt.Ignore());

            CreateMap<UpsertDonorProfileDTO, DonorProfile>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.IsVerified, opt => opt.Ignore())
                    .ForMember(d => d.ReliabilityScore, opt => opt.Ignore())
                    .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                    .ForAllMembers(opt =>
                        opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpsertVolunteerProfileDTO, VolunteerProfile>()
                    .ForMember(d => d.Id, opt => opt.Ignore())
                    .ForMember(d => d.IsVerified, opt => opt.Ignore())
                    .ForMember(d => d.UserId, opt => opt.Ignore())
                    .ForMember(d => d.User, opt => opt.Ignore())
                    .ForMember(d => d.Status, opt => opt.Ignore())
                    .ForMember(d => d.LastActiveAt, opt => opt.Ignore())
                    .ForMember(d => d.CreatedAt, opt => opt.Ignore())
                    .ForMember(d => d.VehicleType, opt => opt.MapFrom(
                        s => (DomainVehicleType?)(int)s.VehicleType))
                    .ForAllMembers(opt =>
                        opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
