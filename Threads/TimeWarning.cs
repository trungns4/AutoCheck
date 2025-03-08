using log4net;
using MXTools.Helpers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Timers;

namespace MXTools.Threads
{
  internal class TimeWarning
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private readonly Timer _timer;
    private readonly SoundPlayer _player;
    private readonly TimeWarningSettings _settings;
    private DateTime _time;
    private Action<int> _update;

    public TimeWarning(TimeWarningSettings setting)
    {
      try
      {
        _settings = setting ?? throw new ArgumentNullException(nameof(setting), "TimeWarningSettings cannot be null.");

        _timer = new Timer
        {
          AutoReset = true,
          Interval = _settings.TimerInterval
        };
        _timer.Elapsed += OnTimerElapsed;

        _time = DateTime.MinValue;
        _player = new SoundPlayer("warning.mp3", _settings.Volume);

        _log.Info("TimeWarning initialized successfully.");
      }
      catch (Exception ex)
      {
        _log.Fatal($"Failed to initialize TimeWarning: {ex.Message}");
        throw;
      }
    }

    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
      try
      {
        if (_time == DateTime.MinValue || !_settings.Auto)
        {
          return;
        }

        var elapsed = (DateTime.Now - _time).TotalMinutes;
        var remain = _settings.Interval - elapsed;

        _update?.Invoke((int)(remain * 60));

        if (_settings.Dismiss.Any(x => Win32.IsKeyHolding(x)))
        {
          _time = DateTime.Now;
          if (_player.IsPlaying())
          {
            _player.Stop();
          }
          return;
        }

        if (remain <= 0)
        {
          _time = DateTime.Now;
          Task.Run(() => _player.Play(TimeSpan.FromSeconds(_settings.Duration)));
        }
      }
      catch (Exception ex)
      {
        _log.Error($"Error in OnTimerElapsed: {ex.Message}");
      }
    }

    public void Start()
    {
      try
      {
        _time = DateTime.Now;
        _timer.Start();
        _log.Info("Warning Timer Started.");
      }
      catch (Exception ex)
      {
        _log.Fatal($"Failed to start TimeWarning: {ex.Message}");
      }
    }

    public void Stop()
    {
      try
      {
        _timer.Stop();
        _player.Stop();
        _log.Info("Warning Timer Stopped.");
      }
      catch (Exception ex)
      {
        _log.Fatal($"Failed to stop TimeWarning: {ex.Message}");
      }
    }

    public bool IsRunning()
    {
      return _timer.Enabled;
    }

    public Action<int> Update
    {
      get => _update;
      set => _update = value;
    }
  }
}
