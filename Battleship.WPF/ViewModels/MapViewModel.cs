using System.Collections.ObjectModel;
using Battleship.WPF.Models;
using Battleship.WPF.Services;
using Battleship.WPF.Shared.ViewModels;

namespace Battleship.WPF.ViewModels;

public class MapViewModel : ViewModelBase
{
    private const int GridSize = 10;

    private readonly CellVM[,] _map;
    private readonly IShipPlacer _shipPlacer;

    private ObservableCollection<ShipVM>? _ships;

    public MapViewModel(string str) : this()
    {
        InitializeMapFromString(str);
    }

    public MapViewModel()
    {
        _map = new CellVM[GridSize, GridSize];
        InitializeMap();
        _shipPlacer = new RandomShipPlacer(GridSize);
    }

    public ObservableCollection<ShipVM>? Ships
    {
        get => _ships;
        set => Set(ref _ships, value);
    }

    public CellVM this[int x, int y] => _map[x, y];

    public IReadOnlyCollection<IReadOnlyCollection<CellVM>> Map
    {
        get
        {
            List<List<CellVM>> viewMap = [];

            for (int y = 0; y < GridSize; y++)
            {
                viewMap.Add([]);

                for (int x = 0; x < GridSize; x++)
                    viewMap[y].Add(this[x, y]);
            }

            return viewMap;
        }
    }

    private void InitializeMap()
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
                _map[i, j] = new CellVM();
        }
    }

    private void InitializeMapFromString(string str)
    {
        string[] mp = str.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                if (mp[i][j] == 'X')
                    _map[i, j].ToShip();
            }
        }
    }

    public void FillMap(params Ship[] ships)
    {
        UpdateMap(ships);
    }

    public void FillMap(params int[] navy)
    {
        List<Ship> ships = _shipPlacer.FillMap(navy);
        UpdateMap(ships);
    }

    private void UpdateMap(IEnumerable<Ship> newShips)
    {
        IEnumerable<Ship> ships = newShips as Ship[] ?? newShips.ToArray();
        UpdateMapWithShips(ships);
        Ships = new ObservableCollection<ShipVM>(ships.Select(ship => new ShipVM(ship)));
    }

    private void UpdateMapWithShips(IEnumerable<Ship> ships)
    {
        InitializeMap();

        foreach (Ship ship in ships)
        {
            switch (ship.Direction)
            {
                case DirectionShip.Horizontal:
                    for (int x = ship.X; x < ship.X + ship.Rang; x++)
                        this[x, ship.Y].ToShip();

                    break;

                case DirectionShip.Vertical:
                    for (int y = ship.Y; y < ship.Y + ship.Rang; y++)
                        this[ship.X, y].ToShip();

                    break;

                case DirectionShip.None:
                default:
                    throw new ArgumentOutOfRangeException(nameof(ships), ship, "The direction of the ship is not valid. "
                                                                               + "It should be either Horizontal or Vertical.");
            }
        }

        OnPropertyChanged(nameof(Map));
    }
}