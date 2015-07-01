// 
// 	ContentItem.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 25/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Database {
    using System;
    using System.Data;

    public class ContentItem {
        private long _offlineFlag;
        private long _onlineFlag;

        public ContentItem(DataRow row) {
            DataRow = row;
            _onlineFlag = (long)DataRow["GameCapsOnline"];
            _offlineFlag = (long)DataRow["GameCapsOffline"];
        }

        internal DataRow DataRow { get; private set; }

        internal bool Changed { get; private set; }

        public int BaseVersion {
            get { return (int)(long)DataRow["BaseVersion"]; }
            set {
                Changed = true;
                DataRow["BaseVersion"] = value;
            }
        }

        public DateTime DateAdded {
            get { return DateTime.FromFileTime((long)DataRow["DateAdded"]); }
            set {
                Changed = true;
                DataRow["DateAdded"] = value.ToFileTime();
            }
        }

        public string Directory {
            get { return (string)DataRow["Directory"]; }
            set {
                Changed = true;
                DataRow["Directory"] = value;
            }
        }

        public int DiscsInSet {
            get {
                var ret = (int)(long)DataRow["DiscsInSet"];
                return ret <= 0 ? 1 : ret;
            }
            set {
                Changed = true;
                DataRow["DiscsInSet"] = value;
            }
        }

        public int DiscNum {
            get {
                var ret = (int)(long)DataRow["DiscNum"];
                return ret <= 0 ? 1 : ret;
            }
            set {
                Changed = true;
                DataRow["DiscNum"] = value;
            }
        }

        public string DiscInfo { get { return string.Format("{0}/{1}", DiscNum, DiscsInSet); } }

        public string Executable { get { return (string)DataRow["Executable"]; } }

        public DbFlags.FileTypes FileType { get { return (DbFlags.FileTypes)DataRow["FileType"]; } }

        public int FoundAtDepth { get { return (int)(long)DataRow["FoundAtDepth"]; } }

        public string Hash { get { return (string)DataRow["Hash"]; } }

        public int Id { get { return (int)(long)DataRow["Id"]; } }

        public int MediaId {
            get { return (int)(long)DataRow["MediaId"]; }
            set {
                Changed = true;
                DataRow["MediaId"] = value;
            }
        }

        public int ScanPathId { get { return (int)(long)DataRow["ScanPathId"]; } }

        public bool SystemLink {
            get { return (long)DataRow["SystemLink"] == 1; }
            set {
                Changed = true;
                DataRow["SystemLink"] = value ? 1 : 0;
            }
        }

        public int TitleId {
            get { return (int)(long)DataRow["TitleId"]; }
            set {
                Changed = true;
                DataRow["TitleId"] = value;
            }
        }

        public DbFlags.ContentFlags ContentFlags {
            get { return (DbFlags.ContentFlags)(long)DataRow["ContentFlags"]; }
            set {
                Changed = true;
                DataRow["ContentFlags"] = (int)value;
            }
        }

        public DbFlags.ContentGroups ContentGroup {
            get { return (DbFlags.ContentGroups)(long)DataRow["ContentGroup"]; }
            set {
                Changed = true;
                DataRow["ContentGroup"] = (int)value;
            }
        }

        public DbFlags.ContentTypes ContentType {
            get { return (DbFlags.ContentTypes)(long)DataRow["ContentType"]; }
            set {
                Changed = true;
                DataRow["ContentType"] = (int)value;
            }
        }

        public DbFlags.ContentGroups DefaultGroup {
            get { return (DbFlags.ContentGroups)(long)DataRow["DefaultGroup"]; }
            set {
                Changed = true;
                DataRow["DefaultGroup"] = (int)value;
            }
        }

        public string Description {
            get { return (string)DataRow["Description"]; }
            set {
                Changed = true;
                DataRow["Description"] = value;
            }
        }

        public string Developer {
            get { return (string)DataRow["Developer"]; }
            set {
                Changed = true;
                DataRow["Developer"] = value;
            }
        }

        public DbFlags.GameCapsFlags GameCapsFlags {
            get { return (DbFlags.GameCapsFlags)(long)DataRow["GameCapsFlags"]; }
            set {
                Changed = true;
                DataRow["GameCapsFlags"] = (int)value;
            }
        }

        public DbFlags.GenreFlags GenreFlag {
            get { return (DbFlags.GenreFlags)DataRow["GenreFlag"]; }
            set {
                Changed = true;
                DataRow["GenreFlag"] = (int)value;
            }
        }

        public int LiveRaters {
            get { return (int)(long)DataRow["LiveRaters"]; }
            set {
                Changed = true;
                DataRow["LiveRaters"] = value;
            }
        }

        public double LiveRating {
            get { return (double)DataRow["LiveRating"]; }
            set {
                Changed = true;
                DataRow["LiveRating"] = value;
            }
        }

        public string Publisher {
            get { return (string)DataRow["Publisher"]; }
            set {
                Changed = true;
                DataRow["Publisher"] = value;
            }
        }

        public string ReleaseDate {
            get { return (string)DataRow["ReleaseDate"]; }
            set {
                Changed = true;
                DataRow["ReleaseDate"] = value;
            }
        }

        public string TitleName {
            get { return (string)DataRow["TitleName"]; }
            set {
                Changed = true;
                DataRow["TitleName"] = value;
            }
        }

        public string OnlineMultiplayerPlayers { get { return string.Format("{0} - {1}", MinimumOnlineMultiplayerPlayers, MaximumOnlineMultiplayerPlayers); } }

        public string OnlineCoOpPlayers { get { return string.Format("{0} - {1}", MinimumOnlineCoOpPlayers, MaximumOnlineCoOpPlayers); } }

        public string OfflinePlayers { get { return string.Format("{0} - {1}", MinimumOfflinePlayers, MaximumOfflinePlayers); } }

        public string OfflineCoOpPlayers { get { return string.Format("{0} - {1}", MinimumOfflineCoOpPlayers, MaximumOfflineCoOpPlayers); } }

        public string OfflineSystemLinkPlayers { get { return string.Format("{0} - {1}", MinimumOfflineSystemLinkPlayers, MaximumOfflineSystemLinkPlayers); } }

        public byte MaximumOnlineCoOpPlayers { get { return (byte)(_onlineFlag & 0xFF); } set { SetOnlineFlag(MaximumOnlineCoOpPlayers, value); } }

        public byte MinimumOnlineCoOpPlayers { get { return (byte)((_onlineFlag >> 8) & 0xFF); } set { SetOnlineFlag(MinimumOnlineCoOpPlayers, (long)value << 8); } }

        public byte MaximumOnlineMultiplayerPlayers { get { return (byte)((_onlineFlag >> 24) & 0xFF); } set { SetOnlineFlag(MaximumOnlineMultiplayerPlayers, (long)value << 24); } }

        public byte MinimumOnlineMultiplayerPlayers { get { return (byte)((_onlineFlag >> 16) & 0xFF); } set { SetOnlineFlag(MinimumOnlineMultiplayerPlayers, (long)value << 16); } }

        public byte MinimumOfflineSystemLinkPlayers { get { return (byte)((_offlineFlag >> 40) & 0xFF); } set { SetOfflineFlag(MinimumOfflineSystemLinkPlayers, (long)value << 40); } }

        public byte MaximumOfflineSystemLinkPlayers { get { return (byte)((_offlineFlag >> 40) & 0xFF); } set { SetOfflineFlag(MaximumOfflineSystemLinkPlayers, (long)value << 40); } }

        public byte MinimumOfflineCoOpPlayers { get { return (byte)((_offlineFlag >> 16) & 0xFF); } set { SetOfflineFlag(MinimumOfflineCoOpPlayers, (long)value << 16); } }

        public byte MaximumOfflineCoOpPlayers { get { return (byte)((_offlineFlag >> 24) & 0xFF); } set { SetOfflineFlag(MaximumOfflineCoOpPlayers, (long)value << 24); } }

        public byte MinimumOfflinePlayers {
            get {
                var ret = _offlineFlag & 0xFF;
                return (byte)(ret <= 0 ? 1 : ret);
            }
            set { SetOfflineFlag(MinimumOfflinePlayers, value); }
        }

        public byte MaximumOfflinePlayers {
            get {
                var ret = (_offlineFlag >> 8) & 0xFF;
                return (byte)(ret <= 0 ? 1 : ret);
            }
            set { SetOfflineFlag(MaximumOfflinePlayers, (long)value << 8); }
        }

        private void SetOnlineFlag(long removeMask, long addMask) {
            Changed = true;
            _onlineFlag &= ~removeMask; // Remove old data
            DataRow["GameCapsOnline"] = _onlineFlag |= addMask; // Set new data;
        }

        private void SetOfflineFlag(long removeMask, long addMask) {
            Changed = true;
            _offlineFlag &= ~removeMask; // Remove old data
            DataRow["GameCapsOffline"] = _offlineFlag |= addMask; // Set new data;
        }
    }
}