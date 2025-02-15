using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Binarysharp.MemoryManagement;
using log4net;
using WindowsInput;

namespace AutoCheck
{
  internal class AutoQWEThread
  {
    private bool _QEnable = true;
    private bool _WEnable = false;
    private bool _EEnable = false;

    private Thread _thread;
    private bool _isRunning = false;

    private int _keyDelay = 10;
    private int _threadOptTime = 50;

    private MemorySharp _sharp;
    private System.Windows.Forms.Label _label;

    private long _count = 0;
    private InputSimulator _is = new InputSimulator();

    public AutoQWEThread(System.Windows.Forms.Label label)
    {
      _label = label;
    }

    public bool IsRunning()
    {
      return _isRunning;
    }

    public int Delay
    {
      get { return _keyDelay; }
      set { _keyDelay = value; }
    }

    public int ThreadOptTime
    {
      get
      {
        return _threadOptTime;
      }
      set
      {
        _threadOptTime = value;
      }
    }

    public bool QEnable
    {
      get { return _QEnable; }
      set
      {
        _QEnable = value;
      }
    }

    public bool WEnable
    {
      get { return _WEnable; }
      set
      {
        _WEnable = value;
      }
    }

    public bool EEnable
    {
      get { return _EEnable; }
      set
      {
        _EEnable = value;
      }
    }

    public bool Start(MemorySharp sharp)
    {
      _sharp = sharp;

      _isRunning = true;
      _thread = new Thread(Run);
      _thread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _count = 0;
      _thread.Start();

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Thread started");

      log.InfoFormat($"Q: {_QEnable} W: {_WEnable} E: {_EEnable}");
      log.InfoFormat($"Key Delay: {_keyDelay}");
      log.InfoFormat($"Operation Time: {_threadOptTime}");

      return true;
    }

    public void Stop()
    {
      _isRunning = false;

      var sw = Stopwatch.StartNew();
      while (sw.ElapsedMilliseconds < 5 * _threadOptTime)
      {
        Thread.Sleep(20);
      }
      _thread?.Join();
      _thread = null;
      _count = 0;

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Thread stopped");
    }

    public void Run()
    {
      while (_isRunning)
      {
        try
        {
          if (Utils.IsWindowActive(_sharp.Pid) == false)
          {
            continue;
          }

          var sw = Stopwatch.StartNew();

          if (_QEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
            Thread.Sleep(_keyDelay);
          }

          if (_WEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
            Thread.Sleep(_keyDelay);
          }

          if (_EEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_E);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_E);
            Thread.Sleep(_keyDelay);
          }

          _count++;

          if (_label.InvokeRequired)
          {
            _label.BeginInvoke(new Action(() => _label.Text = _count.ToString()));
          }
          else
          {
            _label.Text = _count.ToString();
          }

          int sleepTime = _threadOptTime - (int)sw.ElapsedMilliseconds;
          if (sleepTime <= 0)
          {
            sleepTime = _keyDelay / 2;
          }
          Thread.Sleep(sleepTime);
        }
        catch
        {
        }
        finally
        {
        }
      }
    }
  }
}
