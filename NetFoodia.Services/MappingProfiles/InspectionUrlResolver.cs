using AutoMapper;
using Microsoft.Extensions.Configuration;
using NetFoodia.Domain.Entities.InspectionModule;
using NetFoodia.Shared.InspectionDTOs;

namespace NetFoodia.Services.MappingProfiles
{
    public class InspectionUrlResolver : IValueResolver<FoodInspection, FoodInspectionDTO, string>
    {
        private readonly IConfiguration _configuration;

        public InspectionUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(FoodInspection source, FoodInspectionDTO destination, string destMember, ResolutionContext context)
        {

            if (source.Donation == null || string.IsNullOrEmpty(source.Donation.ImagePath))
                return string.Empty;

            if (source.Donation.ImagePath.StartsWith("http"))
                return source.Donation.ImagePath;

            var baseUrl = _configuration["URLs:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
                return source.Donation.ImagePath;


            var fullUrl = $"{baseUrl.TrimEnd('/')}/{source.Donation.ImagePath.TrimStart('/')}";

            return fullUrl;
        }
    }
}
