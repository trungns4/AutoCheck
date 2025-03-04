#pragma once

#include <memory>
#include "BlackBone/Process/Process.h"

using namespace System::Threading;

namespace MXTools
{
  public delegate bool ScanProgressDelegate(unsigned __int64 current);
  public delegate void ScanCompleteDelegate(unsigned __int64 address, int offset);

  public ref class MxSharp
  {
  private:
    static MxSharp^ _instance;
    static System::Object^ lockObj = gcnew System::Object();

  private:
    blackbone::Process* _process;

  private:
    MxSharp();

  public:
    static property MxSharp^ Instance
    {
      MxSharp^ get();
    }

    ~MxSharp();

    bool Attach(unsigned int pid);
    bool EnsureAttached();
    void Dettach();

    unsigned int PID();
    bool Valid();

    static unsigned int FindProcess(System::String^ name);

    bool GetMemoryInfo(unsigned __int64% numRegions, unsigned __int64% totalBytes);
    bool ScanMemory(int number, int offset, ScanProgressDelegate^ progress, ScanCompleteDelegate^ complete, CancellationToken token);

    int ReadMemory(unsigned __int64 addr);
  };
}