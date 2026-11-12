namespace MyApp.Data;

public static class DbSeed
{
    public static void Seed(AppDbContext db)
    {
        if (db.Clients.Any()) return;

        var c1 = new Client { FullName = "Иван Петров", Phone = "+7 900 111-22-33", IsRegular = true };
        var c2 = new Client { FullName = "Мария Иванова", Phone = "+7 900 444-55-66" };
        var c3 = new Client { FullName = "Алексей Смирнов", Phone = "+7 900 777-88-99" };
        db.Clients.AddRange(c1, c2, c3);

        var i1 = new Item { Name = "Каминные", PricePerHour = 150, Size = "Взрослый", IsAvailable = true };
        var i2 = new Item { Name = "Обычные", PricePerHour = 50, Size = "Детский", IsAvailable = true };
        var i3 = new Item { Name = "Хозяйственные", PricePerHour = 80, Size = "Взрослый", IsAvailable = false };
        db.Items.AddRange(i1, i2, i3);

        db.SaveChanges();

        db.Rentals.AddRange(
            new Rental { ClientId = c1.Id, ItemId = i1.Id, StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1), TotalCost = 150 },
            new Rental { ClientId = c2.Id, ItemId = i2.Id, StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(2), TotalCost = 100 },
            new Rental { ClientId = c3.Id, ItemId = i3.Id, StartTime = DateTime.Now, EndTime = DateTime.Now.AddDays(1), TotalCost = 80 }
        );
        db.SaveChanges();
    }
}