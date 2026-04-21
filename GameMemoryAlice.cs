using SRTPluginProviderAlice.Structs.Enemy;
using SRTPluginProviderAlice.Structs.Player;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using SRTPluginProviderAlice.Structs.General;

namespace SRTPluginProviderAlice
{
    public class AliceHero(CKHkAliceHero hero)
    {
        public HeroNumber HeroNumber { get; } = hero.HeroNumber;
        public float CurrentHealth { get; set; } = -1;
        public float MaxHealth { get; set; } = -1;
        public float Percentage => CurrentHealth <= 0f ? 0f : (CurrentHealth / MaxHealth) * 100;
        public float PositionX { get; } = hero.PositionX;
        public float PositionY { get; } = hero.PositionY;
        public float PositionZ { get; } = hero.PositionX;
        public PlayerCharacter CharacterType { get; } = hero.CharacterType;
    }
    public class GameMemoryAlice: IGameMemoryAlice
    {
        public string GameName => "Alice in Wonderland";
        public string? VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

        public MapType Map { get => _map; set => _map = value; }
        internal MapType _map;
        public int Sector { get => _sector; set => _sector = value; }
        internal int _sector;
        public float GameTime { get => _gameTime; set => _gameTime = value; }
        internal float _gameTime;
        public List<AliceHero> Heroes { get => _heroes; set => _heroes = value; }
        internal List<AliceHero> _heroes;
        public List<CKHkAliceEnemy> Enemies { get => _enemies; set => _enemies = value; }
        internal List<CKHkAliceEnemy> _enemies;

        public GameMemoryAlice()
        {
            Map = MapType.Loading;
            Sector = 0;
            _heroes = [];
            _enemies = [];
        }
    }
}
