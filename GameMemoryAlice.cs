using SRTPluginProviderAlice.Structs.Enemy;
using SRTPluginProviderAlice.Structs.Player;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using SRTPluginProviderAlice.Structs.General;

namespace SRTPluginProviderAlice
{
    public class AliceHero
    {
        public HeroNumber HeroNumber { get; }
        public float CurrentHealth { get; set; }
        public float MaxHealth { get; set; }
        public float Percentage => CurrentHealth <= 0f ? 0f : (CurrentHealth / MaxHealth) * 100;
        public float PositionX { get; }
        public float PositionY { get; }
        public float PositionZ { get; }
        public PlayerCharacter CharacterType { get; }

        public AliceHero(CKHkAliceHero hero)
        {
            HeroNumber = hero.HeroNumber;
            CurrentHealth = -1;
            MaxHealth = -1;
            PositionX = hero.PositionX;
            PositionY = hero.PositionY;
            PositionZ = hero.PositionX;
            CharacterType = hero.CharacterType;
        }
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
            _heroes = new List<AliceHero>();
            _enemies = new List<CKHkAliceEnemy>();
        }
    }
}
