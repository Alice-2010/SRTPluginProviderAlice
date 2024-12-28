using ProcessMemory;
using SRTPluginProviderAlice.Structs.GameStructs;
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
        private MultilevelPointer PointerGroupSquad { get; set; }

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
            gameVersion = GameHashes.DetectVersion(process.MainModule.FileName);
            if (gameVersion == GameVersion.Unknown)
                return;

            int pid = process.Id;
            memoryAccess = new ProcessMemoryHandler(pid);
            if (ProcessRunning)
            {
                BaseAddress = process?.MainModule?.BaseAddress ?? IntPtr.Zero;

                PointerGroupSquad = new MultilevelPointer(memoryAccess, (IntPtr)(BaseAddress + 0x44B8A8), 0x8C, 0x4, 0x30, 0x18);
            }
        }

        internal void UpdatePointers()
        {
            PointerGroupSquad.UpdatePointers();
        }

        private unsafe void UpdateEnemies()
        {
            List<CKHkAliceEnemy> enemies = new();
            CKGrpSquad squad = PointerGroupSquad.Deref<CKGrpSquad>(0x0);
            IntPtr enemyPtr = squad.FirstEnemy;
            while (enemyPtr != IntPtr.Zero)
            {
                byte enemyType = memoryAccess.GetByteAt((IntPtr)(enemyPtr + 0x6C));
                float health = memoryAccess.GetFloatAt((IntPtr)(enemyPtr + 0x3D0));
                CKHkAliceEnemy enemy = new(enemyType, health);
                enemies.Add(enemy);
                // Offset 0x14 is equivalent of "next"
                enemyPtr = (IntPtr)memoryAccess.GetIntAt((IntPtr)(enemyPtr + 0x14));
            }

            gameMemoryValues.Enemies = enemies;
        }

        internal unsafe IGameMemoryAlice Refresh()
        {
            UpdateEnemies();
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
