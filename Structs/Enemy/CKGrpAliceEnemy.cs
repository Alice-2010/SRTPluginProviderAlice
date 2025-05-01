using System.Runtime.InteropServices;
using System;

namespace SRTPluginProviderAlice.Structs.Enemy
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x50)]
    public readonly struct CKGrpAliceEnemy
    {
        [FieldOffset(0x30)] private readonly int _fightZone;
        public readonly IntPtr FightZone => (IntPtr)_fightZone;
    }
}
