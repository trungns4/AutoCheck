using log4net;
using MXTools.Helpers;
using MXTools.Input;
using System.Reflection;
using System.Threading;

namespace MXTools.Threads
{
  internal class AutoMouseThread(MouseThreadSettings settings)
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private Thread _thread;
    private bool _isRunning = false;
    private bool _up = false;

    //---------------------------------------------------------------------------------------
    public bool Start()
    {
      _isRunning = true;
      _thread = new Thread(Run)
      {
        IsBackground = true,
        Priority = ThreadPriority.Normal
      };
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
        MouseManager.Active.RightButtonUp();
      }
    }
    //---------------------------------------------------------------------------------------
    private void Run()
    {
      while (_isRunning)
      {
        if (settings.Auto == false)
        {
          Thread.Sleep(settings.ThreadDelay);
          continue;
        }

        if (GlobalFlags.IsTargetWindowActive == false)
        {
          CheckToFireUp();
          Thread.Sleep(settings.ThreadDelay);
          continue;
        }

        if (Win32.IsAltHolding())
        {
          CheckToFireUp();
          Thread.Sleep(settings.ThreadDelay);
          continue;
        }
        else
        {
          Win32.GetMousePos(out int x, out int y);
          var rect = ForegroundWindowCheck.Instance.GetCurrentRectangle();
          if (rect.Contains(x, y))
          {
            MouseManager.Active.RightButtonDown();
            _up = false;
            Thread.Sleep(settings.ClickDelay);
          }
          else
          {
            CheckToFireUp();
            Thread.Sleep(settings.ThreadDelay);
          }
        }

        Thread.Sleep(settings.ThreadDelay);
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

      MouseManager.Active.RightButtonUp();
      _log.DebugFormat("Mouse Click Thread stopped");
    }

    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
  }
}
