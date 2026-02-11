namespace NetFoodia.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
