using AutoCheck.Properties;
using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using log4net;
using NAudio.Wave;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using static System.Net.Mime.MediaTypeNames;

namespace AutoCheck
{
  interface IValuesDisplay
  {
    int MaxValue { get; set; }
    int CurValue { get; set; }

    IAsyncResult BeginInvoke(Delegate method, params object[] args);
    bool InvokeRequired { get; }
  }

  internal class AutoKeyThread
  {
    private char _key;
    private QWMemThreadSettings _settings;
    private MemorySharp _sharp;

    //current
    private int _curVal = 0;
    private long _curAdr = 0;

    //max
    private int _maxVal = 0;
    private long _maxAdr = 0;

    private Thread _thread;
    private Thread _keyThread;
    private Thread _warnThread;
    private ManualResetEvent _keyFlag = new ManualResetEvent(false);

    private IValuesDisplay _ValuesDisplay;

    private bool _isRunning = false;
    private bool _full = false;
    private SoundPlayer _player;
    private VirtualKeyCode _keyCode;

    //---------------------------------------------------------------------------------------
    public AutoKeyThread(char key, QWMemThreadSettings settings, IValuesDisplay displayBox)
    {
      _sharp = null;
      _ValuesDisplay = displayBox;
      _key = key;
      _keyCode = Utils.KeyCode(_key);
      _settings = settings;
      _player = new SoundPlayer("alarm2.mp3", _settings._warnVolume);
    }
    //---------------------------------------------------------------------------------------
    public bool Start(MemorySharp sharp, long curAddr, long maxAddr)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      _sharp = sharp;
      _curAdr = curAddr;
      _maxAdr = maxAddr;

      if (_settings._warnScale != 0)
      {
        _player.Volume = _settings._warnVolume;
      }
      else
      {
        _player.Volume = 0;
      }

      _curVal = 0;
      _maxVal = 0;

      _isRunning = true;
      _thread = new Thread(RunMemoryCheck);
      _thread.IsBackground = true;
      _thread.Priority = ThreadPriority.Normal;
      _thread.Start();

      Thread.Sleep(50);

      _keyThread = new Thread(RunKeyCheck);
      _keyThread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _keyThread.Start();

      _warnThread = new Thread(RunWarnCheck);
      _warnThread.IsBackground = true;
      _warnThread.Start();

      log.Info("Monitoring Thread started");

      log.InfoFormat($"Key: {_key}");
      log.InfoFormat($"Key Up Delay: {_settings._keyUpDelay}");
      log.InfoFormat($"Key Down Delay: {_settings._keyDownDelay}");
      log.InfoFormat($"Key Thread Delay: {_settings._keyThreadDelay}");
      log.InfoFormat($"Mem Thread Delay: {_settings._memThreadDelay}");
      log.InfoFormat($"Warn Thread Delay: {_settings._warnThreadDelay}");

      log.InfoFormat($"Scale: {_settings._scale}");
      log.InfoFormat($"Warn Scale: {_settings._warnScale}");
      log.InfoFormat($"Warn Volume: {_settings._warnVolume:0.00}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public bool Stop()
    {
      _isRunning = false;

      if (_thread != null && _thread.IsAlive)
      {
        _thread.Join();
        _thread = null;
      }

      //to stop the key thread
      _keyFlag.Set();

      Thread.Sleep(10);

      if (_keyThread != null && _keyThread.IsAlive)
      {
        _keyThread.Join();
        _keyThread = null;
      }

      Thread.Sleep(10);

      if (_warnThread != null && _warnThread.IsAlive)
      {
        _warnThread.Join();
        _warnThread = null;
      }

      _player.Stop();

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.Info($"Thread {_key} stopped");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public bool IsRunning()
    {
      return _isRunning;
    }
    //---------------------------------------------------------------------------------------
    public string Value
    {
      get
      {
        return string.Format("{0}", _curVal);
      }
    }
    //---------------------------------------------------------------------------------------
    private void PlayAlarm(bool play)
    {
      if (play)
      {
        Task.Run(() => _player.Play(TimeSpan.FromSeconds(10000)));
      }
      else
      {
        _player.Stop();
      }
    }
    //---------------------------------------------------------------------------------------
    private void CheckWarning()
    {
      if (_settings._warnScale != 0)
      {
        double cv = _curVal;
        double mv = _maxVal;

        //warning
        if (cv <= (mv * _settings._warnScale))
        {
          if (cv != 0)
          {
            PlayAlarm(true);
          }
        }
        else
        {
          PlayAlarm(false);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunMemoryCheck()
    {
      while (_isRunning)
      {
        try
        {
          if (_key == 'q')
          {
            AutoFlags.IsTargetWindowActive = Utils.IsWindowActive(_sharp.Pid);
          }
          if (_curAdr >= 24 && _maxAdr >= 24 && _settings._auto)
          {
            _curVal = _sharp.Read<int>((IntPtr)_curAdr, false);
            _maxVal = _sharp.Read<int>((IntPtr)_maxAdr, false);

            double cv = _curVal;
            double mv = _maxVal;


            if (cv <= (mv * _settings._scale))
            {
              _full = false;
              _keyFlag.Set();
            }
            else
            {
              _full = true;
              _keyFlag.Reset();
            }
          }
        }
        catch
        {
          _curVal = 0;
          _maxVal = 0;
          DisplayValues(0, 0);
        }
        finally
        {
          Thread.Sleep(_settings._memThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunKeyCheck()
    {
      while (_isRunning)
      {
        _keyFlag.WaitOne();
        bool delay = true;

        var si = Input.Simulator;

        while (_settings._auto && _settings._autoKey == true
        && _isRunning == true && _full == false && AutoFlags.IsTargetWindowActive == true)
        {
          si.Keyboard.KeyDown(_keyCode);
          Thread.Sleep(_settings._keyUpDelay);

          si.Keyboard.KeyUp(_keyCode);
          Thread.Sleep(_settings._keyDownDelay);

          delay = false;
        }
        if (delay == true)
        {
          Thread.Sleep(_settings._keyThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunWarnCheck()
    {
      while (_isRunning)
      {
        try
        {
          CheckWarning();
          DisplayValues(_curVal, _maxVal);
        }
        finally
        {
          Thread.Sleep(_settings._warnThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void DisplayValues(int current, int max)
    {
      if (_ValuesDisplay.InvokeRequired)
      {
        _ValuesDisplay.BeginInvoke(new Action(() =>
        {
          _ValuesDisplay.MaxValue = max;
          _ValuesDisplay.CurValue = current;
        }));
      }
      else
      {
        _ValuesDisplay.MaxValue = max;
        _ValuesDisplay.CurValue = current;
      }
    }
  }
}
