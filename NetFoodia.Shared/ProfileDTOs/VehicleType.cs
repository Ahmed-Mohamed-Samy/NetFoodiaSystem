namespace NetFoodia.Shared.ProfileDTOs
{
    /// <summary>
    /// Vehicle type enum for volunteer profile DTOs.
    /// Integer values mirror <c>NetFoodia.Domain.Entities.ProfileModule.VehicleType</c>
    /// and the AI Smart Matching model's training encoding exactly.
    /// </summary>
    public enum VehicleType
    {
        Walking    = 1,
        Bicycle    = 2,
        Motorcycle = 3,
        Car        = 4
    }
}
