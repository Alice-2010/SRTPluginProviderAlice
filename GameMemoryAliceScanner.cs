using ProcessMemory;
using SRTPluginProviderAlice.Structs.Enemy;
using SRTPluginProviderAlice.Structs.General;
using SRTPluginProviderAlice.Structs.Player;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SRTPluginProviderAlice
{
    internal class GameMemoryAliceScanner: IDisposable
    {
        private ProcessMemoryHandler memoryAccess;
        private readonly GameMemoryAlice gameMemoryValues;
        private GameVersion gameVersion;
        public bool HasScanned;
        public bool ProcessRunning => memoryAccess != null && memoryAccess.ProcessRunning;
        public uint ProcessExitCode => memoryAccess != null ? memoryAccess.ProcessExitCode : 0;

        // Pointers
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerGameManager { get; set; }
        private CKGrpAliceHero HeroGroup { get; set; }
        private CKGrpAliceEnemy EnemyGroup { get; set; }
        private CKAliceGameStructure GameStructure { get; set; }
        private CKLevel Level { get; set; }
        private readonly Dictionary<GameVersion, int> BaseAddresses = new()
        {
            {
                GameVersion.PCSteam, 0x44B8A8
            },
            {
                // Polish DVDROM
                GameVersion.PCDVDROM, 0xAA3948
            }
        };

        internal GameMemoryAliceScanner(Process? process = null)
        {
            gameMemoryValues = new GameMemoryAlice();
            if (process != null)
                Initialize(process);
        }

        internal unsafe void Initialize(Process process)
        {
            if (process == null)
                return;

            uint pid = (uint)process.Id;
            this.memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                this.BaseAddress = process?.MainModule?.BaseAddress ?? IntPtr.Zero;
                if (this.BaseAddress == IntPtr.Zero)
                    return;

                if (process?.ProcessName.ToLower().Contains("dolphin") ?? false)
                {
                    foreach (MemoryBasicInformation item in process.MemoryPages(true))
                    {
                        if (item.Type == MemPageType.MEM_MAPPED && item.AllocationProtect == MemPageProtect.PAGE_READWRITE &&
                            item.State == MemPageState.MEM_COMMIT && item.Protect == MemPageProtect.PAGE_READWRITE && (int)item.RegionSize == 0x2000000)
                        {
                            this.BaseAddress = item.BaseAddress;
                            break;
                        }
                    }
                }

                this.gameVersion = GameHashes.DetectVersion(process, BaseAddress);
                if (this.gameVersion == GameVersion.Unknown)
                    return;

                // TODO: Add Dolphin support
                this.PointerGameManager = new MultilevelPointer(this.memoryAccess, (nint*)(this.BaseAddress + this.BaseAddresses[this.gameVersion]), 0x8C);
            }
        }

        internal void UpdatePointers()
        {
            PointerGameManager.UpdatePointers();
        }

        private unsafe void UpdateGeneralInfo()
        {
            CKAliceGameManager gameManager = PointerGameManager.Deref<CKAliceGameManager>(0x0);
            this.Level = memoryAccess.GetAt<CKLevel>((nint*)gameManager.Level);
            this.GameStructure = memoryAccess.GetAt<CKAliceGameStructure>((nint*)gameManager.Structure);
            this.HeroGroup = memoryAccess.GetAt<CKGrpAliceHero>((nint*)gameManager.HeroGroup);
            this.EnemyGroup = memoryAccess.GetAt<CKGrpAliceEnemy>((nint*)gameManager.EnemyGroup);
            CKAlicePlayer player = memoryAccess.GetAt<CKAlicePlayer>((nint*)gameManager.Player2);

            gameMemoryValues.Map = gameManager.MapType;
            gameMemoryValues.Sector = this.Level.Sector;
            gameMemoryValues.GameTime = player.GameTime;
        }

        private unsafe void UpdatePlayers()
        {
            IntPtr extraHealthItemPtr = memoryAccess.GetAt<IntPtr>((nint*)this.GameStructure.InventoryItemsList);
            CKAliceInventoryItem extraHealthItem = memoryAccess.GetAt<CKAliceInventoryItem>((nint*)extraHealthItemPtr);
            CKGameLevelCollectible collectible = memoryAccess.GetAt<CKGameLevelCollectible>((nint*)extraHealthItem.GameLevelCollectible);
            float maxHealth = collectible.Collected && collectible.Bought ? 200 : 100;
            List<AliceHero> players = [];
            IntPtr heroPtr = this.HeroGroup.FirstPlayer;
            while (heroPtr != IntPtr.Zero)
            {
                CKHkAliceHero hero = memoryAccess.GetAt<CKHkAliceHero>((nint*)heroPtr);
                AliceHero aliceHero = new(hero);
                if (hero.HeroNumber == HeroNumber.Player1 || hero.HeroNumber == HeroNumber.Player2)
                {
                    aliceHero.CurrentHealth = hero.HeroNumber == HeroNumber.Player1 ? this.GameStructure.Player1Health : this.GameStructure.Player2Health;
                    aliceHero.MaxHealth = maxHealth;
                }
                players.Add(aliceHero);
                heroPtr = hero.NextPlayer;
            }
            players.Reverse();
            gameMemoryValues.Heroes = players;
        }

        private unsafe void UpdateEnemies()
        {
            List<CKHkAliceEnemy> enemies = [];
            CKGrpFightZone fightZone = memoryAccess.GetAt<CKGrpFightZone>((nint*)this.EnemyGroup.FightZone);
            IntPtr squadPtr = fightZone.FirstSquad;
            while (squadPtr != IntPtr.Zero)
            {
                CKGrpSquad squad = memoryAccess.GetAt<CKGrpSquad>((nint*)squadPtr);
                IntPtr enemyPtr = squad.FirstEnemy;
                while (enemyPtr != IntPtr.Zero)
                {
                    CKHkAliceEnemy enemy = memoryAccess.GetAt<CKHkAliceEnemy>((nint*)enemyPtr);
                    enemies.Add(enemy);
                    enemyPtr = enemy.NextEnemy;
                }
                squadPtr = squad.NextSquad;
            }

            gameMemoryValues.Enemies = enemies;
        }

        internal unsafe IGameMemoryAlice Refresh()
        {
            // NOTE: This sets structs for later update calls. Ensure general info is updated first.
            UpdateGeneralInfo();
            UpdateEnemies();
            UpdatePlayers();
            HasScanned = true;
            return gameMemoryValues;
        }

        #region IDisposable Support
        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    memoryAccess?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
