using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Catalogue.Entities;

public class ProductCategory : BaseEntity
{
    public string Name { get; set; }

    public List<Product> Products { get; set; }
}
