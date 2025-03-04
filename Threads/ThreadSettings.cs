using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MXTools.Properties;
using NAudio.Gui;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Runtime;

namespace MXTools.Threads
{
  public class MouseThreadSettings
  {
    public bool _auto { get; set; }
    public int _clickDelay { get; set; }
    public int _threadDelay { get; set; }

    public MouseThreadSettings()
    {
      _auto = true;
      _clickDelay = 50;
      _threadDelay = 50;
    }
  }

  public class QWMemThreadSettings
  {
    public bool _auto { get; set; }
    public bool _autoKey { get; set; }
    public int _keyUpDelay { get; set; }
    public int _keyDownDelay { get; set; }
    public int _keyThreadDelay { get; set; }
    public int _memThreadDelay { get; set; }
    public int _warnThreadDelay { get; set; }
    public float _scale { get; set; }
    public float _warnScale { get; set; }
    public float _warnVolume { get; set; }

    public QWMemThreadSettings()
    {
      _auto = true;
      _autoKey = true;
      _keyUpDelay = 32;
      _keyDownDelay = 32;
      _keyThreadDelay = 32;
      _memThreadDelay = 32;
      _warnThreadDelay = 32;
      _scale = 0.99f;
      _warnScale = 0.96f;
      _warnVolume = 0.5f;
    }
  }

  public class QWEThreadSettings
  {
    public bool _q { get; set; }
    public bool _w { get; set; }
    public bool _e { get; set; }
    public int _keyUpDelayQ { get; set; }
    public int _keyDownDelayQ { get; set; }
    public int _threadDelayQ { get; set; }
    public int _keyUpDelayW { get; set; }
    public int _keyDownDelayW { get; set; }
    public int _threadDelayW { get; set; }
    public int _keyUpDelayE { get; set; }
    public int _keyDownDelayE { get; set; }
    public int _threadDelayE { get; set; }

    public QWEThreadSettings()
    {
      _q = true;
      _w = false;
      _e = false;

      _keyUpDelayQ = 32;
      _keyDownDelayQ = 32;
      _threadDelayQ = 32;

      _keyUpDelayW = 28;
      _keyDownDelayW = 28;
      _threadDelayW = 28;

      _keyUpDelayE = 30;
      _keyDownDelayE = 30;
      _threadDelayE = 30;
    }
  }

  public class TimeWarningSettings
  {
    public bool _auto { get; set; }
    public float _interval { get; set; }
    public int _timerInterval { get; set; }
    public int _duration { get; set; }
    public float _volume { get; set; }
    public string _dismiss { get; set; }

    public TimeWarningSettings()
    {
      _auto = true;

      //in minutes
      _interval = 3.0f;

      //20 seconds
      _duration = 30;

      _volume = 0.7f;

      //1 seconds
      _timerInterval = 300;

      _dismiss = "56S";
    }
  }

  public class ThreadSettings
  {
    public QWMemThreadSettings Q { get; }
    public QWMemThreadSettings W { get; }
    public QWEThreadSettings QWE { get; }
    public MouseThreadSettings M { get; }
    public TimeWarningSettings T { get; }

    public ThreadSettings()
    {
      Q = new QWMemThreadSettings() { };

      W = new QWMemThreadSettings()
      {
        _warnScale = 0,
        _warnVolume = 0,
        _scale = 0.9f
      };

      QWE = new QWEThreadSettings() { };

      M = new MouseThreadSettings() { };

      T = new TimeWarningSettings() { };
    }
    //----------------------------------------------------------------------------------
    private static string GetDataFile()
    {
      string exeDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      return Path.Combine(exeDirectory, "data.json");
    }
    //----------------------------------------------------------------------------------
    public bool LoadData()
    {
      try
      {
        var file = GetDataFile();
        if (!File.Exists(file))
        {
          return false;
        }

        string json = File.ReadAllText(file);

        // Deserialize into a temporary object
        ThreadSettings tempSettings = JsonConvert.DeserializeObject<ThreadSettings>(json);

        CopyFrom(tempSettings);

        return true;
      }
      catch
      {
        MessageBox.Show("Load data failed", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        return false;
      }
    }
    //----------------------------------------------------------------------------------
    public void SaveData()
    {
      try
      {
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(GetDataFile(), json);
      }
      catch
      {
        MessageBox.Show("Load data failed", Resources.MsgBoxCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
    //----------------------------------------------------------------------------------
    public void CopyFrom(ThreadSettings other)
    {
      if (other == null) return;

      // Copy complex nested objects
      if (other.Q != null)
      {
        Q._auto = other.Q._auto;
        Q._autoKey = other.Q._autoKey;

        Q._keyUpDelay = other.Q._keyUpDelay;
        Q._keyDownDelay = other.Q._keyDownDelay;
        Q._keyThreadDelay = other.Q._keyThreadDelay;
        Q._memThreadDelay = other.Q._memThreadDelay;
        Q._warnThreadDelay = other.Q._warnThreadDelay;
        Q._scale = other.Q._scale;
        Q._warnScale = other.Q._warnScale;
        Q._warnVolume = other.Q._warnVolume;
      }

      if (other.W != null)
      {
        W._auto = other.W._auto;
        W._autoKey = other.W._autoKey;

        W._keyUpDelay = other.W._keyUpDelay;
        W._keyDownDelay = other.W._keyDownDelay;
        W._keyThreadDelay = other.W._keyThreadDelay;
        W._memThreadDelay = other.W._memThreadDelay;
        W._warnThreadDelay = other.W._warnThreadDelay;
        W._scale = other.W._scale;
        W._warnScale = other.W._warnScale;
        W._warnVolume = other.W._warnVolume;
      }

      if (other.QWE != null)
      {
        QWE._q = other.QWE._q;
        QWE._w = other.QWE._w;
        QWE._e = other.QWE._e;

        QWE._keyUpDelayQ = other.QWE._keyUpDelayQ;
        QWE._keyDownDelayQ = other.QWE._keyDownDelayQ;
        QWE._threadDelayQ = other.QWE._threadDelayQ;

        QWE._keyUpDelayW = other.QWE._keyUpDelayW;
        QWE._keyDownDelayW = other.QWE._keyDownDelayW;
        QWE._threadDelayW = other.QWE._threadDelayW;

        QWE._keyUpDelayE = other.QWE._keyUpDelayE;
        QWE._keyDownDelayE = other.QWE._keyDownDelayE;
        QWE._threadDelayE = other.QWE._threadDelayE;
      }

      if (other.M != null)
      {
        M._clickDelay = other.M._clickDelay;
        M._threadDelay = other.M._threadDelay;
        M._auto = other.M._auto;
      }

      if (other.T != null)
      {
        T._auto = other.T._auto;
        T._interval = other.T._interval;
        T._duration = other.T._duration;
        T._volume = other.T._volume;
        T._timerInterval = other.T._timerInterval;
        T._dismiss = other.T._dismiss;
      }
    }
  }
}
