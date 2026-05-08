namespace NetFoodia.Domain.Entities.ProfileModule
{
    /// <summary>
    /// Vehicle type a volunteer uses for food pickups.
    /// Integer values must match the encoding used by the AI Smart Matching model exactly.
    /// Do NOT start from 0 — the AI was trained on 1-based values.
    /// </summary>
    public enum VehicleType
    {
        Walking    = 1,
        Bicycle    = 2,
        Motorcycle = 3,
        Car        = 4
    }
}
