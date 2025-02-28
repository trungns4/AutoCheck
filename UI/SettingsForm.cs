﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MXTools.Properties;

namespace MXTools
{
  public partial class SettingsForm : Form
  {
    private Settings _settings;

    private bool _saved = true;

    public SettingsForm(Settings settings)
    {
      InitializeComponent();

      _settings = new Settings();
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
    public Settings Settings
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
      m_QAuto.Checked = _settings.Q._auto;
      m_QAutoKey.Checked = _settings.Q._autoKey;
      m_QKeyUpDelay.Value = _settings.Q._keyUpDelay;
      m_QKeyDownDelay.Value = _settings.Q._keyDownDelay;
      m_QKeyThreadDelay.Value = _settings.Q._keyThreadDelay;
      m_QMemThreadDelay.Value = _settings.Q._memThreadDelay;
      m_QWarnThreadDelay.Value = _settings.Q._warnThreadDelay;
      m_QScale.Value = (decimal)(_settings.Q._scale * 100);
      m_QWarnScale.Value = (decimal)(_settings.Q._warnScale * 100);
      m_QWarnVolume.Value = (decimal)(_settings.Q._warnVolume * 100);

      m_WAuto.Checked = _settings.W._auto;
      m_WAutoKey.Checked = _settings.W._autoKey;
      m_WKeyUpDelay.Value = _settings.W._keyUpDelay;
      m_WKeyDownDelay.Value = _settings.W._keyDownDelay;
      m_WKeyThreadDelay.Value = _settings.W._keyThreadDelay;
      m_WMemThreadDelay.Value = _settings.W._memThreadDelay;
      m_WWarnThreadDelay.Value = _settings.W._warnThreadDelay;
      m_WScale.Value = (decimal)(_settings.W._scale * 100);
      m_WWarnScale.Value = (decimal)(_settings.W._warnScale * 100);
      m_WWarnVolume.Value = (decimal)(_settings.W._warnVolume * 100);

      _qweQKeyAuto.Checked = _settings.QWE._q;
      _qweWKeyAuto.Checked = _settings.QWE._w;
      _qweEKeyAuto.Checked = _settings.QWE._e;

      _qweQKeyUpDelay.Value = _settings.QWE._keyUpDelayQ;
      _qweQKeyDownDelay.Value = _settings.QWE._keyDownDelayQ;
      _qweQKeyThreadDelay.Value = _settings.QWE._threadDelayQ;

      _qweWKeyUpDelay.Value = _settings.QWE._keyUpDelayW;
      _qweWKeyDownDelay.Value = _settings.QWE._keyDownDelayW;
      _qweWKeyThreadDelay.Value = _settings.QWE._threadDelayW;

      _qweEKeyUpDelay.Value = _settings.QWE._keyUpDelayE;
      _qweEKeyDownDelay.Value = _settings.QWE._keyDownDelayE;
      _qweEKeyThreadDelay.Value = _settings.QWE._threadDelayE;

      _MRClick.Checked = _settings.M._auto;
      _MRThreadDelay.Value = _settings.M._threadDelay;
      _MRClickDelay.Value = _settings.M._clickDelay;

      _WarnAuto.Checked = _settings.T._auto;
      _WarnInterval.Value = (decimal)_settings.T._interval;
      _WarnDuration.Value = (decimal)_settings.T._duration;
      _WarnVolume.Value = (decimal)_settings.T._volume * 100;
      _WarnTimer.Value = (decimal)_settings.T._timerInterval;
      _WarnDismiss.Text = _settings.T._dismiss;
    }
    //----------------------------------------------------------------------------------
    private void UIToData()
    {
      // Save Q settings
      _settings.Q._auto = m_QAuto.Checked;
      _settings.Q._autoKey = m_QAutoKey.Checked;
      _settings.Q._keyUpDelay = (int)m_QKeyUpDelay.Value;
      _settings.Q._keyDownDelay = (int)m_QKeyDownDelay.Value;
      _settings.Q._keyThreadDelay = (int)m_QKeyThreadDelay.Value;
      _settings.Q._memThreadDelay = (int)m_QMemThreadDelay.Value;
      _settings.Q._warnThreadDelay = (int)m_QWarnThreadDelay.Value;
      _settings.Q._scale = (float)(m_QScale.Value / 100);
      _settings.Q._warnScale = (float)(m_QWarnScale.Value / 100);
      _settings.Q._warnVolume = (float)(m_QWarnVolume.Value / 100);

      // Save W settings
      _settings.W._auto = m_WAuto.Checked;
      _settings.W._autoKey = m_WAutoKey.Checked;
      _settings.W._keyUpDelay = (int)m_WKeyUpDelay.Value;
      _settings.W._keyDownDelay = (int)m_WKeyDownDelay.Value;
      _settings.W._keyThreadDelay = (int)m_WKeyThreadDelay.Value;
      _settings.W._memThreadDelay = (int)m_WMemThreadDelay.Value;
      _settings.W._warnThreadDelay = (int)m_WWarnThreadDelay.Value;
      _settings.W._scale = (float)(m_WScale.Value / 100);
      _settings.W._warnScale = (float)(m_WWarnScale.Value / 100);
      _settings.W._warnVolume = (float)(m_WWarnVolume.Value / 100);

      // Save QWE settings
      _settings.QWE._q = _qweQKeyAuto.Checked;
      _settings.QWE._w = _qweWKeyAuto.Checked;
      _settings.QWE._e = _qweEKeyAuto.Checked;

      _settings.QWE._keyUpDelayQ = (int)_qweQKeyUpDelay.Value;
      _settings.QWE._keyDownDelayQ = (int)_qweQKeyDownDelay.Value;
      _settings.QWE._threadDelayQ = (int)_qweQKeyThreadDelay.Value;

      _settings.QWE._keyUpDelayW = (int)_qweWKeyUpDelay.Value;
      _settings.QWE._keyDownDelayW = (int)_qweWKeyDownDelay.Value;
      _settings.QWE._threadDelayW = (int)_qweWKeyThreadDelay.Value;

      _settings.QWE._keyUpDelayE = (int)_qweEKeyUpDelay.Value;
      _settings.QWE._keyDownDelayE = (int)_qweEKeyDownDelay.Value;
      _settings.QWE._threadDelayE = (int)_qweEKeyThreadDelay.Value;

      _settings.M._auto = _MRClick.Checked;
      _settings.M._threadDelay = (int)_MRThreadDelay.Value;
      _settings.M._clickDelay = (int)_MRClickDelay.Value;

      _settings.T._auto = _WarnAuto.Checked;
      _settings.T._interval = (float)_WarnInterval.Value;
      _settings.T._duration = (int)_WarnDuration.Value;
      _settings.T._volume = (float)_WarnVolume.Value / 100;
      _settings.T._timerInterval = (int)_WarnTimer.Value;
      _settings.T._dismiss = _WarnDismiss.Text;

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
      _settings = new Settings();
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
