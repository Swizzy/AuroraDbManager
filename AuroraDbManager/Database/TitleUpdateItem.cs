// 
// 	TitleUpdateItem.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 25/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Database {
    using System.Data;

    public class TitleUpdateItem {
        public TitleUpdateItem(DataRow row) { DataRow = row; }

        internal DataRow DataRow { get; private set; }

        internal bool Changed { get; private set; }

        public int Id { get { return (int)((long)DataRow["Id"]); } }

        public string DisplayName {
            get { return (string)DataRow["DisplayName"]; }
            set {
                Changed = true;
                DataRow["DisplayName"] = value;
            }
        }

        public int TitleId {
            get { return (int)((long)DataRow["TitleId"]); }
            set {
                Changed = true;
                DataRow["TitleId"] = value;
            }
        }

        public int MediaId {
            get { return (int)((long)DataRow["MediaId"]); }
            set {
                Changed = true;
                DataRow["MediaId"] = value;
            }
        }

        public int BaseVersion {
            get { return (int)((long)DataRow["BaseVersion"]); }
            set {
                Changed = true;
                DataRow["BaseVersion"] = value;
            }
        }

        public int Version {
            get { return (int)((long)DataRow["Version"]); }
            set {
                Changed = true;
                DataRow["Version"] = value;
            }
        }

        public string FileName {
            get { return (string)DataRow["FileName"]; }
            set {
                Changed = true;
                DataRow["FileName"] = value;
            }
        }

        public string FileSize { get { return (string)DataRow["FileSize"]; } }

        public string LiveDeviceId { get { return (string)DataRow["LiveDeviceId"]; } }

        public string LivePath {
            get { return (string)DataRow["LivePath"]; }
            set {
                Changed = true;
                DataRow["LivePath"] = value;
            }
        }

        public string BackupPath { get { return (string)DataRow["BackupPath"]; } }

        public string Hash { get { return (string)DataRow["Hash"]; } }
    }
}