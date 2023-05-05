using System.ComponentModel.DataAnnotations.Schema;
using EStoreWebApi.Shared.Entities;
using EStoreWebApi.Shared.Entities.Base;

namespace EStoreWebApi.Features.Orders.Entities;

public enum OrderStatus
{
    PaymentPending,
    Paid,
    InTransit,
    Received,
    InTransitReturned,
    Returned
}

public class Order : TimestampedEntity
{
    public int UserId { get; set; }

    public List<OrderItem> OrderItems { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.PaymentPending;     
    
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string? CompanyName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string AddressStreet1 { get; set; }
    
    public string AddressStreet2 { get; set; }

    public string AddressZipCode { get; set; }
    
    public string AddressCity { get; set; }

    public int AddressCountryId { get; set; }

    public Country AddressCountry { get; set; }

    [NotMapped] 
    public int TotalPriceInCents => OrderItems.Sum(item => item.TotalPriceInCents);
}
