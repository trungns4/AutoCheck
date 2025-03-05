using log4net;
using MxTools;
using MXTools.Helpers;
using MXTools.Input;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools.Threads
{
  internal class AutoKeyThread
  {
    private ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private char _key;
    private QWMemThreadSettings _settings;

    //current
    private int _curVal = 0;
    private ulong _curAdr = 0;

    //max
    private int _maxVal = 0;
    private ulong _maxAdr = 0;

    private Thread _thread;
    private Thread _keyThread;
    private Thread _warnThread;
    private ManualResetEvent _keyFlag = new ManualResetEvent(false);

    private bool _isRunning = false;
    private bool _full = false;
    private SoundPlayer _player;
    private Keys _keyCode;

    private Action<int, int> _display;

    //---------------------------------------------------------------------------------------
    public AutoKeyThread(char key, QWMemThreadSettings settings, Action<int, int> display)
    {
      _key = key;
      _keyCode = Win32.KeyCode(_key);
      _settings = settings;
      _player = new SoundPlayer("alarm2.mp3", _settings._warnVolume);
      _display = display;
    }
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
      _curAdr = curAddr;
      _maxAdr = maxAddr;

      if (_settings._warnScale != 0)
      {
        _player.Volume = _settings._warnVolume;
      }
      else
      {
        _player.Volume = 0;
      }

      _curVal = 0;
      _maxVal = 0;

      _isRunning = true;
      _thread = new Thread(RunMemoryCheck);
      _thread.IsBackground = true;
      _thread.Priority = ThreadPriority.Normal;
      _thread.Start();

      _keyThread = new Thread(RunKeyCheck);
      _keyThread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _keyThread.Start();

      _warnThread = new Thread(RunWarnCheck);
      _warnThread.IsBackground = true;
      _warnThread.Start();

      _log.Info("Monitoring Thread started");

      _log.InfoFormat($"Key: {_key}");
      _log.InfoFormat($"Key Up Delay: {_settings._keyUpDelay}");
      _log.InfoFormat($"Key Down Delay: {_settings._keyDownDelay}");
      _log.InfoFormat($"Key Thread Delay: {_settings._keyThreadDelay}");
      _log.InfoFormat($"Mem Thread Delay: {_settings._memThreadDelay}");
      _log.InfoFormat($"Warn Thread Delay: {_settings._warnThreadDelay}");

      _log.InfoFormat($"Scale: {_settings._scale}");
      _log.InfoFormat($"Warn Scale: {_settings._warnScale}");
      _log.InfoFormat($"Warn Volume: {_settings._warnVolume:0.00}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public bool Stop()
    {
      _isRunning = false;

      if (_thread != null && _thread.IsAlive)
      {
        _thread.Join();
        _thread = null;
      }

      //to stop the key thread
      _keyFlag.Set();

      Thread.Sleep(10);

      if (_keyThread != null && _keyThread.IsAlive)
      {
        _keyThread.Join();
        _keyThread = null;
      }

      Thread.Sleep(10);

      if (_warnThread != null && _warnThread.IsAlive)
      {
        _warnThread.Join();
        _warnThread = null;
      }

      _player.Stop();

      _log.Info($"Thread {_key} stopped");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
    //---------------------------------------------------------------------------------------
    public string Value
    {
      get
      {
        return string.Format("{0}", _curVal);
      }
    }
    //---------------------------------------------------------------------------------------
    private void PlayAlarm(bool play)
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
    //---------------------------------------------------------------------------------------
    private void CheckWarning()
    {
      if (_settings._warnScale != 0)
      {
        double cv = _curVal;
        double mv = _maxVal;

        //warning
        if (cv <= mv * _settings._warnScale)
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
          if (_key == 'q')
          {
            if (MxSharp.Instance.EnsureAttached() == false)
            {
              _log.Info("App is not running");
              continue;
            }
            GlobalFlags.IsTargetWindowActive =
              (ForegroundWindowCheck.Instance.GetCurrentProcessId() == MxSharp.Instance.PID());
          }

          if (_curAdr >= 24 && _maxAdr >= 24 && _settings._auto)
          {
            _curVal = MxSharp.Instance.ReadMemory(_curAdr);
            _maxVal = MxSharp.Instance.ReadMemory(_maxAdr);

            double cv = _curVal;
            double mv = _maxVal;

            if (cv <= mv * _settings._scale)
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
        catch
        {
          _curVal = 0;
          _maxVal = 0;
          DisplayValues(0, 0);
        }
        finally
        {
          Thread.Sleep(_settings._memThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunKeyCheck()
    {
      while (_isRunning)
      {
        _keyFlag.WaitOne();
        bool delay = true;

        while (_settings._auto == true
              && _settings._autoKey == true
              && _isRunning == true
              && _full == false
              && GlobalFlags.IsTargetWindowActive == true)
        {
          KeyboardManager.Active.KeyDown((byte)_keyCode);
          Thread.Sleep(_settings._keyUpDelay);

          KeyboardManager.Active.KeyUp((byte)_keyCode);
          Thread.Sleep(_settings._keyDownDelay);

          delay = false;
        }
        if (delay == true)
        {
          Thread.Sleep(_settings._keyThreadDelay);
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
        finally
        {
          Thread.Sleep(_settings._warnThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void DisplayValues(int current, int max)
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
  }
}
