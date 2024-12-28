using SRTPluginBase;
using System;
using System.Reflection;

namespace SRTPluginProviderAlice
{
    internal class PluginInfo : IPluginInfo
    {
        public string Name => "Game Memory Provider (Alice in Wonderland)";
        public string Description => "A game memory provider plugin for Alice in Wonderland (2010).";
        public string Author => "DeathHound6";
        public Uri MoreInfoURL => new("https://github.com/Alice-2010/SRTPluginProviderAlice");
        public int VersionMajor
        {
            get
            {
                if (assemblyVersion == null)
                    return 0;
                return assemblyVersion.Major;
            }
        }
        public int VersionMinor
        {
            get
            {
                if (assemblyVersion == null)
                    return 0;
                return assemblyVersion.Minor;
            }
        }
        public int VersionBuild
        {
            get
            {
                if (assemblyVersion == null)
                    return 0;
                return assemblyVersion.Build;
            }
        }
        public int VersionRevision
        {
            get
            {
                if (assemblyVersion == null)
                    return 0;
                return assemblyVersion.Revision;
            }
        }
        private readonly Version? assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
    }
}
