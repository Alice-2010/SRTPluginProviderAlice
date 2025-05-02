using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x14)]
    public readonly struct CKAlicePlayer
    {
        [FieldOffset(0xC)] internal readonly int _gameState;
        [FieldOffset(0x10)] public readonly float GameTime;
        public IntPtr GameState => (IntPtr)_gameState;
    }
}
