using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.GameStructs
{
    public enum EnemyType: byte
    {
        Spearman = 0,
        Archer = 1,
        Tank = 2,
        Fortress = 3,
        Sniffer = 4,
        // ? BlackKnight = 5, <- this is just an assumption and needs checked
        Stayne = 6
    }

    public struct CKHkAliceEnemy
    {
        private readonly byte _enemyType;
        private float _health;

        internal CKHkAliceEnemy(byte enemyType, float health)
        {
            _enemyType = enemyType;
            _health = health;
        }

        public readonly EnemyType EnemyType => (EnemyType)_enemyType;

        public float CurrentHealth
        {
            readonly get => _health;
            set => _health = value;
        }
        public readonly float MaxHealth => EnemyType switch
        {
            EnemyType.Spearman => 201f,
            EnemyType.Archer => 151f,
            EnemyType.Tank => 801f,
            EnemyType.Fortress => 201f,
            EnemyType.Sniffer => 151f,
            EnemyType.Stayne => 1500f,
            _ => 201f
        };
        public readonly float Percentage => CurrentHealth == 0f ? 0f : (CurrentHealth / MaxHealth) * 100;
        public readonly bool IsAlive => CurrentHealth > 0f;
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x1D0)]
    public readonly struct CKGrpSquad
    {
        [FieldOffset(0x28)] private readonly int firstEnemy;
        public readonly IntPtr FirstEnemy => (IntPtr)firstEnemy;
    }
}
