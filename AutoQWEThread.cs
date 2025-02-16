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
using Newtonsoft.Json.Linq;
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

    private int _keyDownDelayQ = 16;
    private int _keyUpDelayQ = 16;
    private int _keyDownDelayW = 16;
    private int _keyUpDelayW = 16;
    private int _keyDownDelayE = 16;
    private int _keyUpDelayE = 16;

    private int _threadDelayQ = 32;
    private int _threadDelayW = 32;
    private int _threadDelayE = 32;

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
    //---------------------------------------------------------------------------------------
    public int Delay
    {
      get { return _keyUpDelayQ; }
      set { _keyUpDelayQ = value; }
    }
    //---------------------------------------------------------------------------------------
    public int ThreadDelayQ
    {
      get
      {
        return _threadDelayQ;
      }
      set
      {
        _threadDelayQ = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool QEnable
    {
      get { return _QEnable; }
      set
      {
        _QEnable = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool WEnable
    {
      get { return _WEnable; }
      set
      {
        _WEnable = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool EEnable
    {
      get { return _EEnable; }
      set
      {
        _EEnable = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public bool Start(MemorySharp sharp)
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

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Threads started");

      log.InfoFormat($"Q: {_QEnable} W: {_WEnable} E: {_EEnable}");
      log.InfoFormat($"Key Delay: {_keyUpDelayQ}");
      log.InfoFormat($"Thread Delay Q: {_threadDelayQ}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public void Stop()
    {
      _isRunning = false;
      Thread.Sleep(200);
      _count = 0;

      if (_qThread != null && _qThread.IsAlive)
      {
        _qThread.Join();
      }

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
    //---------------------------------------------------------------------------------------
    private void UpdateLabel()
    {
      if (_label.InvokeRequired)
        _label.BeginInvoke((MethodInvoker)(() => _label.Text = _count.ToString()));
      else
        _label.Text = _count.ToString();
    }
    //---------------------------------------------------------------------------------------
    private void UpdateUIAndSleep(int sleep)
    {
      Interlocked.Increment(ref _count);
      UpdateLabel();
      Thread.Sleep(sleep);
    }
    //---------------------------------------------------------------------------------------
    public void Run(char key)
    {
      while (_isRunning)
      {
        if (AutoFlags.IsTargetWindowActive == false)
        {
          Thread.Sleep(_threadDelayQ);
          continue;
        }

        if (key == 'q')
        {
          if (_QEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);
            Thread.Sleep(_keyDownDelayQ);

            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
            UpdateUIAndSleep(_keyUpDelayQ);
          }
        }

        if (key == 'w')
        {
          if (_WEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
            Thread.Sleep(_keyDownDelayW);

            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
            UpdateUIAndSleep(_keyUpDelayW);
          }
        }

        if (key == 'e')
        {
          if (_WEnable)
          {
            _is.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_E);
            Thread.Sleep(_keyDownDelayE);

            _is.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_E);
            UpdateUIAndSleep(_keyUpDelayE);
          }
        }

        if (_QEnable == false && _WEnable == false && _EEnable == false)
        {
          UpdateLabel();
          Thread.Sleep(Math.Min(_threadDelayQ, Math.Min(_threadDelayW, _threadDelayE)));
        }
      }
    }

    //---------------------------------------------------------------------------------------
    public JObject GetSettings()
    {
      JObject ret = new JObject()
      {
        ["_q"] = _QEnable,
        ["_w"] = _WEnable,
        ["_e"] = _EEnable,

        ["_keyUpDelayQ"] = _keyUpDelayQ,
        ["_keyDownDelayQ"] = _keyDownDelayQ,
        ["_threadDelayQ"] = _threadDelayQ,

        ["_keyUpDelayW"] = _keyUpDelayW,
        ["_keyDownDelayW"] = _keyDownDelayW,
        ["_threadDelayW"] = _threadDelayW,

        ["_keyUpDelayE"] = _keyUpDelayE,
        ["_keyDownDelayE"] = _keyDownDelayE,
        ["_threadDelayE"] = _threadDelayE,
      };
      return ret;
    }
    //---------------------------------------------------------------------------------------
    public void LoadSettings(JObject settings)
    {
      _QEnable = Utils.Get(settings, "_q", true);
      _WEnable = Utils.Get(settings, "_w", false);
      _EEnable = Utils.Get(settings, "_e", false);

      _keyUpDelayQ = Utils.Get(settings, "_keyUpDelayQ", 10);
      _keyDownDelayQ = Utils.Get(settings, "_keyDownDelayQ", 10);
      _threadDelayQ = Utils.Get(settings, "_threadDelayQ", 10);

      _keyUpDelayW = Utils.Get(settings, "_keyUpDelayW", 10);
      _keyDownDelayW = Utils.Get(settings, "_keyDownDelayW", 10);
      _threadDelayW = Utils.Get(settings, "_threadDelayW", 10);

      _keyUpDelayE = Utils.Get(settings, "_keyUpDelayE", 10);
      _keyDownDelayE = Utils.Get(settings, "_keyDownDelayE", 10);
      _threadDelayE = Utils.Get(settings, "_threadDelayE", 10);
    }
  }
}
