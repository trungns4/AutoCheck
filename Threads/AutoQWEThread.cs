using log4net;
using MXTools.Input;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools.Threads
{
  internal class AutoQWEThread(QWEThreadSettings settings, Action<long> display)
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private Thread _qThread;
    private Thread _wThread;
    private Thread _eThread;

    private bool _isRunning = false;
    private long _count = 0;
    private readonly QWEThreadSettings _settings = settings;
    private Action<long> _display = display;

    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      try
      {
        return _isRunning;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in IsRunning: {ex.Message}");
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    public Action<long> Display
    {
      get
      {
        try
        {
          return _display;
        }
        catch (Exception ex)
        {
          _log.Fatal($"An error occurred in Display getter: {ex.Message}");
          return _ => { };
        }
      }
      set
      {
        try
        {
          _display = value;
        }
        catch (Exception ex)
        {
          _log.Fatal($"An error occurred in Display setter: {ex.Message}");
        }
      }
    }
    //---------------------------------------------------------------------------------------
    public bool Start()
    {
      try
      {
        _isRunning = true;
        _qThread = new Thread(() => Run('q')) { IsBackground = true, Priority = ThreadPriority.Normal };
        _wThread = new Thread(() => Run('w')) { IsBackground = true, Priority = ThreadPriority.Normal };
        _eThread = new Thread(() => Run('e')) { IsBackground = true, Priority = ThreadPriority.Normal };

        _count = 0;
        _qThread.Start();
        _wThread.Start();
        _eThread.Start();

        _log.Info("Auto Key Threads started");
        _log.Info($"Q: {_settings.Q} W: {_settings.W} E: {_settings.E}");

        return true;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while starting AutoQWEThread: {ex.Message}");
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    public void Stop()
    {
      try
      {
        _isRunning = false;
        Thread.Sleep(100);
        _count = 0;

        StopThread(ref _qThread, "Q");
        StopThread(ref _wThread, "W");
        StopThread(ref _eThread, "E");

        _log.Info("Auto Key Threads stopped");
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while stopping AutoQWEThread: {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void StopThread(ref Thread thread, string name)
    {
      try
      {
        if (thread != null && thread.IsAlive)
        {
          if (!thread.Join(GlobalSettings.Instance.ThreadJoinWaitingTime))
          {
            _log.Warn($"{name} thread did not terminate in time.");
          }
          thread = null;
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while stopping {name} thread: {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void DisplayCountNumber()
    {
      try
      {
        _display?.Invoke(_count);
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in DisplayCountNumber: {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void UpdateUIAndSleep(int sleep)
    {
      try
      {
        Interlocked.Increment(ref _count);
        DisplayCountNumber();
        Thread.Sleep(sleep);
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in UpdateUIAndSleep: {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void Run(char key)
    {
      try
      {
        while (_isRunning)
        {
          if (!GlobalFlags.IsTargetWindowActive)
          {
            Thread.Sleep(Math.Min(_settings.ThreadDelayQ, Math.Min(_settings.ThreadDelayW, _settings.ThreadDelayE)));
            continue;
          }

          switch (key)
          {
            case 'q' when _settings.Q:
              PressKey(Keys.Q, _settings.KeyDownDelayQ, _settings.KeyUpDelayQ);
              break;
            case 'w' when _settings.W:
              PressKey(Keys.W, _settings.KeyDownDelayW, _settings.KeyUpDelayW);
              break;
            case 'e' when _settings.E:
              PressKey(Keys.E, _settings.KeyDownDelayE, _settings.KeyUpDelayE);
              break;
          }

          if (!_settings.Q && !_settings.W && !_settings.E)
          {
            DisplayCountNumber();
            Thread.Sleep(Math.Min(_settings.ThreadDelayQ, Math.Min(_settings.ThreadDelayW, _settings.ThreadDelayE)));
          }
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in the Run method ({key}): {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void PressKey(Keys key, int downDelay, int upDelay)
    {
      try
      {
        KeyboardManager.Active.KeyDown((byte)key);
        Thread.Sleep(downDelay);
        KeyboardManager.Active.KeyUp((byte)key);
        UpdateUIAndSleep(upDelay);
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while pressing {key}: {ex.Message}");
      }
    }
  }
}
