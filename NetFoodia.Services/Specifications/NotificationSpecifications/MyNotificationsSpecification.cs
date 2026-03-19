using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NetFoodia.Domain.Entities.NotificationModule;

namespace NetFoodia.Services.Specifications.NotificationSpecifications
{
    public class MyNotificationsSpecification : BaseSpecification<Notification>
    {
        public MyNotificationsSpecification(string userId)
            : base(n => n.UserId == userId)
        {
            AddOrderByDesc(n => n.CreatedAt);
        }
    }
}
