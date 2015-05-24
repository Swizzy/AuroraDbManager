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

        public IEnumerable<TitleUpdateItem> GetTitleUpdateItems() { return GetContentDataTable("SELECT * FROM TitleUpdates").Select().Select(row => new TitleUpdateItem(row)).ToArray(); }

        public class ContentItem {
            public enum ContentFlagValues {}

            public enum ContentGroupValues {}

            public enum ContentTypeValues {}

            public enum GameCapsFlagsValues {}

            public enum GenreFlagValues {}

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
                GamePlayers = new GameCaps((int)(long)row["GameCapsOnline"], (long)row["GameCapsOffline"]);
                GameCapsFlags = (GameCapsFlagsValues)((long)row["GameCapsFlags"]);
                FileType = (int)((long)row["FileType"]);
                ContentType = (ContentTypeValues)((long)row["ContentType"]);
                ContentGroup = (ContentGroupValues)((long)row["ContentGroup"]);
                DefaultGroup = (ContentGroupValues)((long)row["DefaultGroup"]);
                DateAdded = new Date((long)row["DateAdded"]);
                FoundAtDepth = (int)((long)row["FoundAtDepth"]);
                SystemLink = (int)((long)row["SystemLink"]);
                ScanPathId = (int)((long)row["ScanPathId"]);
            }

            public int BaseVersion { get; set; }

            public Date DateAdded { get; private set; }

            public string Directory { get; private set; }

            public int DiscsInSet { get; set; }

            public int DiscNum { get; set; }

            public string Executable { get; private set; }

            public int FileType { get; private set; }

            public int FoundAtDepth { get; private set; }

            public string Hash { get; private set; }

            public int Id { get; private set; }

            public int MediaId { get; set; }

            public int ScanPathId { get; private set; }

            public int SystemLink { get; set; }

            public int TitleId { get; set; }

            public ContentFlagValues ContentFlags { get; set; }

            public ContentGroupValues ContentGroup { get; set; }

            public ContentTypeValues ContentType { get; set; }

            public ContentGroupValues DefaultGroup { get; set; }

            public string Description { get; set; }

            public string Developer { get; set; }

            public GameCapsFlagsValues GameCapsFlags { get; set; }

            public GameCaps GamePlayers { get; set; }

            public GenreFlagValues GenreFlag { get; set; }

            public int LiveRaters { get; set; }

            public double LiveRating { get; set; }

            public string Publisher { get; set; }

            public string ReleaseDate { get; set; }

            public string TitleName { get; set; }

            public string TitleIdHex { get { return TitleId.ToString("X08"); } }

            public string MediaIdHex { get { return MediaId.ToString("X08"); } }

            public string BaseVersionHex { get { return BaseVersion.ToString("X08"); } }

            public class Date {
                public Date(long value) { }
            }

            public class GameCaps {
                public GameCaps(int online, long offline) {
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
                FileSize = (long)row["FileSize"]; // This isn't 100% correct, it's actually stored as "##.##kb", or "##.#MB" for instance... and not as an integer number, but trying to cast it to string causes a crash... dunno how to do this one?
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

            public long FileSize { get; private set; }

            public string LiveDeviceId { get; private set; }

            public string LivePath { get; set; }

            public string BackupPath { get; private set; }

            public string Hash { get; private set; }
        }
    }
}