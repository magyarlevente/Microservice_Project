namespace Common.Entities;

public class BaseEntity : IEntity
{
    public int Id { get; set; }
    public DateTimeOffset CreatedAt { get; set; } =  DateTimeOffset.UtcNow;
}