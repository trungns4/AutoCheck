using log4net;
using MXTools.Input;
using System;
using System.Reflection;
using System.Threading;

namespace MXTools.Threads
{
  class GeneralThread
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    private Thread _thread;
    private bool _isRunning = false;
    private readonly int _delay = 1000;

    public GeneralThread()
    {
    }

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

        _log.Info("General thread started");
        return true;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while starting GeneralThread: {ex.Message}");
        return false;
      }
    }

    public void Stop()
    {
      try
      {
        _isRunning = false;

        if (_thread != null)
        {
          if (!_thread.Join(GlobalSettings.Instance.ThreadJoinWaitingTime))
          {
            _log.Warn("General thread did not terminate in time.");
          }
          _thread = null;
        }

        _log.Info("General thread stopped");
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while stopping GeneralThread: {ex.Message}");
      }
    }

    private void Run()
    {
      while (_isRunning)
      {
        try
        {
          if (!MxSharp.Instance.EnsureAttached())
          {
            _log.Info("App is not running");
            Thread.Sleep(_delay);
            continue;
          }

          GlobalFlags.IsTargetWindowActive =
              (ForegroundWindowCheck.Instance.GetCurrentProcessId() == MxSharp.Instance.PID());

          if (!GlobalFlags.IsTargetWindowActive)
          {
            ForegroundWindowCheck.Instance.ForceForegroundCheck();
            GlobalFlags.IsTargetWindowActive =
                (ForegroundWindowCheck.Instance.GetCurrentProcessId() == MxSharp.Instance.PID());
          }
        }
        catch (Exception ex)
        {
          _log.Fatal($"An error occurred in GeneralThread Run method: {ex.Message}");
        }
        Thread.Sleep(_delay);
      }
    }
  }
}
