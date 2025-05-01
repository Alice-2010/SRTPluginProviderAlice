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
        public int ProcessExitCode => (memoryAccess != null) ? memoryAccess.ProcessExitCode : 0;

        // Pointers
        private IntPtr BaseAddress { get; set; }
        private MultilevelPointer PointerGameManager { get; set; }
        private CKGrpAliceHero heroGroup { get; set; }
        private CKGrpAliceEnemy enemyGroup { get; set; }
        private CKAliceGameStructure gameStructure { get; set; }
        private CKLevel level { get; set; }

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
            gameVersion = GameHashes.DetectVersion(process.MainModule?.FileName ?? "");
            if (gameVersion == GameVersion.Unknown)
                return;

            int pid = process.Id;
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = process?.MainModule?.BaseAddress ?? IntPtr.Zero;

                // TODO: Add Dolphin support
                // TODO: Add DVDROM support?
                PointerGameManager = new MultilevelPointer(memoryAccess, (IntPtr)(BaseAddress + 0x44B8A8), 0x8C);
            }
        }

        internal void UpdatePointers()
        {
            PointerGameManager.UpdatePointers();
        }

        private unsafe void UpdateGeneralInfo()
        {
            CKAliceGameManager gameManager = PointerGameManager.Deref<CKAliceGameManager>(0x0);
            this.level = memoryAccess.GetAt<CKLevel>((IntPtr)gameManager._level);
            this.gameStructure = memoryAccess.GetAt<CKAliceGameStructure>((IntPtr)gameManager._structure);
            this.heroGroup = memoryAccess.GetAt<CKGrpAliceHero>((IntPtr)gameManager._heroGroup);
            this.enemyGroup = memoryAccess.GetAt<CKGrpAliceEnemy>((IntPtr)gameManager._enemyGroup);

            gameMemoryValues.Map = gameManager.MapType;
            gameMemoryValues.Sector = this.level.Sector;
            
        }

        private unsafe void UpdatePlayers()
        {
            IntPtr extraHealthItemPtr = memoryAccess.GetAt<IntPtr>(this.gameStructure.InventoryItemsList);
            CKAliceInventoryItem extraHealthItem = memoryAccess.GetAt<CKAliceInventoryItem>(extraHealthItemPtr);
            CKGameLevelCollectible collectible = memoryAccess.GetAt<CKGameLevelCollectible>(extraHealthItem.GameLevelCollectible);
            float maxHealth = collectible.Collected && collectible.Bought ? 200 : 100;
            List<AliceHero> players = new();
            IntPtr heroPtr = this.heroGroup.FirstPlayer;
            while (heroPtr != IntPtr.Zero)
            {
                CKHkAliceHero hero = memoryAccess.GetAt<CKHkAliceHero>(heroPtr);
                AliceHero aliceHero = new(hero);
                if (hero.HeroNumber == HeroNumber.Player1 || hero.HeroNumber == HeroNumber.Player2)
                {
                    aliceHero.CurrentHealth = hero.HeroNumber == HeroNumber.Player1 ? this.gameStructure.Player1Health : this.gameStructure.Player2Health;
                    aliceHero.MaxHealth = maxHealth;
                }
                players.Add(aliceHero);
                heroPtr = (IntPtr)hero._nextPlayer;
            }
            players.Reverse();
            gameMemoryValues.Heroes = players;
        }

        private unsafe void UpdateEnemies()
        {
            List<CKHkAliceEnemy> enemies = new();
            CKGrpFightZone fightZone = memoryAccess.GetAt<CKGrpFightZone>(this.enemyGroup.FightZone);
            IntPtr squadPtr = fightZone.FirstSquad;
            while (squadPtr != IntPtr.Zero)
            {
                CKGrpSquad squad = memoryAccess.GetAt<CKGrpSquad>(squadPtr);
                IntPtr enemyPtr = squad.FirstEnemy;
                while (enemyPtr != IntPtr.Zero)
                {
                    CKHkAliceEnemy enemy = memoryAccess.GetAt<CKHkAliceEnemy>(enemyPtr);
                    enemies.Add(enemy);
                    enemyPtr = (IntPtr)enemy._nextEnemy;
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
