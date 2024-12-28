using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.GameStructs
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1D0)]
    public readonly struct CKGrpSquad
    {
        [FieldOffset(0x14)] private readonly int nextSquad;
        [FieldOffset(0x28)] private readonly int firstEnemy;
        public readonly IntPtr NextSquad => (IntPtr)nextSquad;
        public readonly IntPtr FirstEnemy => (IntPtr)firstEnemy;
    }
}
