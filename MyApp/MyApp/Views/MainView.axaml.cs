using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MyApp.Data;

namespace MyApp.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
            DbSeed.Seed(db);
        }

        RefreshAll();
    }

    // ============ ОБНОВЛЕНИЕ ВСЕХ СПИСКОВ ============
    private void RefreshAll()
    {
        RefreshClients();
        RefreshItems();
        RefreshRentals();
        RefreshCombos();
    }

    private void RefreshClients()
    {
        using var db = new AppDbContext();
        ClientsList.ItemsSource = db.Clients.OrderBy(c => c.FullName).ToList();
    }

    private void RefreshItems()
    {
        using var db = new AppDbContext();
        ItemsList.ItemsSource = db.Items.OrderBy(i => i.Name).ToList();
    }

    private void RefreshRentals()
    {
        using var db = new AppDbContext();
        RentalsList.ItemsSource = db.Rentals
            .Include(r => r.Client)
            .Include(r => r.Item)
            .ToList();
    }

    private void RefreshCombos()
    {
        using var db = new AppDbContext();
        var clients = db.Clients.OrderBy(c => c.FullName).ToList();
        var items = db.Items.OrderBy(i => i.Name).ToList();

        RentalClientCombo.ItemsSource = clients;
        RentalClientCombo.SelectedItem = null;

        RentalItemCombo.ItemsSource = items;
        RentalItemCombo.SelectedItem = null;
    }

    // ============ КЛИЕНТЫ ============
    private void AddClient_Click(object sender, RoutedEventArgs e)
    {
        ClientMessage.Text = "";
        var name = ClientNameBox.Text?.Trim();
        var phone = ClientPhoneBox.Text?.Trim();
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
        {
            ClientMessage.Text = "Заполните имя и телефон!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            db.Clients.Add(new Client { FullName = name, Phone = phone, IsRegular = ClientIsRegularBox.IsChecked == true });
            db.SaveChanges();
            ClientNameBox.Text = "";
            ClientPhoneBox.Text = "";
            ClientIsRegularBox.IsChecked = false;
            RefreshAll();
            ClientMessage.Text = "Клиент добавлен.";
        }
        catch (Exception ex)
        {
            ClientMessage.Text = "Ошибка: " + ex.Message;
        }
    }

    private void DeleteClient_Click(object sender, RoutedEventArgs e)
    {
        ClientMessage.Text = "";
        if (ClientsList.SelectedItem is not Client client)
        {
            ClientMessage.Text = "Выберите клиента в списке!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var toDelete = db.Clients.Find(client.Id);
            if (toDelete != null)
            {
                db.Clients.Remove(toDelete);
                db.SaveChanges();
            }
            RefreshAll();
            ClientMessage.Text = "Клиент удалён.";
        }
        catch (Exception ex)
        {
            ClientMessage.Text = "Ошибка: " + ex.Message;
        }
    }

    private void ClientsList_SelectionChanged(object sender, SelectionChangedEventArgs e) { }

    // ============ ИНВЕНТАРЬ ============
    private void AddItem_Click(object sender, RoutedEventArgs e)
    {
        ItemMessage.Text = "";
        var name = ItemNameBox.Text?.Trim();
        var size = ItemSizeBox.Text?.Trim();
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(size))
        {
            ItemMessage.Text = "Заполните название и размер!";
            return;
        }
        if (!int.TryParse(ItemPriceBox.Text, out var price))
        {
            ItemMessage.Text = "Цена должна быть числом!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            db.Items.Add(new Item { Name = name, PricePerHour = price, Size = size, IsAvailable = ItemAvailableBox.IsChecked == true });
            db.SaveChanges();
            ItemNameBox.Text = "";
            ItemPriceBox.Text = "";
            ItemSizeBox.Text = "";
            ItemAvailableBox.IsChecked = false;
            RefreshAll();
            ItemMessage.Text = "Товар добавлен.";
        }
        catch (Exception ex)
        {
            ItemMessage.Text = "Ошибка: " + ex.Message;
        }
    }

    private void DeleteItem_Click(object sender, RoutedEventArgs e)
    {
        ItemMessage.Text = "";
        if (ItemsList.SelectedItem is not Item item)
        {
            ItemMessage.Text = "Выберите товар в списке!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var toDelete = db.Items.Find(item.Id);
            if (toDelete != null)
            {
                db.Items.Remove(toDelete);
                db.SaveChanges();
            }
            RefreshAll();
            ItemMessage.Text = "Товар удалён.";
        }
        catch (Exception ex)
        {
            ItemMessage.Text = "Ошибка: " + ex.Message;
        }
    }

    // ============ АРЕНДЫ ============
    private void AddRental_Click(object sender, RoutedEventArgs e)
    {
        RentalMessage.Text = "";
        if (RentalClientCombo.SelectedItem is not Client client)
        {
            RentalMessage.Text = "Выберите клиента!";
            return;
        }
        if (RentalItemCombo.SelectedItem is not Item item)
        {
            RentalMessage.Text = "Выберите сапборд!";
            return;
        }
        if (RentalStartDate.SelectedDate == null)
        {
            RentalMessage.Text = "Выберите дату!";
            return;
        }
        if (!int.TryParse(RentalCostBox.Text, out var cost))
        {
            RentalMessage.Text = "Стоимость должна быть числом!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var start = RentalStartDate.SelectedDate.Value;
            db.Rentals.Add(new Rental
            {
                ClientId = client.Id,
                ItemId = item.Id,
                StartTime = start,
                EndTime = start.AddDays(1),
                TotalCost = cost
            });
            db.SaveChanges();
            RentalCostBox.Text = "";
            RentalStartDate.SelectedDate = null;
            RefreshAll();
            RentalMessage.Text = "Аренда добавлена.";
        }
        catch (Exception ex)
        {
            RentalMessage.Text = "Ошибка: " + ex.Message;
        }
    }

    private void DeleteRental_Click(object sender, RoutedEventArgs e)
    {
        RentalMessage.Text = "";
        if (RentalsList.SelectedItem is not Rental rental)
        {
            RentalMessage.Text = "Выберите аренду в списке!";
            return;
        }

        try
        {
            using var db = new AppDbContext();
            var toDelete = db.Rentals.Find(rental.Id);
            if (toDelete != null)
            {
                db.Rentals.Remove(toDelete);
                db.SaveChanges();
            }
            RefreshAll();
            RentalMessage.Text = "Аренда удалена.";
        }
        catch (Exception ex)
        {
            RentalMessage.Text = "Ошибка: " + ex.Message;
        }
    }
}