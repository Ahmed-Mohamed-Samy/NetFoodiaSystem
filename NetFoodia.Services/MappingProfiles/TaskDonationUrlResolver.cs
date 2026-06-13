using AutoMapper;
using Microsoft.Extensions.Configuration;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Services.MappingProfiles
{
    public class TaskDonationUrlResolver<TDestination> : 
        IValueResolver<PickupTask, TDestination, string?>,
        IValueResolver<AssignmentAttempt, TDestination, string?>
    {
        private readonly IConfiguration _configuration;

        public TaskDonationUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string? Resolve(PickupTask source, TDestination destination, string? destMember, ResolutionContext context)
        {
            return ResolveUrl(source.Donation);
        }

        public string? Resolve(AssignmentAttempt source, TDestination destination, string? destMember, ResolutionContext context)
        {
            return ResolveUrl(source.PickupTask?.Donation);
        }

        private string? ResolveUrl(Donation? donation)
        {
            if (donation == null || string.IsNullOrEmpty(donation.ImagePath))
                return string.Empty;

            if (donation.ImagePath.StartsWith("http"))
                return donation.ImagePath;

            var baseUrl = _configuration["URLs:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
                return donation.ImagePath;

            var fullUrl = $"{baseUrl.TrimEnd('/')}/{donation.ImagePath.TrimStart('/')}";

            return fullUrl;
        }
    }
}
