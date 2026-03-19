using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.NotificationModule;

namespace NetFoodia.Services.Specifications.NotificationSpecifications
{
    public class NotificationByUserSpecification : BaseSpecification<Notification>
    {
        public NotificationByUserSpecification(string userId, int notificationId)
            : base(n => n.UserId == userId && n.Id == notificationId)
        {
        }
    }
}
