using AutoMapper;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.SharedValueObjects;
using NetFoodia.Shared.CharityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NetFoodia.Services.MappingProfiles
{
    public class CharityMappingProfile : Profile
    {
        public CharityMappingProfile()
        {

            CreateMap<CreateMyCharityDTO, Charity>();


            CreateMap<UpdateCharityInfoDTO, Charity>();


            CreateMap<UpdateCharityLocationDTO, Charity>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));


            CreateMap<Charity, CharityDetailsDTO>()
                .ForMember(d => d.CharityId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.MembershipStatus,
                    opt => opt.MapFrom(s => (NetFoodia.Shared.CharityDTOs.CharityMembershipStatus)s.MembershipStatus));

            CreateMap<Charity, CharityListItemDTO>()
                .ForMember(d => d.CharityId, opt => opt.MapFrom(s => s.Id));

            CreateMap<CreateMyCharityDTO, Charity>()
               .ForMember(d => d.Location,
                   opt => opt.MapFrom(s => new GeoLocation(s.Latitude, s.Longitude)));

            CreateMap<UpdateCharityInfoDTO, Charity>();

            CreateMap<UpdateCharityLocationDTO, Charity>()
                .ForMember(d => d.Location, opt =>
                    opt.MapFrom((src, dest) =>
                      new GeoLocation(
                            src.Latitude ?? dest.Location.Latitude,
                            src.Longitude ?? dest.Location.Longitude
                      )));



            CreateMap<Charity, CharityDetailsDTO>()
                .ForMember(d => d.CharityId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Location.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Location.Longitude))
                .ForMember(d => d.MembershipStatus,
                    opt => opt.MapFrom(s => (NetFoodia.Shared.CharityDTOs.CharityMembershipStatus)s.MembershipStatus));

            CreateMap<Charity, CharityListItemDTO>()
                .ForMember(d => d.CharityId, opt => opt.MapFrom(s => s.Id));
        }
    }
}
