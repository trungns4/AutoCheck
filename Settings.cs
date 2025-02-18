using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using AutoCheck.Properties;
using NAudio.Gui;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Runtime;

namespace AutoCheck
{
  public class QWMemThreadSettings
  {
    public bool _auto { get; set; }
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
      _keyUpDelay = 16;
      _keyDownDelay = 16;
      _keyThreadDelay = 32;
      _memThreadDelay = 16;
      _warnThreadDelay = 16;
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

      _keyUpDelayQ = 16;
      _keyDownDelayQ = 16;
      _threadDelayQ = 16;

      _keyUpDelayW = 16;
      _keyDownDelayW = 16;
      _threadDelayW = 16;

      _keyUpDelayE = 16;
      _keyDownDelayE = 16;
      _threadDelayE = 16;
    }
  }

  public class Settings
  {
    public QWMemThreadSettings Q { get; set; }
    public QWMemThreadSettings W { get; set; }
    public QWEThreadSettings QWE { get; set; }

    public Settings()
    {
      Q = new QWMemThreadSettings()
      {

      };

      W = new QWMemThreadSettings()
      {
        _warnScale = 0,
        _warnVolume = 0,
        _scale = 0.9f
      };

      QWE = new QWEThreadSettings()
      {

      };
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
        Settings tempSettings = JsonConvert.DeserializeObject<Settings>(json);

        if (tempSettings != null)
        {
          // Update complex nested objects by copying values
          if (tempSettings.Q != null)
          {
            Q._auto = tempSettings.Q._auto;
            Q._keyUpDelay = tempSettings.Q._keyUpDelay;
            Q._keyDownDelay = tempSettings.Q._keyDownDelay;
            Q._keyThreadDelay = tempSettings.Q._keyThreadDelay;
            Q._memThreadDelay = tempSettings.Q._memThreadDelay;
            Q._warnThreadDelay = tempSettings.Q._warnThreadDelay;
            Q._scale = tempSettings.Q._scale;
            Q._warnScale = tempSettings.Q._warnScale;
            Q._warnVolume = tempSettings.Q._warnVolume;
          }

          if (tempSettings.W != null)
          {
            W._auto = tempSettings.W._auto;
            W._keyUpDelay = tempSettings.W._keyUpDelay;
            W._keyDownDelay = tempSettings.W._keyDownDelay;
            W._keyThreadDelay = tempSettings.W._keyThreadDelay;
            W._memThreadDelay = tempSettings.W._memThreadDelay;
            W._warnThreadDelay = tempSettings.W._warnThreadDelay;
            W._scale = tempSettings.W._scale;
            W._warnScale = tempSettings.W._warnScale;
            W._warnVolume = tempSettings.W._warnVolume;
          }

          if (tempSettings.QWE != null)
          {
            QWE._q = tempSettings.QWE._q;
            QWE._w = tempSettings.QWE._w;
            QWE._e = tempSettings.QWE._e;

            QWE._keyUpDelayQ = tempSettings.QWE._keyUpDelayQ;
            QWE._keyDownDelayQ = tempSettings.QWE._keyDownDelayQ;
            QWE._threadDelayQ = tempSettings.QWE._threadDelayQ;

            QWE._keyUpDelayW = tempSettings.QWE._keyUpDelayW;
            QWE._keyDownDelayW = tempSettings.QWE._keyDownDelayW;
            QWE._threadDelayW = tempSettings.QWE._threadDelayW;

            QWE._keyUpDelayE = tempSettings.QWE._keyUpDelayE;
            QWE._keyDownDelayE = tempSettings.QWE._keyDownDelayE;
            QWE._threadDelayE = tempSettings.QWE._threadDelayE;
          }
        }

        return true;
      }
      catch
      {
        MessageBox.Show("Load data failed", Resources.MsgBoxCaption);
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
        MessageBox.Show("Load data failed", Resources.MsgBoxCaption);
      }
    }
    //----------------------------------------------------------------------------------
    public void CopyFrom(Settings other)
    {
      if (other == null) return;

      // Copy complex nested objects
      if (other.Q != null)
      {
        Q._auto = other.Q._auto;
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
    }
  }
}
