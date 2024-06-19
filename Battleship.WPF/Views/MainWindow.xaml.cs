using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Battleship.WPF.ViewModels;

namespace Battleship.WPF.Views;

public partial class MainWindow : Window
{
    private readonly BattleshipVM _battleshipVm = new();
    private readonly Random _random = new();

    public MainWindow()
    {
        DataContext = _battleshipVm;
        InitializeComponent();
    }

    private void OnBorderMouseDown(object sender, MouseButtonEventArgs e)
    {
        Border? border = sender as Border;
        CellVM? cell = border?.DataContext as CellVM;
        cell?.ToShot();

        int x = _random.Next(App.MapSize);
        int y = _random.Next(App.MapSize);
        _battleshipVm.ShotToOurMap(x, y);
    }
}