using Microsoft.EntityFrameworkCore;
using MyApp.Data;

Console.WriteLine($"База данных: {AppDbContext.DbPath}");

using (var db = new AppDbContext())
{
    db.Database.EnsureCreated();
    DbSeed.Seed(db);
}

using (var db = new AppDbContext())
{
    Console.WriteLine("\n=== Клиенты ===");
    foreach (var c in db.Clients.ToList())
        Console.WriteLine(c);

    Console.WriteLine("\n=== Инвентарь ===");
    foreach (var i in db.Items.ToList())
        Console.WriteLine(i);

    Console.WriteLine("\n=== Аренды ===");
    foreach (var r in db.Rentals.Include(x => x.Client).Include(x => x.Item).ToList())
        Console.WriteLine(r);
}