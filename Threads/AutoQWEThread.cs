using log4net;
using MXTools.Input;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MXTools.Threads
{
  internal class AutoQWEThread
  {
    ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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
      _log.InfoFormat($"Q: {_settings._q} W: {_settings._w} E: {_settings._e}");

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
        if (GlobalFlags.IsTargetWindowActive == false)
        {
          Thread.Sleep(Math.Min(_settings._threadDelayQ, Math.Min(_settings._threadDelayW, _settings._threadDelayE)));
          continue;
        }

        if (key == 'q' && _settings._q)
        {
          InputSender.SendKey((ushort)Keys.Q, true);
          Thread.Sleep(_settings._keyDownDelayQ);
          InputSender.SendKey((byte)Keys.Q, false);
          UpdateUIAndSleep(_settings._keyUpDelayQ);
        }
        else if (key == 'w' && _settings._w)
        {
          InputSender.SendKey((ushort)Keys.W, true);
          Thread.Sleep(_settings._keyDownDelayW);
          InputSender.SendKey((byte)Keys.W, false);
          UpdateUIAndSleep(_settings._keyUpDelayW);
        }
        else if (key == 'e' && _settings._e)
        {
          InputSender.SendKey((ushort)Keys.E, true);
          Thread.Sleep(_settings._keyDownDelayE);
          InputSender.SendKey((byte)Keys.E, false);
          UpdateUIAndSleep(_settings._keyUpDelayE);
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
