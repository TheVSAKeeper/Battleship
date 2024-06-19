using System.Windows;

namespace Battleship.WPF.Models;

public class Ship
{
    public Ship((int x, int y) position, int rang, DirectionShip direction = DirectionShip.Horizontal)
    {
        X = position.x;
        Y = position.y;
        Rang = rang;
        Direction = direction;
        Borders = GetShipRect();
    }

    public int X { get; }
    public int Y { get; }
    public int Rang { get; }
    public DirectionShip Direction { get; }
    public Rect Borders { get; }

    public bool Intersects(Ship other) => Borders.IntersectsWith(other.Borders);

    private Rect GetShipRect()
    {
        double width = Direction == DirectionShip.Horizontal ? Rang : 1;
        double height = Direction == DirectionShip.Vertical ? Rang : 1;

        return new Rect(X, Y, width, height);
    }

    public override string ToString() => $"Position ({X}, {Y}), rang {Rang}, direction {Direction}, borders {Borders}";
}