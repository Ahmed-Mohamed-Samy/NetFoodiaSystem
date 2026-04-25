namespace NetFoodia.Shared.DashboardDTOs
{
    public class DashboardStatsDTO
    {

        public int TotalDonations { get; set; }
        public double TotalFoodWeight { get; set; }
        public int PendingRequests { get; set; }
        public int TotalVolunteers { get; set; }


        public List<ChartItemDTO> DonationsPerMonth { get; set; } = [];
        public List<ChartItemDTO> FoodWeightPerMonth { get; set; } = [];
        //public List<ChartItemDTO> FoodSafetyStats { get; set; } = [];
    }
}
