using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement;
using log4net;

namespace MXTools
{
  internal class AutoMouseThread
  {
    private MouseThreadSettings _settings;
    private Thread _thread;
    private MemorySharp _sharp;
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
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      var wnd = _sharp.Windows.MainWindow;
      if (wnd != null && wnd.Handle != IntPtr.Zero)
      {
        _left = wnd.X;
        _right = wnd.X + wnd.Width;
        _top = wnd.Y;
        _bottom = wnd.Y + wnd.Height;
        log.InfoFormat($"Target Window: {_left} {_top} {_right} {_bottom}");
        return true;
      }
      else
      {
        return false;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool Start(MemorySharp sharp)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      _sharp = sharp;
      CheckWindowRect();

      _isRunning = true;
      _thread = new Thread(Run);
      _thread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _thread.Start();

      log.Info("Mouse click thread started");
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
        if (AutoFlags.IsTargetWindowActive == false)
        {
          CheckToFireUp();
          Thread.Sleep(_settings._threadDelay);
          continue;
        }

        if (_right == 0 && _bottom == 0)
        {
          CheckWindowRect();
        }

        if (_settings._auto)
        {
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
        }
        else
        {
          Thread.Sleep(_settings._threadDelay);
        }
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

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Mouse Click Thread stopped");
    }

    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
    //---------------------------------------------------------------------------------------
    public void Pause(int time)
    {
      var running = _isRunning;
      _isRunning = false;
      Thread.Sleep(10);
      var sw = Stopwatch.StartNew();
      while (sw.ElapsedMilliseconds < time)
      {
        Thread.Sleep(time / 10);
      }
      _isRunning = running;
    }
  }
}
