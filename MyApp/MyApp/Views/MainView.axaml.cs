using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;

namespace MyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        // Создаем базу и заполняем её при запуске
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
            DbSeed.Seed(db); 
        }

        LoadClients();
        LoadItems();
        LoadRentals();
    }

    private void LoadClients()
    {
        using var db = new AppDbContext();
        ClientsList.ItemsSource = db.Clients
            .OrderBy(c => c.FullName)
            .ToList();
    }

    private void LoadItems()
    {
        using var db = new AppDbContext();
        ItemsList.ItemsSource = db.Items
            .OrderBy(i => i.Name)
            .ToList();
    }

    private void LoadRentals()
    {
        using var db = new AppDbContext();
        RentalsList.ItemsSource = db.Rentals
            .Include(r => r.Client)
            .Include(r => r.Item)
            .OrderBy(r => r.StartTime)
            .ToList();
    }
}