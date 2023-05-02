using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Catalogue.Entities;

public class Product : TimestampedEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public uint Price { get; set; }

    public List<ProductCategory> ProductCategories { get; set; }
}
