using log4net;
using MXTools.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

    public void Stop()
    {
      _isRunning = false;

      if (_thread != null)
      {
        if (!_thread.Join(500)) // Timeout after 500ms
        {
          _log.Warn("Thread did not terminate in time.");
        }
        _thread = null;
      }

      _log.Debug("General Thread stopped");
    }


    public void Run()
    {
      while (_isRunning)
      {
        if (MxSharp.Instance.EnsureAttached() == false)
        {
          _log.Info("App is not running");
          Thread.Sleep(_delay);
          continue;
        }

        GlobalFlags.IsTargetWindowActive =
          (ForegroundWindowCheck.Instance.GetCurrentProcessId() == MxSharp.Instance.PID());

        if (GlobalFlags.IsTargetWindowActive == false)
        {
          ForegroundWindowCheck.Instance.ForceForegroundCheck();

          GlobalFlags.IsTargetWindowActive =
            (ForegroundWindowCheck.Instance.GetCurrentProcessId() == MxSharp.Instance.PID());
        }

        Thread.Sleep(_delay);
      }
    }
  }
}
