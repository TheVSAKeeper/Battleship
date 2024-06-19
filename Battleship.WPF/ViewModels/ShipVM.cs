using System.Windows;
using Battleship.WPF.Models;
using Battleship.WPF.Shared.ViewModels;

namespace Battleship.WPF.ViewModels;

public class ShipVM : ViewModelBase
{
    private (int x, int y) pos;
    private DirectionShip dir = DirectionShip.Horizontal;
    private int rang = 1;
    private Rect rect;

    public ShipVM(Ship ship)
    {
        pos = (ship.X, ship.Y);
        rang = ship.Rang;
        dir = ship.Direction;
        rect = new Rect(ship.Borders.X * App.CellSize, ship.Borders.Y * App.CellSize, ship.Borders.Width * App.CellSize, ship.Borders.Height * App.CellSize);
    }

    public DirectionShip Direct
    {
        get => dir;
        set => Set(ref dir, value, nameof(Angle));
    }

    public int Rang
    {
        get => rang;
        set
        {
            Set(ref rang, value, nameof(RangView));
            Notify(nameof(RectView));
        }
    }

    public int RangView => rang * App.CellSize - 5;
    public int Angle => dir == DirectionShip.Horizontal ? 0 : 90;

    public (int x, int y) Pos
    {
        get => pos;
        set
        {
            Set(ref pos, value, nameof(X), nameof(Y));
            Ship ship = new(pos, rang, dir);
            rect = new Rect(ship.Borders.X * App.CellSize, ship.Borders.Y * App.CellSize, ship.Borders.Width * App.CellSize, ship.Borders.Height * App.CellSize);
            Notify(nameof(RectView));
        }
    }

    public int X => pos.x * App.CellSize + 3;
    public int Y => pos.y * App.CellSize + 3;

    public Rect RectView
    {
        get => rect;
        set => Set(ref rect, value);
    }
}