using log4net;
using System;
using System.Drawing;
using System.Reflection;
using System.Text;
using Vanara.PInvoke;
using static MXTools.Helpers.Win32;
using static Vanara.PInvoke.User32;
using static Vanara.PInvoke.Kernel32;

namespace MxTools
{
  public class ForegroundWindowCheck
  {
    private ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private static readonly Lazy<ForegroundWindowCheck> _instance = new Lazy<ForegroundWindowCheck>(() => new ForegroundWindowCheck());
    public static ForegroundWindowCheck Instance => _instance.Value;

    public event Action<IntPtr, Rectangle> ForegroundWindowChanged;
    private User32.WinEventProc _winEventDelegate;
    private HWINEVENTHOOK _hookHandle;
    private Rectangle _currentRectangle;
    private uint _currentProcessId;

    private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
    private const uint WINEVENT_OUTOFCONTEXT = 0;

    private ForegroundWindowCheck()
    {
      _winEventDelegate = WinEventProc;
    }

    public void Start()
    {
      _hookHandle = User32.SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, HINSTANCE.NULL, _winEventDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
      _log.Info("Monitoring foreground window started");
    }

    public void Stop()
    {
      if (!_hookHandle.IsNull)
      {
        User32.UnhookWinEvent(_hookHandle);
        _hookHandle = default;
      }
      _log.Info("Monitoring foreground window stopped");
    }

    public Rectangle GetCurrentRectangle()
    {
      if(_currentRectangle.IsEmpty)
      {
        HWND hwnd = GetForegroundWindow();
        GetWindowRect(hwnd, out Vanara.PInvoke.RECT rect);
        _currentRectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
      }
      return _currentRectangle;
    }

    public uint GetCurrentProcessId()
    {
      if (_currentProcessId == 0)
      {
        return GetForegroundProcessId();
      }
      else
      {
        return _currentProcessId;
      }
    }

    public uint GetForegroundProcessId()
    {
      HWND hwnd = GetForegroundWindow();
      if (!hwnd.IsNull)
      {
        User32.GetWindowThreadProcessId(hwnd, out uint processId);
        return processId;
      }
      return 0;
    }

    private void WinEventProc(HWINEVENTHOOK hWinEventHook, uint eventType, HWND hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
      if (eventType == EVENT_SYSTEM_FOREGROUND && hwnd != IntPtr.Zero)
      {
        if (User32.GetWindowRect(hwnd, out var rect))
        {
          _currentRectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
          User32.GetWindowThreadProcessId(hwnd, out _currentProcessId);
          ForegroundWindowChanged?.Invoke((nint)hwnd, _currentRectangle);

          _log.Info($"Foreground changed to {_currentProcessId}");
        }
      }
    }
  }
}
