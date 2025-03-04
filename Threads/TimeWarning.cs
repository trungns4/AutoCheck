using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using log4net;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Logging;
using MXTools.Helpers;

namespace MXTools.Threads
{

  internal class TimeWarning
  {
    ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private readonly Timer _timer;
    private SoundPlayer _player;
    private TimeWarningSettings _settings;
    private DateTime _time;
    private Action<int> _update;

    public TimeWarning(TimeWarningSettings setting)
    {
      _settings = setting;

      _timer = new Timer();
      _timer.Elapsed += OnTimerElapsed;
      _timer.AutoReset = true;
      _timer.Interval = _settings._timerInterval;

      _time = DateTime.MinValue;
      _player = new SoundPlayer("warning.mp3", _settings._volume);
    }
    //------------------------------------------------------------------------------
    private void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
      if (_time == DateTime.MinValue || _settings._auto == false)
      {
        return;
      }

      var elapsed = (DateTime.Now - _time).TotalMinutes;
      var remain = _settings._interval - elapsed;

      if (remain >= 0 && _update != null)
      {
        _update((int)(remain * 60));
      }

      if (_settings._dismiss.Any(x => Utils.IsKeyHeld(x)))
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
        Task.Run(() => _player.Play(TimeSpan.FromSeconds(_settings._duration)));
      }
    }
    //------------------------------------------------------------------------------
    public void Start()
    {
      _time = DateTime.Now;
      _timer.Start();
      _log.InfoFormat("Warning Timer Started");
    }
    //------------------------------------------------------------------------------
    public void Stop()
    {
      _timer.Stop();
      _player.Stop();
      _log.InfoFormat("Warning Timer Stopped");
    }
    //------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _timer.Enabled;
    }
    //------------------------------------------------------------------------------
    public Action<int> Update
    {
      get { return _update; }
      set { _update = value; }
    }
  }
}
