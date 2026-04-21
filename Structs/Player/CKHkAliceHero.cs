using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.Player
{
    // NOTE: This could be a byte like enemies aswell rather than int
    public enum PlayerCharacter: int
    {
        Invalid = -1,
        McTwisp = 0,
        MadHatter = 1,
        CheshireCat = 2,
        MarchHare = 3,
        AliceSmall = 4,
        Alice = 5,
        Mallymkun = 6
    }

    public enum HeroNumber: byte
    {
        Invalid = 0xFF,
        Player1 = 0,
        Player2 = 1,
        Alice = 2
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0xB8C)]
    public struct CKHkAliceHero
    {
        [FieldOffset(0x4)] private int _nextPlayer;
        [FieldOffset(0x28)] private readonly byte _heroNumber;
        [FieldOffset(0x68)] internal readonly float PositionX;
        [FieldOffset(0x6C)] internal readonly float PositionY;
        [FieldOffset(0x70)] internal readonly float PositionZ;
        [FieldOffset(0x9D0)] private readonly int _characterType;
        public readonly IntPtr NextPlayer => (IntPtr)_nextPlayer;
        public readonly HeroNumber HeroNumber => (HeroNumber)_heroNumber;
        public readonly PlayerCharacter CharacterType => (PlayerCharacter)_characterType;
        public readonly string CharacterName
        {
            get
            {
                return CharacterType switch
                {
                    PlayerCharacter.McTwisp => "McTwisp",
                    PlayerCharacter.MadHatter => "Mad Hatter",
                    PlayerCharacter.CheshireCat => "Cheshire Cat",
                    PlayerCharacter.MarchHare => "March Hare",
                    PlayerCharacter.AliceSmall => "Alice (Small)",
                    PlayerCharacter.Alice => "Alice",
                    PlayerCharacter.Mallymkun => "Mallymkun",
                    _ => "Invalid",
                };
            }
        }
    }
}
