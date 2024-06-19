using Battleship.WPF.Models;

namespace Battleship.WPF.Services;

public interface IShipPlacer
{
    List<Ship> FillMap(params int[] shipTypes);
}