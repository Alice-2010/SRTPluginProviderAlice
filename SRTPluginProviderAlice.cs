using SRTPluginBase;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace SRTPluginProviderAlice
{
    public class SRTPluginProviderAlice: IPluginProvider
    {
        private Process? process;
        private GameMemoryAliceScanner? gameMemoryScanner;
        private Stopwatch? stopwatch;
        private IPluginHostDelegates? hostDelegates;
        public IPluginInfo Info => new PluginInfo();

        public bool GameRunning
        {
            get
            {
                if (gameMemoryScanner != null && !gameMemoryScanner.ProcessRunning)
                {
                    process = GetProcess();
                    if (process != null)
                        gameMemoryScanner.Initialize(process);
                }
                return gameMemoryScanner != null && gameMemoryScanner.ProcessRunning;
            }
        }

        public int Startup(IPluginHostDelegates hostDelegates)
        {
            this.hostDelegates = hostDelegates;
            process = GetProcess();
            gameMemoryScanner = new GameMemoryAliceScanner(process);
            stopwatch = new Stopwatch();
            stopwatch.Start();
            return 0;
        }

        public int Shutdown()
        {
            gameMemoryScanner?.Dispose();
            gameMemoryScanner = null;
            stopwatch?.Stop();
            stopwatch = null;
            return 0;
        }

        public object? PullData()
        {
            try
            {
                if (!GameRunning)
                    return null;

                if (stopwatch?.ElapsedMilliseconds >= 2000L)
                {
                    gameMemoryScanner?.UpdatePointers();
                    stopwatch.Restart();
                }

                return gameMemoryScanner?.Refresh();
            }
            catch (Win32Exception ex)
            {
                if ((ProcessMemory.Win32Error)ex.NativeErrorCode != ProcessMemory.Win32Error.ERROR_PARTIAL_COPY)
                    hostDelegates?.ExceptionMessage(ex);
            }
            catch (Exception ex)
            {
                hostDelegates?.ExceptionMessage(ex);
            }

            return null;
        }

        private static Process? GetProcess()
        {
            // TODO: Add Dolphin Support
            Process? proc = Process.GetProcessesByName("Alice").Concat(Array.Empty<Process>()).FirstOrDefault();
            return proc;
        }
    }
}
