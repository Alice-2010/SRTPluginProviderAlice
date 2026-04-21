using SRTPluginProviderAlice.Structs.Enemy;
using SRTPluginProviderAlice.Structs.General;
using System.Collections.Generic;

namespace SRTPluginProviderAlice
{
    public interface IGameMemoryAlice
    {
        string GameName { get; }
        string? VersionInfo { get; }
        MapType Map { get; set; }
        int Sector { get; set; }
        float GameTime { get; set; }
        List<AliceHero> Heroes { get; set; }
        List<CKHkAliceEnemy> Enemies { get; set; }
    }
}
