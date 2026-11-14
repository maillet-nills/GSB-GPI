using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using GSB_GPI.Data;
using GSB_GPI.Models;

namespace GSB_GPI.ViewModels;

public record ParkItem(string Label, int Count, int Total, IBrush Color);

public partial class DashboardViewModel : ViewModelBase
{
    private readonly List<Ticket> _tickets;

    public int TotalEquipments { get; }
    public int TotalTickets { get; }
    public ParkItem[] ParkItems { get; }

    public string[] States { get; } = new[] { "Tous" }.Concat(Enum.GetNames<State>()).ToArray();

    [ObservableProperty] private string _selectedState = "Tous";

    public IEnumerable<Ticket> RecentTickets =>
        _tickets
            .Where(t => SelectedState == "Tous" || t.State.ToString() == SelectedState)
            .OrderByDescending(t => t.Timestamp)
            .Take(10);

    partial void OnSelectedStateChanged(string value) => OnPropertyChanged(nameof(RecentTickets));

    public DashboardViewModel()
    {
        var equipments = DbToolBox.LoadEquipments().ToList();
        _tickets = DbToolBox.LoadTickets();

        TotalEquipments = equipments.Count;
        TotalTickets = _tickets.Count;

        int total = Math.Max(1, equipments.Count);

        ParkItems = new[]
        {
            new ParkItem("Équipements réseau", equipments.OfType<NetEquipment>().Count(), total, new SolidColorBrush(Color.Parse("#1F3A6E"))),
            new ParkItem("Postes", equipments.OfType<Workstation>().Count(), total, new SolidColorBrush(Color.Parse("#6B8CEF"))),
            new ParkItem("Serveurs", equipments.OfType<Server>().Count(), total, new SolidColorBrush(Color.Parse("#A5B8F5")))
        };
    }
}