using NetFoodia.Domain.Contracts;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Services.Specifications.CharitySpecifications;
using NetFoodia.Services.Specifications.DonationSpecifications;
using NetFoodia.Services.Specifications.MembershipSpecifications;
using NetFoodia.Services_Abstraction;
using NetFoodia.Shared.CommonResult;
using NetFoodia.Shared.HomeDTOs;
using System.Threading.Tasks;

namespace NetFoodia.Services
{
    public class HomeStatisticsService : IHomeStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeStatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<HomeStatisticsDto>> GetImpactStatisticsAsync()
        {
            var charityRepo = _unitOfWork.GetRepository<Charity>();
            var donationRepo = _unitOfWork.GetRepository<Donation>();
            var membershipRepo = _unitOfWork.GetRepository<VolunteerMembership>();

            var activeCharitiesCount = await charityRepo.CountAsync(new ActiveCharitiesSpecification());
            var completedDonationsCount = await donationRepo.CountAsync(new CompletedDonationsSpecification());
            var approvedMembershipsCount = await membershipRepo.CountAsync(new ApprovedVolunteerMembershipsSpecification());

            var dto = new HomeStatisticsDto
            {
                ActiveCharitiesCount = activeCharitiesCount,
                CompletedDonationsCount = completedDonationsCount,
                ApprovedVolunteerMembershipsCount = approvedMembershipsCount
            };

            return Result<HomeStatisticsDto>.OK(dto);
        }
    }
}
