using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
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

    private Thread _qThread;
    private Thread _wThread;
    private Thread _eThread;

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

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Threads started");

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

      if (_qThread != null && _qThread.IsAlive)
      {
        _qThread.Join();
      }

      _count = 0;

      if (_wThread != null && _wThread.IsAlive)
      {
        _wThread.Join();
      }

      if (_eThread != null && _eThread.IsAlive)
      {
        _eThread.Join();
      }

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Thread stopped");
    }

    private void UpdateLabel()
    {
      if (_label.InvokeRequired)
        _label.BeginInvoke((MethodInvoker)(() => _label.Text = _count.ToString())); // ✅ Non-blocking update
      else
        _label.Text = _count.ToString();
    }

    public void Run(char key)
    {
      while (_isRunning)
      {
        try
        {
          if (AutoFlags.IsTargetWindowActive == false)
          {
            Thread.Sleep(10);
            continue;
          }

          var sw = Stopwatch.StartNew();
          if (key == 'q' && _QEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
            Thread.Sleep(_keyDelay);
          }

          if (key == 'w' && _WEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
            Thread.Sleep(_keyDelay);
          }

          if (key == 'e' && _EEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_E);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_E);
            Thread.Sleep(_keyDelay);
          }

          Interlocked.Increment(ref _count);

          UpdateLabel();

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
