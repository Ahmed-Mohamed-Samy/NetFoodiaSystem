namespace NetFoodia.Shared.ProfileDTOs
{
    public class MyProfileDTO
    {
        public UserInfoDTO User { get; set; } = default!;
        public string Role { get; set; } = default!;
        public DonorProfileDTO? Donor { get; set; }
        public VolunteerProfileDTO? Volunteer { get; set; }
        public CharityAdminProfileDTO? CharityAdmin { get; set; }


    }
}
