using Battleship.WPF.Models;

namespace Battleship.WPF.Services;

public class RandomShipPlacer(int gridSize, int maxAttempts = 100) : IShipPlacer
{
    private readonly List<Ship> _ships = [];
    private readonly Random _random = new();
    private readonly Stack<(int currentShipType, Ship currentShip, int attempts)> _backtrackStack = new();

    private int _attempts;

    public int MaxAttemptsUsed { get; private set; }

    public List<Ship> FillMap(params int[] shipTypes)
    {
        MaxAttemptsUsed = 0;
        _ships.Clear();
        _backtrackStack.Clear();

        int shipType = FindNextShipType(shipTypes);

        if (shipType < 1)
        {
            Console.WriteLine("No more ships to place.");
            return _ships;
        }

        Ship ship = new((0, 0), shipType, DirectionShip.None);

        shipTypes[shipType]--;
        _attempts = 0;

        _backtrackStack.Push((shipType, ship, _attempts));

        while (_backtrackStack.Count > 0)
        {
            (shipType, ship, _attempts) = _backtrackStack.Pop();

            if (_attempts < maxAttempts)
            {
                ship = GenerateRandomShipPosition(ship.Rang);

                if (CanPlaceShip(_ships, ship))
                    PlaceShip(ship, shipTypes);
                else
                    RetryPlacingShip(ship, shipType);
            }
            else
            {
                RemoveLastShip(shipTypes);
            }
        }

        Console.WriteLine($"Maximum number of attempts used: {MaxAttemptsUsed}");

        return _ships;
    }

    private Ship GenerateRandomShipPosition(int shipType)
    {
        DirectionShip direction = GetRandomDirection();
        int x = GetRandomCoordinate(shipType, direction);

        int y = GetRandomCoordinate(shipType, direction == DirectionShip.Horizontal
            ? DirectionShip.Vertical
            : DirectionShip.Horizontal);

        return new Ship((x, y), shipType, direction);
    }

    private DirectionShip GetRandomDirection() =>
        _random.Next(2) == 0 ? DirectionShip.Horizontal : DirectionShip.Vertical;

    private int GetRandomCoordinate(int shipType, DirectionShip direction) =>
        direction == DirectionShip.Horizontal ? _random.Next(gridSize - shipType) : _random.Next(gridSize);

    private static bool CanPlaceShip(List<Ship> ships, Ship currentShip) =>
        ships.Any(currentShip.Intersects) == false;

    private void PlaceShip(Ship ship, int[] shipTypes)
    {
        MaxAttemptsUsed = Math.Max(MaxAttemptsUsed, _attempts);

        _ships.Add(ship);
        Console.WriteLine($"Placed ship {ship}.");

        int currentShipType = FindNextShipType(shipTypes);

        if (currentShipType < 1)
        {
            Console.WriteLine("No more ships to place.");
            return;
        }

        ship = new Ship((0, 0), currentShipType, DirectionShip.None);
        shipTypes[currentShipType]--;
        _attempts = 0;

        _backtrackStack.Push((currentShipType, ship, _attempts));
    }

    private void RetryPlacingShip(Ship ship, int shipType)
    {
        Console.WriteLine($"Ship {ship} intersects with existing ships. Trying again ({_attempts + 1}/{maxAttempts}).");
        _backtrackStack.Push((shipType, ship, _attempts + 1));
    }

    private void RemoveLastShip(int[] shipTypes)
    {
        if (_ships.Count < 1)
            return;

        shipTypes[_ships[^1].Rang]++;
        Console.WriteLine($"Removed ship {_ships[^1]}.");
        _ships.RemoveAt(_ships.Count - 1);
    }

    private static int FindNextShipType(int[] shipTypes)
    {
        int currentShipType = shipTypes.Length - 1;

        while (currentShipType > 0 && shipTypes[currentShipType] == 0)
        {
            currentShipType--;
        }

        return currentShipType;
    }
}