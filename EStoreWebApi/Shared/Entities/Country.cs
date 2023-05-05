using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Shared.Entities;

public class Country : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }
}
