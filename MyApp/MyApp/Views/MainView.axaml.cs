using System.Collections.Generic;
using Avalonia.Controls;
using MyApp.Data;

namespace MyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        LoadData();
    }

    private void LoadData()
    {
        ClientsList.ItemsSource = new List<Client>
        {
            new Client { FullName = "Иван Петров", Phone = "+7 900 111-22-33" },
            new Client { FullName = "Мария Иванова", Phone = "+7 900 444-55-66" }
        };
        ItemsList.ItemsSource = new List<Item>
        {
            new Item { Name = "Каминные", Size = "Взрослый" },
            new Item { Name = "Обычные", Size = "Детский" }
        };
        RentalsList.ItemsSource = new List<Rental>();
    }
}