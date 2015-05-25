// 
// 	AuroraDbManager.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 14/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Database {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SQLite;
    using System.Linq;

    internal class AuroraDbManager {
        private SQLiteConnection _content, _settings;

        public bool IsContentOpen { get { return _content != null; } }

        public bool IsSettingsOpen { get { return _settings != null; } }

        public void ConnectToContent(string path) {
            if(_content != null)
                _content.Close();
            _content = new SQLiteConnection("Data Source=\"" + path + "\";Version=3;");
            _content.Open();
        }

        public void ConnectToSettings(string path) {
            if(_settings != null)
                _settings.Close();
            _settings = new SQLiteConnection("Data Source=\"" + path + "\";Version=3;");
            _settings.Open();
        }

        private DataTable GetContentDataTable(string sql) {
            var dt = new DataTable();
            try {
                var cmd = new SQLiteCommand(sql, _content);
                using(var reader = cmd.ExecuteReader())
                    dt.Load(reader);
            }
            catch(Exception ex) {
                App.SaveException(ex);
            }
            return dt;
        }

        private DataTable GetSettingsDataTable(string sql) {
            var dt = new DataTable();
            try {
                var cmd = new SQLiteCommand(sql, _settings);
                using(var reader = cmd.ExecuteReader())
                    dt.Load(reader);
            }
            catch(Exception ex) {
                App.SaveException(ex);
            }
            return dt;
        }

        private int ExecuteNonQueryContent(string sql) {
            try {
                var cmd = new SQLiteCommand(sql, _content);
                return cmd.ExecuteNonQuery();
            }
            catch(Exception ex) {
                App.SaveException(ex);
                return 0;
            }
        }

        private int ExecuteNonQuerySettings(string sql) {
            try {
                var cmd = new SQLiteCommand(sql, _content);
                return cmd.ExecuteNonQuery();
            }
            catch(Exception ex) {
                App.SaveException(ex);
                return 0;
            }
        }

        public IEnumerable<ContentItem> GetContentItems() { return GetContentDataTable("SELECT * FROM ContentItems").Select().Select(row => new ContentItem(row)).ToArray(); }

        public IEnumerable<TitleUpdateItem> GetTitleUpdateItems() {
            return GetContentDataTable("SELECT CAST(FileSize AS TEXT) AS FileSize, * FROM TitleUpdates").Select().Select(row => new TitleUpdateItem(row)).ToArray();
        }

        public class ContentItem {
            [Flags] public enum ContentFlagValues {
                KinectCompatible = 1,
                SystemLinkCompatible = 2,
                RetailSigned = 4,
                DevkitSigned = 8,
            }

            [Flags] public enum ContentGroupValues {
                Hidden = 0,
                Xbox360 = 1,
                Xbla = 2,
                Indie = 3,
                XboxClassic = 4,
                Unsigned = 5,
                LibXenon = 6,
                Count = 7
            }

            [Flags] public enum ContentTypeValues {}

            [Flags] public enum FileTypes {
                Xex = 1,
                GamesOnDemand = 3
            }

            [Flags] public enum GameCapsFlagsValues {
                None = 0,
                DolbyDigitalSupported = 1,
                RequiresOnlineHdd = 2,
                RequiresOfflineHdd = 4,
                OnlineLeaderBoards = 8,
                OnlineContentDownload = 16,
                OnlineVoice = 32
            }

            [Flags] public enum GenreFlagValues {
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

            public ContentItem(DataRow row) {
                Id = (int)((long)row["Id"]);
                Directory = (string)row["Directory"];
                Executable = (string)row["Executable"];
                TitleId = (int)((long)row["TitleId"]);
                MediaId = (int)((long)row["MediaId"]);
                BaseVersion = (int)((long)row["BaseVersion"]);
                DiscNum = (int)((long)row["DiscNum"]);
                if(DiscNum <= 0)
                    DiscNum = 1;
                DiscsInSet = (int)((long)row["DiscsInSet"]);
                if(DiscsInSet <= 0)
                    DiscsInSet = 1;
                TitleName = (string)row["TitleName"];
                Description = (string)row["Description"];
                Publisher = (string)row["Publisher"];
                Developer = (string)row["Developer"];
                LiveRating = (double)row["LiveRating"];
                LiveRaters = (int)((long)row["LiveRaters"]);
                ReleaseDate = (string)row["ReleaseDate"];
                GenreFlag = (GenreFlagValues)((long)row["GenreFlag"]);
                ContentFlags = (ContentFlagValues)((long)row["ContentFlags"]);
                Hash = (string)row["Hash"];
                GamePlayers = new GamePlayerCaps((int)(long)row["GameCapsOnline"], (long)row["GameCapsOffline"]);
                GameCapsFlags = (GameCapsFlagsValues)((long)row["GameCapsFlags"]);
                FileType = (FileTypes)((long)row["FileType"]);
                ContentType = (ContentTypeValues)((long)row["ContentType"]);
                ContentGroup = (ContentGroupValues)((long)row["ContentGroup"]);
                DefaultGroup = (ContentGroupValues)((long)row["DefaultGroup"]);
                DateAdded = DateTime.FromFileTime((long)row["DateAdded"]);
                FoundAtDepth = (int)((long)row["FoundAtDepth"]);
                SystemLink = (long)row["SystemLink"] == 1;
                ScanPathId = (int)((long)row["ScanPathId"]);
            }

            public int BaseVersion { get; set; }

            public DateTime DateAdded { get; private set; }

            public string Directory { get; private set; }

            public int DiscsInSet { get; set; }

            public int DiscNum { get; set; }

            public string DiscInfo { get { return string.Format("{0}/{1}", DiscNum, DiscsInSet); } }

            public string Executable { get; private set; }

            public FileTypes FileType { get; private set; }

            public int FoundAtDepth { get; private set; }

            public string Hash { get; private set; }

            public int Id { get; private set; }

            public int MediaId { get; set; }

            public int ScanPathId { get; private set; }

            public bool SystemLink { get; set; }

            public int TitleId { get; set; }

            public ContentFlagValues ContentFlags { get; set; }

            public ContentGroupValues ContentGroup { get; set; }

            public ContentTypeValues ContentType { get; set; }

            public ContentGroupValues DefaultGroup { get; set; }

            public string Description { get; set; }

            public string Developer { get; set; }

            public GameCapsFlagsValues GameCapsFlags { get; set; }

            public GamePlayerCaps GamePlayers { get; set; }

            public GenreFlagValues GenreFlag { get; set; }

            public int LiveRaters { get; set; }

            public double LiveRating { get; set; }

            public string Publisher { get; set; }

            public string ReleaseDate { get; set; }

            public string TitleName { get; set; }

            public class GamePlayerCaps {
                public GamePlayerCaps(int online, long offline) {
                    MinimumOnlineMultiplayerPlayers = (byte)((online & 0x00FF0000) >> 16);
                    MaximumOnlineMultiplayerPlayers = (byte)((online & 0xFF000000) >> 24);
                    MinimumOnlineCoOpPlayers = (byte)(online & 0x000000FF);
                    MaximumOnlineCoOpPlayers = (byte)((online & 0x0000FF00) >> 8);
                    MinimumOfflinePlayers = (byte)(offline & 0x000000FF);
                    if(MinimumOfflinePlayers <= 0)
                        MinimumOfflinePlayers = 1;
                    MaximumOfflinePlayers = (byte)((offline & 0x0000FF00) >> 8);
                    if(MaximumOfflinePlayers <= 0)
                        MaximumOfflinePlayers = 1;
                    MinimumOfflineCoOpPlayers = (byte)((offline & 0x000000FF) >> 16);
                    MaximumOfflineCoOpPlayers = (byte)((offline & 0x0000FF00) >> 24);
                    MinimumOfflineSystemLinkPlayers = (byte)((offline >> 32) & 0xFF);
                    MaximumOfflineSystemLinkPlayers = (byte)((offline >> 40) & 0xFF);
                }

                public string OnlineMultiplayerPlayers { get { return string.Format("{0} - {1}", MinimumOnlineMultiplayerPlayers, MaximumOnlineMultiplayerPlayers); } }

                public string OnlineCoOpPlayers { get { return string.Format("{0} - {1}", MinimumOnlineCoOpPlayers, MaximumOnlineCoOpPlayers); } }

                public string OfflinePlayers { get { return string.Format("{0} - {1}", MinimumOfflinePlayers, MaximumOfflinePlayers); } }

                public string OfflineCoOpPlayers { get { return string.Format("{0} - {1}", MinimumOfflineCoOpPlayers, MaximumOfflineCoOpPlayers); } }

                public string OfflineSystemLinkPlayers { get { return string.Format("{0} - {1}", MinimumOfflineSystemLinkPlayers, MaximumOfflineSystemLinkPlayers); } }

                public byte MaximumOnlineCoOpPlayers { get; set; }

                public byte MinimumOnlineCoOpPlayers { get; set; }

                public byte MaximumOnlineMultiplayerPlayers { get; set; }

                public byte MinimumOnlineMultiplayerPlayers { get; set; }

                public byte MinimumOfflineSystemLinkPlayers { get; set; }

                public byte MaximumOfflineSystemLinkPlayers { get; set; }

                public byte MinimumOfflineCoOpPlayers { get; set; }

                public byte MaximumOfflineCoOpPlayers { get; set; }

                public byte MinimumOfflinePlayers { get; set; }

                public byte MaximumOfflinePlayers { get; set; }
            }
        }

        public class TitleUpdateItem {
            public TitleUpdateItem(DataRow row) {
                Id = (int)((long)row["Id"]);
                DisplayName = (string)row["DisplayName"];
                TitleId = (int)((long)row["TitleId"]);
                MediaId = (int)((long)row["MediaId"]);
                BaseVersion = (int)((long)row["BaseVersion"]);
                Version = (int)((long)row["Version"]);
                FileName = (string)row["FileName"];
                FileSize = (string)row["FileSize"];
                LiveDeviceId = (string)row["LiveDeviceId"];
                LivePath = (string)row["LivePath"];
                BackupPath = (string)row["BackupPath"];
                Hash = (string)row["Hash"];
            }

            public int Id { get; private set; }

            public string DisplayName { get; private set; }

            public int TitleId { get; set; }

            public int MediaId { get; set; }

            public int BaseVersion { get; set; }

            public int Version { get; set; }

            public string FileName { get; set; }

            public string FileSize { get; private set; }

            public string LiveDeviceId { get; private set; }

            public string LivePath { get; set; }

            public string BackupPath { get; private set; }

            public string Hash { get; private set; }
        }
    }
}