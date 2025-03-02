using Binarysharp.MemoryManagement;
using log4net;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools
{
  internal class AutoQWEThread
  {
    private Thread _qThread;
    private Thread _wThread;
    private Thread _eThread;

    private bool _isRunning = false;
    private long _count = 0;
    private QWEThreadSettings _settings;
    private Action<long> _display;

    public AutoQWEThread(QWEThreadSettings settings, Action<long> display)
    {
      _display = display;
      _settings = settings;
    }
    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
    //---------------------------------------------------------------------------------------
    public Action<long> Dislplay
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
    public bool Start(MemorySharp sharp)
    {
      _isRunning = true;
      _qThread = new Thread(() => Run('q'));
      _qThread.IsBackground = true;
      _qThread.Priority = ThreadPriority.Normal;
      _count = 0;
      _qThread.Start();

      Thread.Sleep(50);

      _wThread = new Thread(() => Run('w'));
      _wThread.IsBackground = true;
      _wThread.Priority = ThreadPriority.Normal;
      _wThread.Start();

      Thread.Sleep(50);

      _eThread = new Thread(() => Run('e'));
      _eThread.IsBackground = true;
      _eThread.Priority = ThreadPriority.Normal;
      _eThread.Start();

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.InfoFormat("Auto Key Threads started");
      log.InfoFormat($"Q: {_settings._q} W: {_settings._w} E: {_settings._e}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public void Stop()
    {
      _isRunning = false;
      Thread.Sleep(100);
      _count = 0;

      if (_qThread != null && _qThread.IsAlive)
      {
        _qThread.Join();
        _qThread = null;
      }

      if (_wThread != null && _wThread.IsAlive)
      {
        _wThread.Join();
        _wThread = null;
      }

      if (_eThread != null && _eThread.IsAlive)
      {
        _eThread.Join();
        _eThread = null;
      }

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Threads stopped");
    }
    //---------------------------------------------------------------------------------------
    private void DisplayCountNumber()
    {
      if (_display != null)
      {
        Task.Run(() =>
        {
          _display(_count);
        });
      }
    }
    //---------------------------------------------------------------------------------------
    private void UpdateUIAndSleep(int sleep)
    {
      Interlocked.Increment(ref _count);
      DisplayCountNumber();
      Thread.Sleep(sleep);
    }
    //---------------------------------------------------------------------------------------
    public void Run(char key)
    {
      while (_isRunning)
      {
        if (AutoFlags.IsTargetWindowActive == false)
        {
          Thread.Sleep(_settings._threadDelayQ);
          continue;
        }

        if (key == 'q')
        {
          if (_settings._q)
          {
            Keyboard.KeyDown((byte)Keys.Q);
            Thread.Sleep(_settings._keyDownDelayQ);
            Keyboard.KeyUp((byte)Keys.Q);
            UpdateUIAndSleep(_settings._keyUpDelayQ);
          }
        }
        else if (key == 'w')
        {
          if (_settings._w)
          {
            Keyboard.KeyDown((byte)Keys.W);
            Thread.Sleep(_settings._keyDownDelayW);
            Keyboard.KeyUp((byte)Keys.W);
            UpdateUIAndSleep(_settings._keyUpDelayW);
          }
        }
        else if (key == 'e')
        {
          if (_settings._e)
          {
            Keyboard.KeyDown((byte)Keys.E);
            Thread.Sleep(_settings._keyDownDelayE);
            Keyboard.KeyUp((byte)Keys.E);
            UpdateUIAndSleep(_settings._keyUpDelayE);
          }
        }

        if (_settings._q == false && _settings._w == false && _settings._e == false)
        {
          DisplayCountNumber();
          Thread.Sleep(Math.Min(_settings._threadDelayQ, Math.Min(_settings._threadDelayW, _settings._threadDelayE)));
        }
      }
    }
  }
}
