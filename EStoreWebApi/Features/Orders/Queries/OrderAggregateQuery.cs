using EStoreWebApi.Data;
using EStoreWebApi.Features.Orders.Entities;
using Microsoft.EntityFrameworkCore;

namespace EStoreWebApi.Features.Orders.Queries;

public class OrderAggregateQuery
{
    public static IQueryable<Order> Query(AppDbContext db)
        => db.Orders.Include(o => o.OrderItems);
}