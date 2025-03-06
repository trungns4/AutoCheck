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
    readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
    public bool Start()
    {
      _isRunning = true;
      _qThread = new Thread(() => Run('q'));
      _qThread.IsBackground = true;
      _qThread.Priority = ThreadPriority.Normal;
      _count = 0;
      _qThread.Start();

      _wThread = new Thread(() => Run('w'));
      _wThread.IsBackground = true;
      _wThread.Priority = ThreadPriority.Normal;
      _wThread.Start();

      _eThread = new Thread(() => Run('e'));
      _eThread.IsBackground = true;
      _eThread.Priority = ThreadPriority.Normal;
      _eThread.Start();

      _log.InfoFormat("Auto Key Threads started");
      _log.InfoFormat($"Q: {_settings.Q} W: {_settings.W} E: {_settings.E}");

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

      _log.DebugFormat("Auto Key Threads stopped");
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
        if (GlobalFlags.IsTargetWindowActive == false)
        {
          Thread.Sleep(Math.Min(_settings.ThreadDelayQ, Math.Min(_settings.ThreadDelayW, _settings.ThreadDelayE)));
          continue;
        }

        if (key == 'q' && _settings.Q)
        {
          KeyboardManager.Active.KeyDown((byte)Keys.Q);
          Thread.Sleep(_settings.KeyDownDelayQ);
          KeyboardManager.Active.KeyUp((byte)Keys.Q);
          UpdateUIAndSleep(_settings.KeyUpDelayQ);
        }
        else if (key == 'w' && _settings.W)
        {
          KeyboardManager.Active.KeyDown((byte)Keys.W);
          Thread.Sleep(_settings.KeyDownDelayW);
          KeyboardManager.Active.KeyUp((byte)Keys.W);
          UpdateUIAndSleep(_settings.KeyUpDelayW);
        }
        else if (key == 'e' && _settings.E)
        {
          KeyboardManager.Active.KeyDown((byte)Keys.E);
          Thread.Sleep(_settings.KeyDownDelayE);
          KeyboardManager.Active.KeyUp((byte)Keys.E);
          UpdateUIAndSleep(_settings.KeyUpDelayE);
        }

        if (_settings.Q == false && _settings.W == false && _settings.E == false)
        {
          DisplayCountNumber();
          Thread.Sleep(Math.Min(_settings.ThreadDelayQ, Math.Min(_settings.ThreadDelayW, _settings.ThreadDelayE)));
        }
      }
    }
  }
}
