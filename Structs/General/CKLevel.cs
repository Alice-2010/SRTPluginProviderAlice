using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1C)]
    internal readonly struct CKLevel
    {
        [FieldOffset(0x18)] internal readonly int Sector;
    }
}
