using Binarysharp.MemoryManagement;
using Binarysharp.MemoryManagement.Memory;
using log4net;
using NAudio.Wave;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;

namespace AutoCheck
{
  internal class AutoKeyThread
  {
    private ManualResetEvent _flag = new ManualResetEvent(false);

    private Thread _thread;
    private Thread _keyThread;
    private Thread _warnThread;

    private bool _isRunning = false;

    private bool _full = false;
    private TextBox _curAdrBox;
    private TextBox _curValBox;

    private TextBox _maxAdrBox;
    private TextBox _maxValBox;

    private MemorySharp _sharp;
    private bool _auto;

    private int _curVal = 0;
    private long _curAdr = 0;

    private int _maxVal = 0;
    private long _maxAdr = 0;

    private char _key;

    private double _scale = 1;

    private double _warnScale = 0;
    private float _warnVolume = 1.0f;
    private string _warnSound = "alarm.mp3";

    private WaveOutEvent m_outputDevice;
    private AudioFileReader m_audioFile;

    private int _keyDelay = 8;
    private int _keyThreadDelay = 10;
    private int _memThreadDelay = 10;
    private int _warnThreadDelay = 10;

    private int _keyThreadOptTime = 100;

    private InputSimulator _is = new InputSimulator();
    VirtualKeyCode _keyCode;

    public AutoKeyThread(char key,
      TextBox curAdrBox, TextBox curValBox,
      TextBox maxAdrBox, TextBox maxValBox)
    {
      _auto = false;
      _sharp = null;

      _key = key;
      _curAdrBox = curAdrBox;
      _curValBox = curValBox;

      _maxAdrBox = maxAdrBox;
      _maxValBox = maxValBox;

      _keyCode = Utils.KeyCode(_key);
    }

    //---------------------------------------------------------------------------------------
    public bool Start(MemorySharp sharp)
    {
      _sharp = sharp;
      if (long.TryParse(_curAdrBox.Text, NumberStyles.HexNumber,
                          CultureInfo.InvariantCulture, out long number))
      {
        _curAdr = number;
      }
      else
      {
        return false;
      }

      if (long.TryParse(_maxAdrBox.Text, NumberStyles.HexNumber,
                    CultureInfo.InvariantCulture, out long number2))
      {
        _maxAdr = number2;
      }
      else
      {
        return false;
      }

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

      ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
      log.Info("Monitoring Thread started");

      log.InfoFormat($"Key: {_key}");
      log.InfoFormat($"Key Delay: {_keyDelay}");
      log.InfoFormat($"Key Thread Delay: {_keyThreadDelay}");
      log.InfoFormat($"Mem Thread Delay: {_memThreadDelay}");
      log.InfoFormat($"Warn Thread Delay: {_warnThreadDelay}");
      log.InfoFormat($"Operation Time: {_keyThreadOptTime}");


      log.InfoFormat($"Scale: {_scale}");
      log.InfoFormat($"Warn Scale: {_warnScale}");

      return true;
    }
    //---------------------------------------------------------------------------------------
    public bool Stop()
    {
      _isRunning = false;
      _thread?.Join();
      _thread = null;

      //to stop the key thread
      _flag.Set();

      Thread.Sleep(10);

      _keyThread?.Join();
      _keyThread = null;

      Thread.Sleep(10);

      _warnThread?.Join();
      _warnThread = null;

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
    public int KeyThreadDelay
    {
      get
      {
        return _keyThreadDelay;
      }
      set
      {
        _keyThreadDelay = value;
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
          UpdateTexts(0, 0);
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
          while (_auto == true && _isRunning == true && _full == false && Utils.IsWindowActive(_sharp.Pid) == true)
          {
            var sw = Stopwatch.StartNew();
            _is.Keyboard.KeyDown(_keyCode);
            Thread.Sleep(_keyDelay);
            _is.Keyboard.KeyUp(_keyCode);
            while (sw.ElapsedMilliseconds < _keyThreadOptTime)
            {
              Thread.Sleep(10);
            }
          }
        }
        finally
        {
          Thread.Sleep(_keyThreadDelay);
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
          UpdateTexts(_curVal, _maxVal);
        }
        finally
        {
          Thread.Sleep(_warnThreadDelay);
        }
      }
    }
    //---------------------------------------------------------------------------------------
    private void UpdateTexts(int current, int max)
    {
      if (_curValBox.InvokeRequired)
      {
        _curValBox.BeginInvoke(new Action(() => _curValBox.Text = current.ToString()));
      }
      else
      {
        _curValBox.Text = current.ToString();
      }

      if (_maxValBox.InvokeRequired)
      {
        _maxValBox.BeginInvoke(new Action(() => _maxValBox.Text = max.ToString()));
      }
      else
      {
        _maxValBox.Text = max.ToString();
      }
    }
  }
}
