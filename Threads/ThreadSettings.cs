using MXTools.Properties;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MXTools.Threads
{
  public class MouseThreadSettings
  {
    public bool Auto { get; set; }
    public int ClickDelay { get; set; }
    public int ThreadDelay { get; set; }

    public MouseThreadSettings()
    {
      Auto = true;
      ClickDelay = 100;
      ThreadDelay = 50;
    }
  }

  public class QWMemThreadSettings
  {
    public bool Auto { get; set; }
    public bool AutoKey { get; set; }
    public int KeyUpDelay { get; set; }
    public int KeyDownDelay { get; set; }
    public int KeyThreadDelay { get; set; }
    public int MemThreadDelay { get; set; }
    public int WarnThreadDelay { get; set; }
    public float Scale { get; set; }
    public float WarnScale { get; set; }
    public float WarnVolume { get; set; }

    public QWMemThreadSettings()
    {
      Auto = true;
      AutoKey = true;
      KeyUpDelay = 32;
      KeyDownDelay = 32;
      KeyThreadDelay = 32;
      MemThreadDelay = 32;
      WarnThreadDelay = 32;
      Scale = 0.99f;
      WarnScale = 0.96f;
      WarnVolume = 0.5f;
    }
  }

  public class QWEThreadSettings
  {
    public bool Q { get; set; }
    public bool W { get; set; }
    public bool E { get; set; }
    public int KeyUpDelayQ { get; set; }
    public int KeyDownDelayQ { get; set; }
    public int ThreadDelayQ { get; set; }
    public int KeyUpDelayW { get; set; }
    public int KeyDownDelayW { get; set; }
    public int ThreadDelayW { get; set; }
    public int KeyUpDelayE { get; set; }
    public int KeyDownDelayE { get; set; }
    public int ThreadDelayE { get; set; }

    public QWEThreadSettings()
    {
      Q = true;
      W = false;
      E = false;

      KeyUpDelayQ = 32;
      KeyDownDelayQ = 32;
      ThreadDelayQ = 32;

      KeyUpDelayW = 28;
      KeyDownDelayW = 28;
      ThreadDelayW = 28;

      KeyUpDelayE = 30;
      KeyDownDelayE = 30;
      ThreadDelayE = 30;
    }
  }

  public class TimeWarningSettings
  {
    public bool Auto { get; set; }
    public float Interval { get; set; }
    public int TimerInterval { get; set; }
    public int Duration { get; set; }
    public float Volume { get; set; }
    public string Dismiss { get; set; }

    public TimeWarningSettings()
    {
      Auto = true;

      //in minutes
      Interval = 3.0f;

      //20 seconds
      Duration = 30;

      Volume = 0.7f;

      //1 seconds
      TimerInterval = 300;

      Dismiss = "56S";
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
        WarnScale = 0,
        WarnVolume = 0,
        Scale = 0.9f
      };

      QWE = new QWEThreadSettings() { };

      M = new MouseThreadSettings() { };

      T = new TimeWarningSettings() { };
    }
    //----------------------------------------------------------------------------------
    private static string GetDataFile()
    {
      string exeDirectory = Path.GetDirectoryName(Environment.ProcessPath);
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
        Q.Auto = other.Q.Auto;
        Q.AutoKey = other.Q.AutoKey;

        Q.KeyUpDelay = other.Q.KeyUpDelay;
        Q.KeyDownDelay = other.Q.KeyDownDelay;
        Q.KeyThreadDelay = other.Q.KeyThreadDelay;
        Q.MemThreadDelay = other.Q.MemThreadDelay;
        Q.WarnThreadDelay = other.Q.WarnThreadDelay;
        Q.Scale = other.Q.Scale;
        Q.WarnScale = other.Q.WarnScale;
        Q.WarnVolume = other.Q.WarnVolume;
      }

      if (other.W != null)
      {
        W.Auto = other.W.Auto;
        W.AutoKey = other.W.AutoKey;

        W.KeyUpDelay = other.W.KeyUpDelay;
        W.KeyDownDelay = other.W.KeyDownDelay;
        W.KeyThreadDelay = other.W.KeyThreadDelay;
        W.MemThreadDelay = other.W.MemThreadDelay;
        W.WarnThreadDelay = other.W.WarnThreadDelay;
        W.Scale = other.W.Scale;
        W.WarnScale = other.W.WarnScale;
        W.WarnVolume = other.W.WarnVolume;
      }

      if (other.QWE != null)
      {
        QWE.Q = other.QWE.Q;
        QWE.W = other.QWE.W;
        QWE.E = other.QWE.E;

        QWE.KeyUpDelayQ = other.QWE.KeyUpDelayQ;
        QWE.KeyDownDelayQ = other.QWE.KeyDownDelayQ;
        QWE.ThreadDelayQ = other.QWE.ThreadDelayQ;

        QWE.KeyUpDelayW = other.QWE.KeyUpDelayW;
        QWE.KeyDownDelayW = other.QWE.KeyDownDelayW;
        QWE.ThreadDelayW = other.QWE.ThreadDelayW;

        QWE.KeyUpDelayE = other.QWE.KeyUpDelayE;
        QWE.KeyDownDelayE = other.QWE.KeyDownDelayE;
        QWE.ThreadDelayE = other.QWE.ThreadDelayE;
      }

      if (other.M != null)
      {
        M.ClickDelay = other.M.ClickDelay;
        M.ThreadDelay = other.M.ThreadDelay;
        M.Auto = other.M.Auto;
      }

      if (other.T != null)
      {
        T.Auto = other.T.Auto;
        T.Interval = other.T.Interval;
        T.Duration = other.T.Duration;
        T.Volume = other.T.Volume;
        T.TimerInterval = other.T.TimerInterval;
        T.Dismiss = other.T.Dismiss;
      }
    }
  }
}
