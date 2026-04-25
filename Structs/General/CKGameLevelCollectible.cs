using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x2C)]
    public readonly struct CKGameLevelCollectible
    {
        [FieldOffset(0x1C)] private readonly byte _collected;
        [FieldOffset(0x1D)] private readonly byte _bought;
        public readonly bool Collected => _collected == 1;
        public readonly bool Bought => _bought == 1;
    }
}
