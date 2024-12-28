using SRTPluginProviderAlice.Structs.GameStructs;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;

namespace SRTPluginProviderAlice
{
    public class GameMemoryAlice: IGameMemoryAlice
    {
        public string GameName => "Alice in Wonderland";
        public string? VersionInfo => FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;
        public List<CKHkAliceEnemy> Enemies { get => _enemies; set => _enemies = value; }
        internal List<CKHkAliceEnemy> _enemies;

        public GameMemoryAlice()
        {
            _enemies = new List<CKHkAliceEnemy>();
        }
    }
}
