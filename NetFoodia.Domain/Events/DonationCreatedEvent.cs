using MediatR;
using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Domain.Events
{
    public class DonationCreatedEvent : INotification
    {
        public Donation Donation { get; }
        public DonationCreatedEvent(Donation donation) => Donation = donation;
    }
}
