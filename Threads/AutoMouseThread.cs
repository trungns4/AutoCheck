using log4net;
using MxTools;
using MXTools.Helpers;
using MXTools.Input;
using System.Reflection;
using System.Threading;

namespace MXTools.Threads
{
  internal class AutoMouseThread
  {
    private ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private MouseThreadSettings _settings;
    private Thread _thread;
    private bool _isRunning = false;

    private bool _up = false;

    public AutoMouseThread(MouseThreadSettings settings)
    {
      _settings = settings;
    }

    //---------------------------------------------------------------------------------------
    public bool Start()
    {
      _isRunning = true;
      _thread = new Thread(Run);
      _thread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _thread.Start();

      _log.Info("Mouse click thread started");
      return true;
    }
    //---------------------------------------------------------------------------------------
    private void CheckToFireUp()
    {
      if (_up == false)
      {
        _up = true;
        InputSender.RightButtonUp();
      }
    }
    //---------------------------------------------------------------------------------------
    private void Run()
    {
      while (_isRunning)
      {
        if (_settings._auto == false)
        {
          Thread.Sleep(_settings._threadDelay);
          continue;
        }

        if (GlobalFlags.IsTargetWindowActive == false)
        {
          CheckToFireUp();
          Thread.Sleep(_settings._threadDelay);
          continue;
        }


        if (Win32.IsAltHolding())
        {
          CheckToFireUp();
          Thread.Sleep(_settings._threadDelay);
          continue;
        }
        else
        {
          Win32.GetMousePos(out int x, out int y);
          var rect = ForegroundWindowCheck.Instance.GetCurrentRectangle();
          if (rect.Contains(x, y))
          {
            InputSender.RightButtonDown();
            _up = false;
            Thread.Sleep(_settings._clickDelay);
          }
          else
          {
            CheckToFireUp();
            Thread.Sleep(_settings._threadDelay);
          }
        }

        Thread.Sleep(_settings._threadDelay);
      }
    }
    //---------------------------------------------------------------------------------------
    public void Stop()
    {
      _isRunning = false;

      Thread.Sleep(200);

      if (_thread != null && _thread.IsAlive)
      {
        _thread.Join();
        _thread = null;
      }

      InputSender.RightButtonUp();
      _log.DebugFormat("Mouse Click Thread stopped");
    }

    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
  }
}
