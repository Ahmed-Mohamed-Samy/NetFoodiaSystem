namespace NetFoodia.Shared.MembershipDTOs
{
    public class VolunteerMembershipDTO
    {
        public int Id { get; set; }
        public int CharityId { get; set; }
        public string CharityName { get; set; } = default!;
        public MembershipStatus Status { get; set; }
        public string? RejectReason { get; set; }
        public string? SuspendReason { get; set; }

    }
}
