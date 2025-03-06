using MXTools.Properties;
using MXTools.Threads;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MXTools
{
  public partial class SettingsForm : Form
  {
    private ThreadSettings _settings;

    private bool _saved = true;

    public SettingsForm(ThreadSettings settings)
    {
      InitializeComponent();

      _settings = new ThreadSettings();
      _settings.CopyFrom(settings);

      ShowData();

      foreach (var numInput in CollectControls<NumericUpDown>(this))
      {
        numInput.ValueChanged += Input_ValueChanged;
      }

      foreach (var checkBox in CollectControls<CheckBox>(this))
      {
        checkBox.CheckedChanged += Input_ValueChanged;
      }

      foreach (var checkBox in CollectControls<TextBox>(this))
      {
        checkBox.TextChanged += Input_ValueChanged;
      }
    }
    //----------------------------------------------------------------------------------
    public ThreadSettings Settings
    {
      get { return _settings; }
    }

    //----------------------------------------------------------------------------------
    public List<T> CollectControls<T>(Control parentControl) where T : Control
    {
      List<T> collectedControls = new List<T>();

      foreach (Control control in parentControl.Controls)
      {
        // If the control is of type T, add it to the list
        if (control is T controlOfType)
        {
          collectedControls.Add(controlOfType);
        }

        // If the control has child controls, recurse through them
        if (control.HasChildren)
        {
          collectedControls.AddRange(CollectControls<T>(control));
        }
      }

      return collectedControls;
    }
    //----------------------------------------------------------------------------------
    private void Input_ValueChanged(object sender, EventArgs e)
    {
      _saved = false;
    }

    //----------------------------------------------------------------------------------
    private void ShowData()
    {
      m_QAuto.Checked = _settings.Q.Auto;
      m_QAutoKey.Checked = _settings.Q.AutoKey;
      m_QKeyUpDelay.Value = _settings.Q.KeyUpDelay;
      m_QKeyDownDelay.Value = _settings.Q.KeyDownDelay;
      m_QKeyThreadDelay.Value = _settings.Q.KeyThreadDelay;
      m_QMemThreadDelay.Value = _settings.Q.MemThreadDelay;
      m_QWarnThreadDelay.Value = _settings.Q.WarnThreadDelay;
      m_QScale.Value = (decimal)(_settings.Q.Scale * 100);
      m_QWarnScale.Value = (decimal)(_settings.Q.WarnScale * 100);
      m_QWarnVolume.Value = (decimal)(_settings.Q.WarnVolume * 100);

      m_WAuto.Checked = _settings.W.Auto;
      m_WAutoKey.Checked = _settings.W.AutoKey;
      m_WKeyUpDelay.Value = _settings.W.KeyUpDelay;
      m_WKeyDownDelay.Value = _settings.W.KeyDownDelay;
      m_WKeyThreadDelay.Value = _settings.W.KeyThreadDelay;
      m_WMemThreadDelay.Value = _settings.W.MemThreadDelay;
      m_WWarnThreadDelay.Value = _settings.W.WarnThreadDelay;
      m_WScale.Value = (decimal)(_settings.W.Scale * 100);
      m_WWarnScale.Value = (decimal)(_settings.W.WarnScale * 100);
      m_WWarnVolume.Value = (decimal)(_settings.W.WarnVolume * 100);

      _qweQKeyAuto.Checked = _settings.QWE.Q;
      _qweWKeyAuto.Checked = _settings.QWE.W;
      _qweEKeyAuto.Checked = _settings.QWE.E;

      _qweQKeyUpDelay.Value = _settings.QWE.KeyUpDelayQ;
      _qweQKeyDownDelay.Value = _settings.QWE.KeyDownDelayQ;
      _qweQKeyThreadDelay.Value = _settings.QWE.ThreadDelayQ;

      _qweWKeyUpDelay.Value = _settings.QWE.KeyUpDelayW;
      _qweWKeyDownDelay.Value = _settings.QWE.KeyDownDelayW;
      _qweWKeyThreadDelay.Value = _settings.QWE.ThreadDelayW;

      _qweEKeyUpDelay.Value = _settings.QWE.KeyUpDelayE;
      _qweEKeyDownDelay.Value = _settings.QWE.KeyDownDelayE;
      _qweEKeyThreadDelay.Value = _settings.QWE.ThreadDelayE;

      _MRClick.Checked = _settings.M.Auto;
      _MRThreadDelay.Value = _settings.M.ThreadDelay;
      _MRClickDelay.Value = _settings.M.ClickDelay;

      _WarnAuto.Checked = _settings.T.Auto;
      _WarnInterval.Value = (decimal)_settings.T.Interval;
      _WarnDuration.Value = (decimal)_settings.T.Duration;
      _WarnVolume.Value = (decimal)_settings.T.Volume * 100;
      _WarnTimer.Value = (decimal)_settings.T.TimerInterval;
      _WarnDismiss.Text = _settings.T.Dismiss;
    }
    //----------------------------------------------------------------------------------
    private void UIToData()
    {
      // Save Q settings
      _settings.Q.Auto = m_QAuto.Checked;
      _settings.Q.AutoKey = m_QAutoKey.Checked;
      _settings.Q.KeyUpDelay = (int)m_QKeyUpDelay.Value;
      _settings.Q.KeyDownDelay = (int)m_QKeyDownDelay.Value;
      _settings.Q.KeyThreadDelay = (int)m_QKeyThreadDelay.Value;
      _settings.Q.MemThreadDelay = (int)m_QMemThreadDelay.Value;
      _settings.Q.WarnThreadDelay = (int)m_QWarnThreadDelay.Value;
      _settings.Q.Scale = (float)(m_QScale.Value / 100);
      _settings.Q.WarnScale = (float)(m_QWarnScale.Value / 100);
      _settings.Q.WarnVolume = (float)(m_QWarnVolume.Value / 100);

      // Save W settings
      _settings.W.Auto = m_WAuto.Checked;
      _settings.W.AutoKey = m_WAutoKey.Checked;
      _settings.W.KeyUpDelay = (int)m_WKeyUpDelay.Value;
      _settings.W.KeyDownDelay = (int)m_WKeyDownDelay.Value;
      _settings.W.KeyThreadDelay = (int)m_WKeyThreadDelay.Value;
      _settings.W.MemThreadDelay = (int)m_WMemThreadDelay.Value;
      _settings.W.WarnThreadDelay = (int)m_WWarnThreadDelay.Value;
      _settings.W.Scale = (float)(m_WScale.Value / 100);
      _settings.W.WarnScale = (float)(m_WWarnScale.Value / 100);
      _settings.W.WarnVolume = (float)(m_WWarnVolume.Value / 100);

      // Save QWE settings
      _settings.QWE.Q = _qweQKeyAuto.Checked;
      _settings.QWE.W = _qweWKeyAuto.Checked;
      _settings.QWE.E = _qweEKeyAuto.Checked;

      _settings.QWE.KeyUpDelayQ = (int)_qweQKeyUpDelay.Value;
      _settings.QWE.KeyDownDelayQ = (int)_qweQKeyDownDelay.Value;
      _settings.QWE.ThreadDelayQ = (int)_qweQKeyThreadDelay.Value;

      _settings.QWE.KeyUpDelayW = (int)_qweWKeyUpDelay.Value;
      _settings.QWE.KeyDownDelayW = (int)_qweWKeyDownDelay.Value;
      _settings.QWE.ThreadDelayW = (int)_qweWKeyThreadDelay.Value;

      _settings.QWE.KeyUpDelayE = (int)_qweEKeyUpDelay.Value;
      _settings.QWE.KeyDownDelayE = (int)_qweEKeyDownDelay.Value;
      _settings.QWE.ThreadDelayE = (int)_qweEKeyThreadDelay.Value;

      _settings.M.Auto = _MRClick.Checked;
      _settings.M.ThreadDelay = (int)_MRThreadDelay.Value;
      _settings.M.ClickDelay = (int)_MRClickDelay.Value;

      _settings.T.Auto = _WarnAuto.Checked;
      _settings.T.Interval = (float)_WarnInterval.Value;
      _settings.T.Duration = (int)_WarnDuration.Value;
      _settings.T.Volume = (float)_WarnVolume.Value / 100;
      _settings.T.TimerInterval = (int)_WarnTimer.Value;
      _settings.T.Dismiss = _WarnDismiss.Text;

      _settings.SaveData();

      _saved = true;
    }
    //----------------------------------------------------------------------------------
    private void AskToQuit(DialogResult result)
    {
      if (false == _saved)
      {
        var m = MessageBox.Show("Do you want to save?", Resources.MsgBoxCaption, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
        if (m == DialogResult.Yes)
        {
          UIToData();
          DialogResult = result;
        }
        else if (m == DialogResult.No)
        {
          DialogResult = result;
        }
        else if (m == DialogResult.Cancel)
        {
          return;
        }
      }
      else
      {
        DialogResult = result;
      }
    }
    //----------------------------------------------------------------------------------
    private void m_OKButton_Click(object sender, EventArgs e)
    {
      AskToQuit(DialogResult.OK);
    }
    //----------------------------------------------------------------------------------
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      AskToQuit(DialogResult.Cancel);
    }
    //----------------------------------------------------------------------------------
    private void SettingsForm_Load(object sender, EventArgs e)
    {
    }
    //----------------------------------------------------------------------------------
    private void m_DefaultButton_Click(object sender, EventArgs e)
    {
      _settings = new ThreadSettings();
      _saved = false;
      ShowData();
    }
    //----------------------------------------------------------------------------------
    private void m_ReloadButton_Click(object sender, EventArgs e)
    {
      _settings.LoadData();
      _saved = false;
      ShowData();
    }
    //----------------------------------------------------------------------------------
    private void _SaveButton_Click(object sender, EventArgs e)
    {
      UIToData();
    }
  }
}
