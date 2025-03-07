/*++

Module Name:

    public.h

Abstract:

    This module contains the common declarations shared by driver
    and user applications.

Environment:

    user and kernel

--*/

//
// Define an Interface Guid so that apps can find the device and talk to it.
//

DEFINE_GUID (GUID_DEVINTERFACE_MxDriver,
    0x5471a56b,0x4411,0x4dd4,0xbb,0x73,0xe6,0x8f,0x85,0xee,0x4f,0x33);
// {5471a56b-4411-4dd4-bb73-e68f85ee4f33}
