using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using NetFoodia.Domain.Entities.NotificationModule;
using NetFoodia.Shared.NotificationDTOs;
using NotificationTypeDTO = NetFoodia.Shared.NotificationDTOs.NotificationType;

namespace NetFoodia.Services.MappingProfiles
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationDTO>()
                .ForMember(d => d.NotificationId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Type, opt => opt.MapFrom(s => (NotificationTypeDTO)(int)s.Type));
        }
    }
}