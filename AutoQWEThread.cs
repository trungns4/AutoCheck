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
using AutoCheck.Properties;
using Binarysharp.MemoryManagement;
using log4net;
using Newtonsoft.Json.Linq;
using WindowsInput;

namespace AutoCheck
{
  internal class AutoQWEThread
  {
    private Thread _qThread;
    private Thread _wThread;
    private Thread _eThread;

    private bool _isRunning = false;
    private System.Windows.Forms.Label _label;
    private long _count = 0;

    private QWEThreadSettings _settings;

    public AutoQWEThread(QWEThreadSettings settings, System.Windows.Forms.Label label)
    {
      _label = label;
      _settings = settings;
    }
    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
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

      Thread.Sleep(50);

      _wThread = new Thread(() => Run('w'));
      _wThread.IsBackground = true;
      _wThread.Priority = ThreadPriority.Normal;
      _wThread.Start();

      Thread.Sleep(50);

      _eThread = new Thread(() => Run('e'));
      _eThread.IsBackground = true;
      _eThread.Priority = ThreadPriority.Normal;
      _eThread.Start();

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.InfoFormat("Auto Key Threads started");
      log.InfoFormat($"Q: {_settings._q} W: {_settings._w} E: {_settings._e}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public void Stop()
    {
      _isRunning = false;
      Thread.Sleep(100);
      _count = 0;

      if (_qThread != null && _qThread.IsAlive)
      {
        _qThread.Join();
        _qThread = null;
      }

      if (_wThread != null && _wThread.IsAlive)
      {
        _wThread.Join();
        _wThread = null;
      }

      if (_eThread != null && _eThread.IsAlive)
      {
        _eThread.Join();
        _eThread = null;
      }

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.DebugFormat("Auto Key Threads stopped");
    }
    //---------------------------------------------------------------------------------------
    private void UpdateLabel()
    {
      if (_label.InvokeRequired)
        _label.BeginInvoke((System.Windows.Forms.MethodInvoker)(() => _label.Text = _count.ToString()));
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
          Thread.Sleep(_settings._threadDelayQ);
          continue;
        }

        var si = Input.Simulator;

        if (key == 'q')
        {
          if (_settings._q)
          {
            si.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Q);
            Thread.Sleep(_settings._keyDownDelayQ);

            si.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_Q);
            UpdateUIAndSleep(_settings._keyUpDelayQ);
          }
        }
        else if (key == 'w')
        {
          if (_settings._w)
          {
            si.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
            Thread.Sleep(_settings._keyDownDelayW);

            si.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
            UpdateUIAndSleep(_settings._keyUpDelayW);
          }
        }
        else if (key == 'e')
        {
          if (_settings._e)
          {
            si.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_E);
            Thread.Sleep(_settings._keyDownDelayE);

            si.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_E);
            UpdateUIAndSleep(_settings._keyUpDelayE);
          }
        }

        if (_settings._q == false && _settings._w == false && _settings._e == false)
        {
          UpdateLabel();
          Thread.Sleep(Math.Min(_settings._threadDelayQ, Math.Min(_settings._threadDelayW, _settings._threadDelayE)));
        }
      }
    }
  }
}
