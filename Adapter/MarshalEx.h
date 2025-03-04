#pragma once
#include <msclr/marshal_cppstd.h>
#include <string>

using namespace msclr::interop;
using namespace System;

namespace MXTools
{
  ref class MarshalEx
  {
    public: 

      static std::wstring ConvertToWString(System::String^ managedString)
      {
        return msclr::interop::marshal_as<std::wstring>(managedString);
      }

      static String^ WStringToString(const std::wstring& wstr)
      {
        return marshal_as<String^>(wstr);
      }
  };
}