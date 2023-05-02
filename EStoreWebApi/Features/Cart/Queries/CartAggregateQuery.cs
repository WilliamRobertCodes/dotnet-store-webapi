using EStoreWebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Cart.Queries;

public static class CartAggregateQuery
{
    public static IQueryable<Entities.Cart> ForUser(AppDbContext db, int userId)
    {
        return db.Carts
            .Include(c => c.CartLineItems)
                .ThenInclude(i => i.Product)
            .AsQueryable()
            .Where(c => c.UserId == userId);
    }
}

