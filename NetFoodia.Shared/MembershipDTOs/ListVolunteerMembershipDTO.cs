namespace NetFoodia.Shared.MembershipDTOs
{
    public class ListVolunteerMembershipDTO
    {
        public int Id { get; set; }
        public string VolunteerId { get; set; } = default!;
        public string VolunteerName { get; set; } = default!;
        public string Address { get; set; } = default!;
        public MembershipStatus Status { get; set; }

    }
}
