using System;
using System.Runtime.InteropServices;

namespace SRTPluginProviderAlice.Structs.Enemy
{
    public enum EnemyType : byte
    {
        Spearman = 0,
        Archer = 1,
        Tank = 2,
        Fortress = 3,
        Sniffer = 4,
        Stayne = 6
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1, Size = 0x458)]
    public struct CKHkAliceEnemy
    {
        [FieldOffset(0x14)] private readonly int _nextEnemy;
        [FieldOffset(0x6C)] private readonly byte _enemyType;
        [FieldOffset(0x3D0)] internal readonly float CurrentHealth;
        public readonly IntPtr NextEnemy => (IntPtr)_nextEnemy;
        public readonly EnemyType EnemyType => (EnemyType)_enemyType;
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
}
