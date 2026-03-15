using AutoMapper;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.SharedValueObjects;
using NetFoodia.Shared.DonationDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DonationStatusDTOEnum = NetFoodia.Shared.DonationDTOs.DonationStatus;

namespace NetFoodia.Services.MappingProfiles
{
    public class DonationMappingProfile : Profile
    {
        public DonationMappingProfile()
        {
            CreateMap<CreateDonationDTO, Donation>()
                .ForMember(d => d.PickupLocation,
                    opt => opt.MapFrom(s => new GeoLocation(s.Latitude, s.Longitude)));

            CreateMap<EditDonationDTO, Donation>()
                .ForMember(d => d.PickupLocation,
                    opt => opt.MapFrom(s => new GeoLocation(s.Latitude, s.Longitude)));

            CreateMap<Donation, DonationDetailsDTO>()
                .ForMember(d => d.DonationId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CharityName, opt => opt.MapFrom(s => s.Charity.OrganizationName))
                .ForMember(d => d.DonorName, opt => opt.MapFrom(s => s.Donor.User.FullName))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.PickupLocation.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.PickupLocation.Longitude))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (DonationStatusDTOEnum)(int)s.Status));

            CreateMap<Donation, DonationListItemDTO>()
                .ForMember(d => d.DonationId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CharityName, opt => opt.MapFrom(s => s.Charity.OrganizationName))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.PickupLocation.Latitude))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.PickupLocation.Longitude))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (DonationStatusDTOEnum)(int)s.Status));

            CreateMap<Donation, PendingDonationListItemDTO>()
                .ForMember(d => d.DonationId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DonorName, opt => opt.MapFrom(s => s.Donor.User.FullName));

            CreateMap<Donation, DonationStatusDTO>()
                .ForMember(d => d.DonationId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (DonationStatusDTOEnum)(int)s.Status));
        }
    }
}