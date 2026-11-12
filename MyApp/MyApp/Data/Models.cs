namespace MyApp.Data;

public class Client
{
    public int Id { get; set; }
    public string FullName { get; set; } = "";
    public string Phone { get; set; } = "";
    public override string ToString() => $"{FullName} · {Phone}";
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Size { get; set; } = "";
    public override string ToString() => $"{Name} · {Size}";
}

public class Rental
{
    public int Id { get; set; }
    public Client? Client { get; set; }
    public Item? Item { get; set; }
    public override string ToString() => $"{Client?.FullName} - «{Item?.Name}»";
}