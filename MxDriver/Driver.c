#include <ntddk.h>
#include <ntddkbd.h>
#include <ntstrsafe.h>  // Add this at the top


PDEVICE_OBJECT KeyboardDevice = NULL;

// Function to locate an active keyboard device
NTSTATUS LocateKeyboardDevice() {
  UNICODE_STRING DeviceName;
  PFILE_OBJECT FileObject = NULL;
  NTSTATUS status = STATUS_UNSUCCESSFUL;

  for (int i = 0; i < 5; i++) { // Try KeyboardClass0 to KeyboardClass4
    WCHAR NameBuffer[64];
    RtlStringCchPrintfW(NameBuffer, 64, L"\\Device\\KeyboardClass%d", i);
    RtlInitUnicodeString(&DeviceName, NameBuffer);

    status = IoGetDeviceObjectPointer(&DeviceName, FILE_ALL_ACCESS, &FileObject, &KeyboardDevice);
    if (NT_SUCCESS(status)) {
      ObDereferenceObject(FileObject);
      DbgPrintEx(DPFLTR_IHVDRIVER_ID, DPFLTR_INFO_LEVEL, "KbdInject: Found keyboard device: %wZ\n", &DeviceName);
      return STATUS_SUCCESS;
    }
  }

  DbgPrintEx(DPFLTR_IHVDRIVER_ID, DPFLTR_ERROR_LEVEL, "KbdInject: No keyboard device found.\n");
  return STATUS_UNSUCCESSFUL;
}

// Function to send a keystroke event
NTSTATUS SendKeyEvent(UCHAR VirtualKey, BOOLEAN KeyDown) {
  if (!KeyboardDevice) {
    DbgPrintEx(DPFLTR_IHVDRIVER_ID, DPFLTR_ERROR_LEVEL, "KbdInject: Keyboard device not set!\n");
    return STATUS_UNSUCCESSFUL;
  }

  KEYBOARD_INPUT_DATA InputData;
  RtlZeroMemory(&InputData, sizeof(KEYBOARD_INPUT_DATA));
  InputData.MakeCode = VirtualKey;
  InputData.Flags = KeyDown ? 0 : KEY_BREAK;

  // Create IRP for keyboard driver
  PIRP Irp = IoBuildDeviceIoControlRequest(IOCTL_KEYBOARD_INSERT_DATA, KeyboardDevice, &InputData, sizeof(InputData), NULL, 0, FALSE, NULL, NULL);
  if (!Irp) {
    DbgPrintEx(DPFLTR_IHVDRIVER_ID, DPFLTR_ERROR_LEVEL, "KbdInject: Failed to build IRP\n");
    return STATUS_INSUFFICIENT_RESOURCES;
  }

  NTSTATUS status = IoCallDriver(KeyboardDevice, Irp);
  DbgPrintEx(DPFLTR_IHVDRIVER_ID, DPFLTR_INFO_LEVEL, "KbdInject: SendKeyEvent VK=0x%X, KeyDown=%d, Status=0x%X\n", VirtualKey, KeyDown, status);

  return status;
}

// Entry point for KDU mapping (NO DriverObject Needed)
NTSTATUS EntryPoint() {
  if (!NT_SUCCESS(LocateKeyboardDevice())) {
    return STATUS_UNSUCCESSFUL;
  }

  DbgPrintEx(DPFLTR_IHVDRIVER_ID, DPFLTR_INFO_LEVEL, "KbdInject: Driver mapped successfully!\n");
  return STATUS_SUCCESS;
}

NTSTATUS DriverEntry(PDRIVER_OBJECT DriverObject, PUNICODE_STRING RegistryPath) {
  UNREFERENCED_PARAMETER(DriverObject);
  UNREFERENCED_PARAMETER(RegistryPath);
  return STATUS_SUCCESS;  // ✅ This prevents linker errors
}
