using log4net;
using MXTools.Helpers;
using MXTools.Input;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools.Threads
{
  internal class AutoKeyThread(char key, QWMemThreadSettings settings, Action<int, int> display)
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private readonly char _key = key;
    private readonly Keys _keyCode = Win32.KeyCode(key);

    private readonly QWMemThreadSettings _settings = settings;
    private readonly SoundPlayer _player = new("alarm2.mp3", settings.WarnVolume);
    private Action<int, int> _display = display;

    //current
    private int _curVal = 0;
    private ulong _curAdr = 0;

    //max
    private int _maxVal = 0;
    private ulong _maxAdr = 0;

    private Thread _thread;
    private Thread _keyThread;
    private Thread _warnThread;
    private readonly ManualResetEvent _keyFlag = new(false);

    private bool _isRunning = false;
    private bool _full = false;

    //---------------------------------------------------------------------------------------
    public Action<int, int> Dislplay
    {
      get
      {
        return _display;
      }
      set
      {
        _display = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool Start(ulong curAddr, ulong maxAddr)
    {
      try
      {
        _curAdr = curAddr;
        _maxAdr = maxAddr;

        if (_settings.WarnScale != 0)
        {
          _player.Volume = _settings.WarnVolume;
        }
        else
        {
          _player.Volume = 0;
        }

        _curVal = 0;
        _maxVal = 0;

        _isRunning = true;
        _thread = new Thread(RunMemoryCheck)
        {
          IsBackground = true
        };
        _thread.Start();

        _keyThread = new Thread(RunKeyCheck)
        {
          IsBackground = true
        };
        _keyThread.Start();

        _warnThread = new Thread(RunWarnCheck)
        {
          IsBackground = true
        };
        _warnThread.Start();

        _log.Info("Monitoring thread started");

        return true;
      }
      catch (Exception ex)
      {
        _log.Fatal("Could not start the monitoring thread");
        _log.Fatal($"Exception: {ex.Message}");
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool Stop()
    {
      try
      {
        _isRunning = false;

        if (_thread != null && _thread.IsAlive)
        {
          if (_thread.Join(GlobalSettings.Instance.ThreadJoinWaitingTime) == false)
          {
            _log.Warn("Memory thread did not terminate in time.");
          }
          _thread = null;
        }

        //to stop the key thread
        _keyFlag.Set();


        if (_keyThread != null && _keyThread.IsAlive)
        {
          if (_keyThread.Join(GlobalSettings.Instance.ThreadJoinWaitingTime) == false)
          {
            _log.Warn("Key thread did not terminate in time.");
          }

          _keyThread = null;
        }


        if (_warnThread != null && _warnThread.IsAlive)
        {
          if (_warnThread.Join(GlobalSettings.Instance.ThreadJoinWaitingTime) == false)
          {
            _log.Warn("Warning thread did not terminate in time.");
          }
          _warnThread = null;
        }

        _player.Stop();

        _log.Info($"Thread {_key} stopped");

        return true;
      }
      catch (Exception ex)
      {
        _log.Fatal("Could not stop the monitoring thread");
        _log.Fatal($"Exception: {ex.Message}");
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
    //---------------------------------------------------------------------------------------
    private void PlayAlarm(bool play)
    {
      try
      {
        if (play)
        {
          Task.Run(() => _player.Play(TimeSpan.FromSeconds(10000)));
        }
        else
        {
          _player.Stop();
        }
      }
      catch (Exception ex)
      {
        _log.Fatal("Could play alarm");
        _log.Fatal($"Exception: {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void CheckWarning()
    {
      if (_settings.WarnScale != 0)
      {
        double cv = _curVal;
        double mv = _maxVal;

        //warning
        if (cv <= mv * _settings.WarnScale)
        {
          if (cv != 0)
          {
            PlayAlarm(true);
          }
        }
        else
        {
          PlayAlarm(false);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunMemoryCheck()
    {
      while (_isRunning)
      {
        try
        {
          if (_curAdr >= 24 && _maxAdr >= 24 && _settings.Auto)
          {
            _curVal = MxSharp.Instance.ReadMemory(_curAdr);
            _maxVal = MxSharp.Instance.ReadMemory(_maxAdr);

            double cv = _curVal;
            double mv = _maxVal;

            if (cv <= mv * _settings.Scale)
            {
              _full = false;
              _keyFlag.Set();
            }
            else
            {
              _full = true;
              _keyFlag.Reset();
            }
          }
        }
        catch (Exception ex)
        {
          _curVal = 0;
          _maxVal = 0;
          DisplayValues(0, 0);
          _log.Fatal("An error occured in the monitoring thread");
          _log.Fatal($"Exception: {ex.Message}");
        }
        finally
        {
          Thread.Sleep(_settings.MemThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunKeyCheck()
    {
      while (_isRunning)
      {
        try
        {
          _keyFlag.WaitOne();
          bool delay = true;

          while (_settings.Auto == true
                && _settings.AutoKey == true
                && _isRunning == true
                && _full == false
                && GlobalFlags.IsTargetWindowActive == true)
          {
            KeyboardManager.Active.KeyDown((byte)_keyCode);
            Thread.Sleep(_settings.KeyDownDelay);

            KeyboardManager.Active.KeyUp((byte)_keyCode);
            Thread.Sleep(_settings.KeyUpDelay);

            delay = false;
          }
          if (delay == true)
          {
            Thread.Sleep(_settings.KeyThreadDelay);
          }
        }
        catch (Exception ex)
        {
          _log.Fatal("An error occurred in the monitoring thread");
          _log.Fatal($"Exception: {ex.Message}");
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunWarnCheck()
    {
      while (_isRunning)
      {
        try
        {
          CheckWarning();
          DisplayValues(_curVal, _maxVal);
        }
        catch (Exception ex)
        {
          _log.Fatal("An error occurred in the monitoring thread");
          _log.Fatal($"Exception: {ex.Message}");
        }

        Thread.Sleep(_settings.WarnThreadDelay);
      }
    }
    //---------------------------------------------------------------------------------------
    private void DisplayValues(int current, int max)
    {
      try
      {
        if (_display != null)
        {
          if (current < 0)
          {
            current = 0;
          }

          if (max < 0)
          {
            max = 0;
          }
          Task.Run(() => _display(current, max));
        }

      }
      catch (Exception ex)
      {
        _log.Fatal("An error occurred in the monitoring thread");
        _log.Fatal($"Exception: {ex.Message}");
      }
    }
  }
}
