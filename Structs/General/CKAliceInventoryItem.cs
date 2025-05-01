using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x70)]
    public readonly struct CKAliceInventoryItem
    {
        [FieldOffset(0x14)] private readonly int _gameLevelCollectible;
        public readonly IntPtr GameLevelCollectible => (IntPtr)_gameLevelCollectible;
    }
}
