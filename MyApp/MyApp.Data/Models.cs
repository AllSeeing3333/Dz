namespace MyApp.Data;

public class Client
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public bool IsRegular { get; set; }
    public List<Rental> Rentals { get; set; } = new();
    public override string ToString() => $"{FullName} · {Phone}";
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int PricePerHour { get; set; }
    public string Size { get; set; } = "";
    public bool IsAvailable { get; set; }
    public List<Rental> Rentals { get; set; } = new();
    public override string ToString() => $"{Name} · {Size} · {PricePerHour} р/ч";
}

public class Rental
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public Client Client { get; set; } = null!;
    public int ItemId { get; set; }
    public Item Item { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int TotalCost { get; set; }
    public override string ToString() => $"{Client?.FullName} — «{Item?.Name}», {TotalCost} р.";
}