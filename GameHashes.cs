using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace SRTPluginProviderAlice
{
    public enum GameVersion: int
    {
        PCSteam,
        PCDVDROM,
        DolphinPAL,
        DolphinNTSC,
        Unknown
    };

    public static class GameHashes
    {
        //private static readonly byte[] alice_steam_2183777722952251792 = [0x73, 0xB5, 0xDC, 0x08, 0x9D, 0x2A, 0x96, 0x78, 0xA2, 0xBD, 0x51, 0x96, 0x47, 0x98, 0x6F, 0x23, 0xE5, 0xC7, 0x05, 0xAD, 0xDD, 0xA4, 0x98, 0x10, 0x29, 0x77, 0x4C, 0xA1, 0x79, 0x62, 0x5B, 0x68];
        private static readonly Dictionary<string, GameVersion> Hashes = new()
        {
            {
                "73b5dc089d2a9678a2bd519647986f23e5c705addda4981029774ca179625b68",
                GameVersion.PCSteam
            },
            {
                // Polish DVDROM
                "2578f6bcec330cc3b4c055fcfeea52258fb6d3d216787b1c547c4bb56cd628f6",
                GameVersion.PCDVDROM
            }
        };
        public static GameVersion DetectVersion(Process process, IntPtr baseAddr)
        {
            if (process.ProcessName.ToLower().Contains("dolphin"))
            {

            }
            else if (process.ProcessName.ToLower().Contains("alice"))
            {
                if (!File.Exists(process.MainModule.FileName))
                    return GameVersion.Unknown;
                byte[] checksum;
                using SHA256 hashFunc = SHA256.Create();
                using FileStream fs = new(process.MainModule.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
                    checksum = hashFunc.ComputeHash(fs);

                string hash = BitConverter.ToString(checksum).Replace("-", "").ToLowerInvariant();
                return GameHashes.Hashes[hash];
            }
            return GameVersion.Unknown;
        }
    }
}
