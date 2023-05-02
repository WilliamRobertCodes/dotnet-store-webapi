using EStoreWebApi.Features.Catalogue.Entities;

namespace EStoreWebApi.Features.Catalogue.Responses;

public class ProductCategoryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }

    public static ProductCategoryResponse FromProductCategory(ProductCategory category)
    {
        return new ProductCategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
        };
    }
}

public class ProductResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public uint Price { get; set; }
    public List<ProductCategoryResponse> ProductCategories { get; set; }

    public static ProductResponse FromProduct(Product product)
    {
        return new ProductResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            ProductCategories = product.ProductCategories
                .Select(ProductCategoryResponse.FromProductCategory)
                .ToList(),
        };
    }
}

