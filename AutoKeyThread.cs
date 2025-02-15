using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using log4net;
using NAudio.Wave;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
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
    private ManualResetEvent _flag = new ManualResetEvent(false);

    private IValuesDisplay _ValuesDisplay;

    private Thread _thread;
    private Thread _keyThread;
    private Thread _warnThread;

    private bool _isRunning = false;
    private bool _full = false;

    private MemorySharp _sharp;
    private bool _auto;

    //current
    private int _curVal = 0;
    private long _curAdr = 0;

    //max
    private int _maxVal = 0;
    private long _maxAdr = 0;

    //key to stroke
    private char _key;
    private double _scale = 1;

    //warning
    private double _warnScale = 0;
    private float _warnVolume = 1.0f;
    private string _warnSound = "alarm.mp3";

    private WaveOutEvent m_outputDevice;
    private AudioFileReader m_audioFile;

    private int _keyDelay = 8;
    private int _memThreadDelay = 10;
    private int _warnThreadDelay = 10;

    private int _keyThreadOptTime = 100;

    private InputSimulator _is = new InputSimulator();
    VirtualKeyCode _keyCode;

    public AutoKeyThread(char key, IValuesDisplay displayBox)
    {
      _auto = false;
      _sharp = null;
      _ValuesDisplay = displayBox;
      _key = key;
      _keyCode = Utils.KeyCode(_key);
    }
    //---------------------------------------------------------------------------------------
    public bool Start(MemorySharp sharp, long curAddr, long maxAddr)
    {
      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      _sharp = sharp;
      if (curAddr == 0 || maxAddr == 0)
      {
        log.FatalFormat($"Address for {_key} is not correct");
        return false;
      }

      _curAdr = curAddr;
      _maxAdr = maxAddr;

      //if this thread needs warning ==> load mp3 player
      if (_warnScale != 0)
      {
        string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string file = Path.Combine(exeDirectory, _warnSound);
        m_outputDevice = new WaveOutEvent();
        m_audioFile = new AudioFileReader(file)
        {
          Volume = 1.0f
        };
        m_outputDevice.Init(m_audioFile);
      }

      _curVal = 0;
      _maxVal = 0;

      _isRunning = true;
      _thread = new Thread(RunMemoryCheck);
      _thread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _thread.Start();

      _keyThread = new Thread(RunKeyCheck);
      _keyThread.IsBackground = true;
      _thread.Priority = ThreadPriority.Highest;
      _keyThread.Start();

      _warnThread = new Thread(RunWarnCheck);
      _warnThread.IsBackground = true;
      _warnThread.Start();

      log.Info("Monitoring Thread started");

      log.InfoFormat($"Key: {_key}");
      log.InfoFormat($"Key Delay: {_keyDelay}");
      log.InfoFormat($"Key Operation Time: {_keyThreadOptTime}");
      log.InfoFormat($"Mem Thread Delay: {_memThreadDelay}");
      log.InfoFormat($"Warn Thread Delay: {_warnThreadDelay}");

      log.InfoFormat($"Scale: {_scale}");
      log.InfoFormat($"Warn Scale: {_warnScale}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public bool Stop()
    {
      _isRunning = false;

      if (_thread != null && _thread.IsAlive)
      {
        _thread.Join();
      }

      //to stop the key thread
      _flag.Set();

      Thread.Sleep(10);

      if (_keyThread != null && _keyThread.IsAlive)
      {
        _keyThread.Join();
      }

      Thread.Sleep(10);

      if (_warnThread != null && _warnThread.IsAlive)
      {
        _warnThread.Join();
      }

      if (m_outputDevice != null)
      {
        m_outputDevice.Dispose();
        m_audioFile.Dispose();

        m_outputDevice = null;
        m_audioFile = null;
      }

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
    public bool Auto
    {
      get { return _auto; }
      set { _auto = value; }
    }
    //---------------------------------------------------------------------------------------
    public double Scale
    {
      get { return _scale; }
      set { _scale = value; }
    }
    //---------------------------------------------------------------------------------------
    public double WarnScale
    {
      get { return _warnScale; }
      set { _warnScale = value; }
    }
    //---------------------------------------------------------------------------------------
    public float WarnVolume
    {
      get { return _warnVolume; }
      set { _warnVolume = Math.Min(value, 1.0f); }
    }
    //---------------------------------------------------------------------------------------
    public string WarnSound
    {
      get { return _warnSound; }
      set { _warnSound = value; }
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
    public int KeyDelay
    {
      get
      {
        return _keyDelay;
      }
      set
      {
        _keyDelay = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public int MemThreadDelay
    {
      get
      {
        return _memThreadDelay;
      }
      set
      {
        _memThreadDelay = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public int WarnThreadDelay
    {
      get
      {
        return _warnThreadDelay;
      }
      set
      {
        _warnThreadDelay = value;
      }
    }
    //---------------------------------------------------------------------------------------
    public int KeyThreadOptTime
    {
      get
      {
        return _keyThreadOptTime;
      }
      set
      {
        _keyThreadOptTime = value;
      }
    }
    //---------------------------------------------------------------------------------------
    private void PlayAlarm(bool play)
    {
      if (play == true)
      {
        if (m_outputDevice != null && m_outputDevice.PlaybackState != PlaybackState.Playing)
        {
          m_outputDevice.Volume = _warnVolume;

          if ((m_audioFile.TotalTime - m_audioFile.CurrentTime).TotalMilliseconds <= 100)
          {
            m_audioFile.CurrentTime = new TimeSpan();
          }
          m_outputDevice.Play();
        }
      }
      else
      {
        if (m_outputDevice != null && m_outputDevice.PlaybackState == PlaybackState.Playing)
        {
          m_outputDevice.Stop();
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void CheckWarning()
    {
      if (_warnScale != 0)
      {
        double cv = _curVal;
        double mv = _maxVal;

        //warning
        if (cv <= (mv * _warnScale))
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
          AutoFlags.IsTargetWindowActive = Utils.IsWindowActive(_sharp.Pid);
          if (_curAdr != 0 && _maxAdr != 0 && _auto)
          {
            _curVal = _sharp.Read<int>((IntPtr)_curAdr, false);
            _maxVal = _sharp.Read<int>((IntPtr)_maxAdr, false);

            double cv = _curVal;
            double mv = _maxVal;

            //normal
            if (cv <= (mv * _scale))
            {
              _full = false;
              _flag.Set();
            }
            else
            {
              _full = true;
              _flag.Reset();
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
          Thread.Sleep(_memThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void RunKeyCheck()
    {
      while (_isRunning)
      {
        try
        {
          _flag.WaitOne();
          while (_auto == true && _isRunning == true && _full == false && AutoFlags.IsTargetWindowActive == true)
          {
            var sw = Stopwatch.StartNew();
            _is.Keyboard.KeyDown(_keyCode);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(_keyCode);
            Thread.Sleep(_keyDelay);

            int sleepTime = _keyThreadOptTime - (int)sw.ElapsedMilliseconds;
            if (sleepTime <= 0)
            {
              sleepTime = _keyDelay / 2;
            }
            Thread.Sleep(sleepTime);
          }
        }
        finally
        {
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
          Thread.Sleep(_warnThreadDelay);
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
