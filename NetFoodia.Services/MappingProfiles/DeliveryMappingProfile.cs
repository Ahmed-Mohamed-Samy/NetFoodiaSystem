using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Shared.DeliveryDTOs;
using TaskStatusDTO = NetFoodia.Shared.DeliveryDTOs.TaskStatus;
using AttemptResponseDTO = NetFoodia.Shared.DeliveryDTOs.AttemptResponse;

namespace NetFoodia.Services.MappingProfiles
{
    public class DeliveryMappingProfile : Profile
    {
        public DeliveryMappingProfile()
        {
            CreateMap<PickupTask, PickupTaskDetailsDTO>()
                .ForMember(d => d.TaskId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DonationTitle, opt => opt.MapFrom(s => s.Donation.FoodType))
                .ForMember(d => d.CharityName, opt => opt.MapFrom(s => s.Charity.OrganizationName))
                .ForMember(d => d.AssignedVolunteerName,
                    opt => opt.MapFrom(s => s.AssignedVolunteer != null ? s.AssignedVolunteer.User.FullName : null))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (TaskStatusDTO)(int)s.Status));

            CreateMap<PickupTask, OpenTaskListItemDTO>()
                .ForMember(d => d.TaskId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DonationTitle, opt => opt.MapFrom(s => s.Donation.FoodType));

            CreateMap<AssignmentAttempt, VolunteerOfferDTO>()
                .ForMember(d => d.TaskId, opt => opt.MapFrom(s => s.PickupTaskId))
                .ForMember(d => d.DonationId, opt => opt.MapFrom(s => s.PickupTask.DonationId))
                .ForMember(d => d.DonationTitle, opt => opt.MapFrom(s => s.PickupTask.Donation.FoodType))
                .ForMember(d => d.CharityName, opt => opt.MapFrom(s => s.PickupTask.Charity.OrganizationName))
                .ForMember(d => d.SlaDueAt, opt => opt.MapFrom(s => s.PickupTask.SlaDueAt))
                .ForMember(d => d.Response, opt => opt.MapFrom(s => (AttemptResponseDTO)(int)s.Response));

            CreateMap<PickupTask, MyTaskHistoryDTO>()
                .ForMember(d => d.TaskId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.DonationTitle, opt => opt.MapFrom(s => s.Donation.FoodType))
                .ForMember(d => d.CharityName, opt => opt.MapFrom(s => s.Charity.OrganizationName))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => (TaskStatusDTO)(int)s.Status));
        }
    }
}