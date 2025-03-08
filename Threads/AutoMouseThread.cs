using log4net;
using MXTools.Helpers;
using MXTools.Input;
using System;
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
      try
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
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while starting the AutoMouseThread: {ex.Message}");
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    private void CheckToFireUp()
    {
      try
      {
        if (_up == false)
        {
          _up = true;
          MouseManager.Active.RightButtonUp();
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in CheckToFireUp: {ex.Message}");
      }
    }
    //---------------------------------------------------------------------------------------
    private void Run()
    {
      while (_isRunning)
      {
        try
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
        catch (Exception ex)
        {
          _log.Fatal($"An error occurred in the Run method: {ex.Message}");
        }
      }
    }
    //---------------------------------------------------------------------------------------
    public void Stop()
    {
      try
      {
        _isRunning = false;

        if (_thread != null && _thread.IsAlive)
        {
          if (_thread.Join(GlobalSettings.Instance.ThreadJoinWaitingTime) == false)
          {
            _log.Warn("Mouse click thread did not terminate in time.");
          }
          _thread = null;
        }

        MouseManager.Active.RightButtonUp();
        _log.Info("Mouse click thread stopped");
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while stopping the AutoMouseThread: {ex.Message}");
      }
    }
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
  }
}
