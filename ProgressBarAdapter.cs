using System;
using System.Windows.Forms;

namespace AutoCheck
{
  internal class ProgressBarAdapter : IValuesDisplay
  {
    private ProgressBar _progressBar;
    private int _max;
    private int _current;

    public ProgressBarAdapter(ProgressBar progressBar, int minimum = 0, int maximum = 100)
    {
      _progressBar = progressBar ?? throw new ArgumentNullException(nameof(progressBar));
      _progressBar.Minimum = minimum;
      _progressBar.Maximum = maximum; // Caller defines max blocks
      _max = maximum > 0 ? maximum : 100; // Default _max
    }

    public int MaxValue
    {
      get => _max;
      set
      {
        if (_max != value && value > 0)
        {
          _max = value;
          CurValue = _current; // Recalculate progress with new max
        }
      }
    }

    public int CurValue
    {
      get => _current;
      set
      {
        if (_current == value) return; // Prevent unnecessary updates

        _current = Math.Max(0, Math.Min(value, _max)); // Clamp to [0, _max]
        int scaledValue = (_current * (_progressBar.Maximum - _progressBar.Minimum)) / _max; // Scale based on ProgressBar max

        if (_progressBar.InvokeRequired)
        {
          _progressBar.Invoke(new Action(() => _progressBar.Value = scaledValue));
        }
        else
        {
          _progressBar.Value = scaledValue;
        }
      }
    }

    public bool InvokeRequired => _progressBar.InvokeRequired;

    public IAsyncResult BeginInvoke(Delegate method, params object[] args)
    {
      return _progressBar.BeginInvoke(method, args);
    }
  }
}
