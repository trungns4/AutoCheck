using System.Runtime.InteropServices;

namespace MXTools.Input
{
  public class Mouse
  {
    // Import mouse_event from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, nuint dwExtraInfo);

    // Mouse event constants
    private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
    private const uint MOUSEEVENTF_RIGHTUP = 0x0010;

    /// <summary>
    /// Simulates pressing the right mouse button down.
    /// </summary>
    public static void RightButtonDown()
    {
      mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, nuint.Zero);
    }

    /// <summary>
    /// Simulates releasing the right mouse button.
    /// </summary>
    public static void RightButtonUp()
    {
      mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, nuint.Zero);
    }
  }
}
