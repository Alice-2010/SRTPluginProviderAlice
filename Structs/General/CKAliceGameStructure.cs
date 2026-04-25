using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xD4)]
    public readonly struct CKAliceGameStructure
    {
        [FieldOffset(0x54)] private readonly int _inventoryItemsList;
        [FieldOffset(0x60)] internal readonly float Player1Health;
        [FieldOffset(0x68)] internal readonly float Player2Health;
        public readonly IntPtr InventoryItemsList => (IntPtr)_inventoryItemsList;

    }
}
