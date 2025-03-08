using System;
using System.Drawing;
using System.Reflection;
using Vanara.PInvoke;
using static MXTools.Helpers.Win32;
using static Vanara.PInvoke.User32;
using static Vanara.PInvoke.Kernel32;
using log4net;

namespace MXTools.Threads
{
  public class ForegroundWindowCheck
  {
    private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private static readonly Lazy<ForegroundWindowCheck> _instance = new(() => new ForegroundWindowCheck());
    public static ForegroundWindowCheck Instance => _instance.Value;

    public event Action<nint, Rectangle> ForegroundWindowChanged;
    private readonly WinEventProc _winEventDelegate;
    private HWINEVENTHOOK _hookHandle;
    private Rectangle _currentRectangle;
    private uint _currentProcessId;

    private const uint EVENT_SYSTEM_FOREGROUND = 0x0003;
    private const uint WINEVENT_OUTOFCONTEXT = 0;

    private ForegroundWindowCheck()
    {
      try
      {
        _winEventDelegate = WinEventProc;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while initializing ForegroundWindowCheck: {ex.Message}");
      }
    }

    public void Start()
    {
      try
      {
        _hookHandle = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, HINSTANCE.NULL, _winEventDelegate, 0, 0, WINEVENT_OUTOFCONTEXT);
        _log.Info("Monitoring foreground window started");
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while starting ForegroundWindowCheck: {ex.Message}");
      }
    }

    public void Stop()
    {
      try
      {
        if (!_hookHandle.IsNull)
        {
          UnhookWinEvent(_hookHandle);
          _hookHandle = default;
        }
        _log.Info("Monitoring foreground window stopped");
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred while stopping ForegroundWindowCheck: {ex.Message}");
      }
    }

    public Rectangle GetCurrentRectangle()
    {
      try
      {
        if (_currentRectangle.IsEmpty)
        {
          HWND hwnd = GetForegroundWindow();
          if (!hwnd.IsNull && GetWindowRect(hwnd, out RECT rect))
          {
            _currentRectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
          }
        }
        return _currentRectangle;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in GetCurrentRectangle: {ex.Message}");
        return Rectangle.Empty;
      }
    }

    public uint GetCurrentProcessId()
    {
      try
      {
        return _currentProcessId == 0 ? GetForegroundProcessId() : _currentProcessId;
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in GetCurrentProcessId: {ex.Message}");
        return 0;
      }
    }

    public static uint GetForegroundProcessId()
    {
      try
      {
        HWND hwnd = GetForegroundWindow();
        if (!hwnd.IsNull)
        {
          _ = GetWindowThreadProcessId(hwnd, out uint processId);
          return processId;
        }
        return 0;
      }
      catch (Exception ex)
      {
        LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
            .Fatal($"An error occurred in GetForegroundProcessId: {ex.Message}");
        return 0;
      }
    }

    private void WinEventProc(HWINEVENTHOOK hWinEventHook, uint eventType, HWND hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
    {
      try
      {
        if (eventType == EVENT_SYSTEM_FOREGROUND && hwnd != nint.Zero)
        {
          if (GetWindowRect(hwnd, out var rect))
          {
            _currentRectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            _ = GetWindowThreadProcessId(hwnd, out _currentProcessId);
            ForegroundWindowChanged?.Invoke((nint)hwnd, _currentRectangle);
          }
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in WinEventProc: {ex.Message}");
      }
    }

    public void ForceForegroundCheck()
    {
      try
      {
        HWND hwnd = GetForegroundWindow();
        if (!hwnd.IsNull)
        {
          _ = GetWindowThreadProcessId(hwnd, out _currentProcessId);
          if (GetWindowRect(hwnd, out RECT rect))
          {
            _currentRectangle = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
          }
        }
      }
      catch (Exception ex)
      {
        _log.Fatal($"An error occurred in ForceForegroundCheck: {ex.Message}");
      }
    }
  }
}
