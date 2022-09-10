namespace MarketPlacement.Domain.Entities.CoreEntities;

public class Product
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public bool Price { get; set; }

    public int OrderId { get; set; }

    public Order Order { get; set; }
    
    
}