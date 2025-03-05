using System;
using System.Drawing;
using Vanara.PInvoke;

class WindowManipulate
{
  public static void HideWindow(IntPtr hWnd)
  {
    User32.SetWindowPos(hWnd, HWND.HWND_BOTTOM, -10000, -10000, 0, 0,
        User32.SetWindowPosFlags.SWP_NOSIZE | User32.SetWindowPosFlags.SWP_NOZORDER | User32.SetWindowPosFlags.SWP_NOACTIVATE);
    var style = (User32.WindowStylesEx)User32.GetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE);
    style &= ~User32.WindowStylesEx.WS_EX_APPWINDOW;
    style |= User32.WindowStylesEx.WS_EX_TOOLWINDOW;
    User32.SetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE, (int)style);
  }

  public static void ShowWindow(IntPtr hWnd)
  {
    User32.GetWindowRect(hWnd, out RECT rect);
    int width = rect.right - rect.left;
    int height = rect.bottom - rect.top;

    User32.MONITORINFO mi = new User32.MONITORINFO();
    mi.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(User32.MONITORINFO));
    User32.GetMonitorInfo(User32.MonitorFromWindow(hWnd, User32.MonitorFlags.MONITOR_DEFAULTTONEAREST), ref mi);
    int centerX = mi.rcWork.left + (mi.rcWork.right - mi.rcWork.left - width) / 2;
    int centerY = mi.rcWork.top + (mi.rcWork.bottom - mi.rcWork.top - height) / 2;

    User32.SetWindowPos(hWnd, HWND.HWND_TOP, centerX, centerY, 0, 0,
        User32.SetWindowPosFlags.SWP_NOSIZE | User32.SetWindowPosFlags.SWP_NOZORDER |
        User32.SetWindowPosFlags.SWP_NOACTIVATE | User32.SetWindowPosFlags.SWP_SHOWWINDOW | User32.SetWindowPosFlags.SWP_FRAMECHANGED);

    var style = (User32.WindowStylesEx)User32.GetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE);
    style |= User32.WindowStylesEx.WS_EX_APPWINDOW;
    style &= ~User32.WindowStylesEx.WS_EX_TOOLWINDOW;
    User32.SetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE, (int)style);

    SetIcon(hWnd);
  }

  public static void ToggleWindow(IntPtr hWnd)
  {
    var style = (User32.WindowStylesEx)User32.GetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE);
    if ((style & User32.WindowStylesEx.WS_EX_TOOLWINDOW) != 0)
      ShowWindow(hWnd);
    else
      HideWindow(hWnd);
  }

  public static bool IsShowing(IntPtr hWnd)
  {
    var style = (User32.WindowStylesEx)User32.GetWindowLong(hWnd, User32.WindowLongFlags.GWL_EXSTYLE);
    return (style & User32.WindowStylesEx.WS_EX_TOOLWINDOW) == 0;
  }

  private static void SetIcon(IntPtr hWnd)
  {
    Icon newIcon = MXTools.Properties.Resources.db;
    IntPtr iconHandle = newIcon.Handle;

    User32.SendMessage(hWnd, User32.WindowMessage.WM_SETICON, (IntPtr)0, iconHandle);
    User32.SendMessage(hWnd, User32.WindowMessage.WM_SETICON, (IntPtr)1, iconHandle);
    User32.SetClassLong(hWnd, User32.GetClassLongFlag.GCL_HICON, iconHandle);
    User32.SetClassLong(hWnd, User32.GetClassLongFlag.GCL_HICONSM, iconHandle);
  }
}