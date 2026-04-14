namespace NetFoodia.Shared.DonationDTOs
{
    public class PendingDonationListItemDTO
    {
        public int DonationId { get; set; }
        public string PictureUrl { get; set; } = default!;
        public string DonorName { get; set; } = default!;
        public string FoodType { get; set; } = default!;
        public int Quantity { get; set; }
        public DateTime ExpirationTime { get; set; }
        public float UrgencyScore { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
