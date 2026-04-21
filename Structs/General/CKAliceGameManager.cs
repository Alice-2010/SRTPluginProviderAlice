using System.Runtime.InteropServices;
using System;

namespace SRTPluginProviderAlice.Structs.General
{
    public enum MapType : int
    {
        Loading = -1,
        MainMenu = 0,
        RoundHall = 10,
        StrangeGarden = 20,
        TulgeyWoods = 30,
        MarchHareHouse = 40,
        Hightopps = 50,
        Cabin = 60,
        RedDesert = 70,
        Moat = 75,
        SalazenGrum = 80,
        BandersnatchStables = 85,
        Marmoreal = 90,
        FrabjousDay = 100
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x2C4)]
    public readonly struct CKAliceGameManager
    {
        [FieldOffset(0x4)] private readonly int _enemyGroup;
        [FieldOffset(0x8)] private readonly int _heroGroup;
        [FieldOffset(0x18)] private readonly int _level;
        [FieldOffset(0x44)] private readonly int _player2;
        [FieldOffset(0x6C)] private readonly int _structure;
        [FieldOffset(0x2BC)] private readonly int _mapType;
        public readonly IntPtr EnemyGroup => (IntPtr)_enemyGroup;
        public readonly IntPtr HeroGroup => (IntPtr)_heroGroup;
        public readonly IntPtr Level => (IntPtr)_level;
        public readonly IntPtr Player2 => (IntPtr)_player2;
        public readonly IntPtr Structure => (IntPtr)_structure;
        public readonly MapType MapType => (MapType)_mapType;
    }
}
