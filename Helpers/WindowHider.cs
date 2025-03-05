using System;
using System.Drawing;
using System.Runtime.InteropServices;

class WindowHider
{
  private const int GWL_EXSTYLE = -20;
  private const int WS_EX_TOOLWINDOW = 0x00000080;
  private const int WS_EX_APPWINDOW = 0x00040000;
  private const int SWP_NOSIZE = 0x0001;
  private const int SWP_NOZORDER = 0x0004;
  private const int SWP_NOACTIVATE = 0x0010;
  private const int SWP_SHOWWINDOW = 0x0040;
  private const int SWP_FRAMECHANGED = 0x0020;

  private const int HORZRES = 8;
  private const int VERTRES = 10;

  [DllImport("user32.dll")]
  private static extern IntPtr GetForegroundWindow();

  [DllImport("user32.dll")]
  private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

  [DllImport("user32.dll")]
  private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32.dll")]
  private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

  [DllImport("user32.dll")]
  private static extern IntPtr GetDesktopWindow();

  [DllImport("user32.dll")]
  private static extern IntPtr GetDC(IntPtr hWnd);

  [DllImport("user32.dll")]
  private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

  [DllImport("gdi32.dll")]
  private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

  [DllImport("user32.dll")]
  private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

  [StructLayout(LayoutKind.Sequential)]
  private struct RECT
  {
    public int Left, Top, Right, Bottom;
  }

  [DllImport("user32.dll")]
  private static extern bool SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

  private const int GCL_HICON = -14;  // Large Icon
  private const int GCL_HICONSM = -34; // Small Icon
  private const int WM_SETICON = 0x80;

  [DllImport("user32.dll", EntryPoint = "SetClassLong")]
  private static extern int SetClassLong32(IntPtr hWnd, int nIndex, int dwNewLong);

  [DllImport("user32.dll", EntryPoint = "SetClassLongPtr")]
  private static extern IntPtr SetClassLong64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


  [DllImport("user32.dll")]
  public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

  // RedrawWindow Flags
  private const uint RDW_INVALIDATE = 0x0001;
  private const uint RDW_INTERNALPAINT = 0x0002;
  private const uint RDW_UPDATENOW = 0x0100;
  private const uint RDW_FRAME = 0x0400;

  public static void HideWindow(IntPtr hWnd)
  {
    SetWindowPos(hWnd, IntPtr.Zero, -10000, -10000, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE);
    int style = GetWindowLong(hWnd, GWL_EXSTYLE);
    style &= ~WS_EX_APPWINDOW;
    style |= WS_EX_TOOLWINDOW;
    SetWindowLong(hWnd, GWL_EXSTYLE, style);
  }

  public static void ShowWindow(IntPtr hWnd)
  {
    // Get screen size
    IntPtr desktopHwnd = GetDesktopWindow();
    IntPtr hdc = GetDC(desktopHwnd);
    int screenWidth = GetDeviceCaps(hdc, HORZRES);
    int screenHeight = GetDeviceCaps(hdc, VERTRES);
    ReleaseDC(desktopHwnd, hdc);

    // Get window size
    GetWindowRect(hWnd, out RECT rect);
    int windowWidth = rect.Right - rect.Left;
    int windowHeight = rect.Bottom - rect.Top;

    // Calculate centered position
    int posX = (screenWidth - windowWidth) / 2;
    int posY = (screenHeight - windowHeight) / 2;

    SetWindowPos(hWnd, IntPtr.Zero, posX, posY, 0, 0, SWP_NOSIZE | SWP_NOZORDER | SWP_NOACTIVATE | SWP_SHOWWINDOW | SWP_FRAMECHANGED);

    int style = GetWindowLong(hWnd, GWL_EXSTYLE);
    style |= WS_EX_APPWINDOW;
    style &= ~WS_EX_TOOLWINDOW;
    SetWindowLong(hWnd, GWL_EXSTYLE, style);

    SetIcon(hWnd);
  }
  //----------------------------------------------------------------------------------
  public static void ToggleWindow(IntPtr hWnd)
  {
    int style = GetWindowLong(hWnd, GWL_EXSTYLE);
    if ((style & WS_EX_TOOLWINDOW) != 0)
    {
      ShowWindow(hWnd);
    }
    else
    {
      HideWindow(hWnd);
    }
  }
  //----------------------------------------------------------------------------------
  public static bool IsShowing(IntPtr hWnd)
  {
    int style = GetWindowLong(hWnd, GWL_EXSTYLE);
    return ((style & WS_EX_TOOLWINDOW) == 0);
  }

  //----------------------------------------------------------------------------------
  private static IntPtr SetClassLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
  {
    if (IntPtr.Size == 4) // 32-bit
      return (IntPtr)SetClassLong32(hWnd, nIndex, (int)dwNewLong);
    else // 64-bit
      return SetClassLong64(hWnd, nIndex, dwNewLong);
  }
  //----------------------------------------------------------------------------------
  private static void SetIcon(IntPtr hWnd)
  {
    // Load the icon from Resources
    Icon newIcon = MXTools.Properties.Resources.db;
    IntPtr iconHandle = newIcon.Handle;

    // Change the title bar and Alt+Tab icon
    SendMessage(hWnd, WM_SETICON, (IntPtr)0, iconHandle); // Small icon
    SendMessage(hWnd, WM_SETICON, (IntPtr)1, iconHandle); // Large icon

    // Change the taskbar icon by modifying the class icon
    SetClassLongPtr(hWnd, GCL_HICON, iconHandle);    // Large taskbar icon
    SetClassLongPtr(hWnd, GCL_HICONSM, iconHandle);  // Small taskbar icon
  }
}
