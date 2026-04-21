using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x14)]
    public readonly struct CKAlicePlayer
    {
        [FieldOffset(0xC)] private readonly int _gameState;
        [FieldOffset(0x10)] internal readonly float GameTime;
        public readonly IntPtr GameState => (IntPtr)_gameState;
    }
}
