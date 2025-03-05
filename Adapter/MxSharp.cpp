#include "MxSharp.h"
#include "MarshalEx.h"

namespace MXTools
{
  int _find(int number, BYTE* data, size_t size, CancellationToken token)
  {
    if (size < 28)
      return -1;

    for (int i = 0; i <= size - 28; i++)
    {
      if (token.IsCancellationRequested)
      {
        return -1;
      }

      int a, b, c, d;

      a = *reinterpret_cast<int*>(data + i);
      b = *reinterpret_cast<int*>(data + i + 8);
      c = *reinterpret_cast<int*>(data + i + 16);
      d = *reinterpret_cast<int*>(data + i + 24);

      if (a != number || c != a)
        continue;

      if (b == d)
      {
        return i;
      }
    }

    return -1;
  }
  //----------------------------------------------------------------------
  MxSharp::MxSharp()
  {
    _process = new blackbone::Process;
  }
  //----------------------------------------------------------------------
  MxSharp::~MxSharp()
  {
    delete _process;
    _process = nullptr;
  }

  //----------------------------------------------------------------------
  MxSharp^ MxSharp::Instance::get()
  {
    if (_instance == nullptr)
    {
      System::Threading::Monitor::Enter(lockObj);
      try
      {
        if (_instance == nullptr)
          _instance = gcnew MxSharp();
      }
      finally
      {
        System::Threading::Monitor::Exit(lockObj);
      }
    }

    return _instance;
  }
  //----------------------------------------------------------------------
  bool MxSharp::Attach(System::String^ name)
  {
    Dettach();

    _processName = name;

    auto pid = FindProcess(name);
    if (pid == 0)
    {
      return false;
    }

    return (_process->Attach(pid) == STATUS_SUCCESS);
  }
  //----------------------------------------------------------------------
  bool MxSharp::EnsureAttached()
  {
    if (Valid() == true)
    {
      return true;
    }
    else
    {
      if (Attach(_processName) == false)
      {
        return false;
      }
      return Valid();
    }
  }
  //----------------------------------------------------------------------
  void MxSharp::Dettach()
  {
    _process->Detach();
  }
  //----------------------------------------------------------------------
  unsigned int MxSharp::PID()
  {
    return _process->pid();
  }
  //----------------------------------------------------------------------
  bool MxSharp::Valid()
  {
    return _process->valid();
  }
  //----------------------------------------------------------------------
  unsigned int MxSharp::FindProcess(System::String^ name)
  {
    auto processList = blackbone::Process::EnumByName(MarshalEx::ConvertToWString(name));
    if (processList.empty() == false)
    {
      return processList.front();
    }
    return 0;
  }
  //----------------------------------------------------------------------
  bool MxSharp::GetMemoryInfo(unsigned __int64% numRegions, unsigned __int64% totalBytes)
  {
    if (Valid() == false)
    {
      return false;
    }

    auto& memory = _process->memory();
    auto regions = memory.EnumRegions();

    numRegions = regions.size();
    totalBytes = 0;

    for (const auto& region : regions)
    {
      totalBytes += region.RegionSize;
    }
    return true;
  }
  //----------------------------------------------------------------------
  bool MxSharp::ScanMemory(int number, int offset, ScanStartDelegate^ start, ScanProgressDelegate^ progress,
    ScanCompleteDelegate^ complete, CancellationToken token)
  {
    if (Valid() == false)
    {
      return false;
    }

    auto& memory = _process->memory();
    auto regions = memory.EnumRegions();

    size_t total = 0;
    for (const auto& region : regions)
    {
      total += region.RegionSize;
    }

    if (total == 0)
    {
      complete(0, 0);
      return false;
    }
   
    start(100);

    size_t current = 0;
    double percel = 0;
    for (const auto& region : regions)
    {
      if (token.IsCancellationRequested)
      {
        complete(0, 0);
        return false;
      }

      percel = (double)current / total * 100;
      if (percel < offset)
      {
        current += region.RegionSize;
        progress(percel);
        continue;
      }

      current += region.RegionSize;
      if (false == progress(percel))
      {
        complete(0, 0);
        return false;
      }

      std::vector<BYTE> buffer;
      buffer.resize(region.RegionSize);

      NTSTATUS status = memory.Read(region.BaseAddress, region.RegionSize, buffer.data(), true);
      if (NT_SUCCESS(status) == false)
      {
        if (false == progress(percel))
        {
          complete(0, 0);
          return false;
        }
        continue;
      }

      auto adr = _find(number, buffer.data(), buffer.size(), token);
      if (adr >= 0)
      {
        complete(region.BaseAddress + adr, Math::Max(0, Math::Min((int)percel - 1, 99)));
        return true;
      }
    }

    complete(0, 0);
    return false;
  }
  //----------------------------------------------------------------------
  int MxSharp::ReadMemory(unsigned __int64 addr)
  {
    if (Valid() == false)
    {
      return 0;
    }
    auto& memory = _process->memory();
    int value = -1;
    NTSTATUS status = memory.Read(addr, sizeof(int), &value);
    if (NT_SUCCESS(status) == false)
    {
      return -1;
    }
    return value;
  }
}