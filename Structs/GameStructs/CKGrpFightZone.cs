using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x50)]
    public readonly struct CKGrpFightZone
    {
        [FieldOffset(0x18)] private readonly int firstSquad;
        public readonly IntPtr FirstSquad => (IntPtr)firstSquad;
    }
}
