namespace EStoreWebApi.Shared.Entities.Base;

public class TimestampedEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
