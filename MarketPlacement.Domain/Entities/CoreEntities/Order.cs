namespace MarketPlacement.Domain.Entities.CoreEntities;

public class Order
{
    public int Id { get; set; }

    public List<Product> Products { get; set; }
}