using AutoMapper;
using Microsoft.Extensions.Configuration;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Shared.DonationDTOs;

namespace NetFoodia.Services.MappingProfiles
{
    public class DonationPictureUrlResolver : IValueResolver<Donation, DonationDetailsDTO, string>
    {
        private readonly IConfiguration _configuration;

        public DonationPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string Resolve(Donation source, DonationDetailsDTO destination, string destMember, ResolutionContext context)
        {

            if (string.IsNullOrEmpty(source.ImagePath))
                return string.Empty;


            if (source.ImagePath.StartsWith("http"))
                return source.ImagePath;


            var baseUrl = _configuration["URLs:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
                return source.ImagePath;


            var fullUrl = $"{baseUrl.TrimEnd('/')}/{source.ImagePath.TrimStart('/')}";

            return fullUrl;
        }
    }
}
