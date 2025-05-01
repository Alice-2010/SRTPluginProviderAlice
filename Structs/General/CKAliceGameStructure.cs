using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.General
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xD4)]
    public readonly struct CKAliceGameStructure
    {
        [FieldOffset(0x54)] private readonly int _inventoryItemsList;
        [FieldOffset(0x60)] private readonly float _player1Health;
        [FieldOffset(0x68)] private readonly float _player2Health;
        public readonly IntPtr InventoryItemsList => (IntPtr)_inventoryItemsList;
        public readonly float Player1Health => _player1Health;
        public readonly float Player2Health => _player2Health;

    }
}
