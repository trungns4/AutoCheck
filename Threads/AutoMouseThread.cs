using log4net;
using MXTools.Helpers;
using MXTools.Input;
using System;
using System.Diagnostics;
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

    private int _left = 0, _right = 0, _top = 0, _bottom = 0;
    private bool _up = false;

    public AutoMouseThread(MouseThreadSettings settings)
    {
      _settings = settings;
    }
    //---------------------------------------------------------------------------------------
    private bool CheckWindowRect()
    {
      var wnd = Utils.GetMainWindowHandle((int)MxSharp.Instance.PID());
      if (wnd != 0)
      {
        Utils.GetWindowRectangle(wnd, out int left, out int top, out int right, out int bottom);
        _left = left;
        _right = right;
        _top = top;
        _bottom = bottom;
        _log.InfoFormat($"Target Window: {_left} {_top} {_right} {_bottom}");
        return true;
      }
      else
      {
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool Start()
    {
      CheckWindowRect();

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

        if (_right == 0 && _bottom == 0)
        {
          CheckWindowRect();
        }

        if (Utils.IsAlt())
        {
          CheckToFireUp();
          Thread.Sleep(_settings._threadDelay);
          continue;
        }
        else
        {
          Utils.GetMouse(out int x, out int y);
          if (x >= _left && x <= _right && y >= _top && y <= _bottom)
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
