using SRTPluginProviderAlice.Structs.GameStructs;
using System.Collections.Generic;

namespace SRTPluginProviderAlice
{
    public interface IGameMemoryAlice
    {
        string GameName { get; }
        string? VersionInfo { get; }
        List<CKHkAliceEnemy> Enemies { get; set; }
    }
}
