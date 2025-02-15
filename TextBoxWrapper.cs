using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCheck
{
  public class TextBoxAdapter : ITextDisplay
  {
    private readonly TextBox _textBox;

    public TextBoxAdapter(TextBox textBox)
    {
      _textBox = textBox ?? throw new ArgumentNullException(nameof(textBox));
    }

    public string Text
    {
      get => _textBox.Text;
      set => _textBox.Text = value;
    }

    public bool InvokeRequired => _textBox.InvokeRequired;

    public IAsyncResult BeginInvoke(Delegate method, params object[] args)
    {
      return _textBox.BeginInvoke(method, args);
    }
  }
}
