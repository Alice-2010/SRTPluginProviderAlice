using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SRTPluginProviderAlice
{
    public enum GameVersion: int
    {
        SteamMarch2010,
        Unknown
    };

    public static class GameHashes
    {
        private static readonly byte[] alice_steamMarch2010 = new byte[32] { 0x73, 0xB5, 0xDC, 0x08, 0x9D, 0x2A, 0x96, 0x78, 0xA2, 0xBD, 0x51, 0x96, 0x47, 0x98, 0x6F, 0x23, 0xE5, 0xC7, 0x05, 0xAD, 0xDD, 0xA4, 0x98, 0x10, 0x29, 0x77, 0x4C, 0xA1, 0x79, 0x62, 0x5B, 0x68 };
        public static GameVersion DetectVersion(string filePath)
        {
            byte[] checksum;
            using SHA256 hashFunc = SHA256.Create();
            using FileStream fs = new(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
            checksum = hashFunc.ComputeHash(fs);

            if (checksum.SequenceEqual(alice_steamMarch2010))
                return GameVersion.SteamMarch2010;
            else
                return GameVersion.Unknown;
        }
    }
}
