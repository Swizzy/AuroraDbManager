// 
// 	DbFlags.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 25/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Database {
    using System;

    public static class DbFlags {
        [Flags] public enum ContentFlags {
            KinectCompatible = 1,
            SystemLinkCompatible = 2,
            RetailSigned = 4,
            DevkitSigned = 8,
        }

        [Flags] public enum ContentGroups {
            Hidden = 0,
            Xbox360 = 1,
            Xbla = 2,
            Indie = 3,
            XboxClassic = 4,
            Unsigned = 5,
            LibXenon = 6,
            Count = 7
        }

        [Flags] public enum ContentTypes {}

        [Flags] public enum FileTypes {
            Xex = 1,
            GamesOnDemand = 3
        }

        [Flags] public enum GameCapsFlags {
            None = 0,
            DolbyDigitalSupported = 1,
            RequiresHddOnline = 2,
            RequiresHddOffline = 4,
            OnlineLeaderBoards = 8,
            OnlineContentDownload = 16,
            OnlineVoice = 32
        }

        [Flags] public enum GenreFlags {
            Unknown = 0,
            Other = 1,
            ActionAdventure = 2,
            Family = 4,
            Fighting = 8,
            Music = 16,
            Platformer = 32,
            RacingFlying = 64,
            RolePlaying = 128,
            Shooter = 256,
            StrategySimulation = 512,
            SportsRecreation = 1024,
            CardBoard = 2048,
            Classics = 4096,
            PuzzleTrivia = 8192
        }

        [Flags] public enum ScanpathOptions: long {
            GameTitle = 0x1,
            SavedGame = 0x2,
            TitleUpdate = 0x4,
            Profile = 0x8,
            Nxe2God = 0x10,
            Dlc = 0x20,
            AllContent = GameTitle | SavedGame | TitleUpdate | SavedGame | Profile | Nxe2God | Dlc,
            IgnorePaths = 0x1000,
            IncludeDlc = 0x2000,
            IncludeAvatar = 0x4000,
            IncludeQuickboot = 0x8000,
            IgnoreDefault = 0x10000,
            ContentPath = 0x20000,
            End = 0x80000000
        }
    }
}