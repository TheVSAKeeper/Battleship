using System.Windows.Input;
using System.Windows.Threading;
using Battleship.WPF.Models;
using Battleship.WPF.Shared.Commands;
using Battleship.WPF.Shared.ViewModels;

namespace Battleship.WPF.ViewModels;

internal class BattleshipVM : ViewModelBase
{
    private readonly DispatcherTimer timer;
    private DateTime startTime;

    private ICommand? _fillEnemyMapCommand;
    private string time = "";

    public BattleshipVM()
    {
        timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(100)
        };

        timer.Tick += Timer_Tick;

        OurMap = new MapViewModel();
        FillOurMap();

        EnemyMap = new MapViewModel();
        FillEnemyMap();
    }

    /* string sampleMap = @"
  **********
  *XXXX***X*
  ******X***
  XX*XX***XX
  ******X***
  *XXX******
  *****XXX**
  **********
  *X********
  **********
  "; */

    public MapViewModel OurMap { get; }
    public MapViewModel EnemyMap { get; }

    public string Time
    {
        get => time;
        private set => Set(ref time, value);
    }

    public ICommand FillEnemyMapCommand => _fillEnemyMapCommand ??= new LambdaCommand(FillEnemyMap);

    private void FillOurMap()
    {
        OurMap.FillMap(new Ship((1, 1), 4),
            new Ship((6, 1), 3, DirectionShip.Vertical),
            new Ship((8, 1), 3, DirectionShip.Vertical),
            new Ship((1, 3), 2),
            new Ship((1, 5), 2),
            new Ship((4, 3), 2, DirectionShip.Vertical),
            new Ship((1, 9), 1),
            new Ship((2, 7), 1),
            new Ship((4, 7), 1),
            new Ship((8, 9), 1));
    }

    private void FillEnemyMap()
    {
        EnemyMap.FillMap(0, 4, 3, 2, 1);

        OnPropertyChanged(nameof(EnemyMap));
        OnPropertyChanged();
    }

    internal void ShotToOurMap(int x, int y)
    {
        OurMap[x, y].ToShot();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        TimeSpan dt = now - startTime;
        Time = dt.ToString(@"mm\:ss");
    }

    public void Start()
    {
        startTime = DateTime.Now;
        timer.Start();
    }

    public void Stop()
    {
        timer.Stop();
    }
}