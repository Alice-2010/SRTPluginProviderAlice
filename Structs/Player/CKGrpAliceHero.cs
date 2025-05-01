using System.Runtime.InteropServices;
using System;

namespace SRTPluginProviderAlice.Structs.Player
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x100)]
    internal readonly struct CKGrpAliceHero
    {
        [FieldOffset(0x28)] private readonly int _firstPlayer;
        public readonly IntPtr FirstPlayer => (IntPtr)_firstPlayer;
    }
}
