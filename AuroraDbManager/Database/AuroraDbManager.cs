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
    using System.Threading;

    internal class AuroraDbManager {
        private SQLiteConnection _content;
        private ContentItem[] _contentItems;
        private SQLiteConnection _settings;
        private TitleUpdateItem[] _titleUpdateItems;

        public bool IsContentOpen { get { return _content != null; } }

        public bool IsSettingsOpen { get { return _settings != null; } }

        public void ConnectToContent(string path) {
            if(_content != null)
                _content.Close();
            _content = new SQLiteConnection("Data Source=\"" + path + "\";Version=3;");
            _content.Open();
            _contentItems = GetContentDataTable("SELECT * FROM ContentItems").Select().Select(row => new ContentItem(row)).ToArray();
            _titleUpdateItems = GetContentDataTable("SELECT CAST(FileSize AS TEXT) AS FileSize, * FROM TitleUpdates").Select().Select(row => new TitleUpdateItem(row)).ToArray();
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

        public IEnumerable<ContentItem> GetContentItems() { return _contentItems; }

        public IEnumerable<TitleUpdateItem> GetTitleUpdateItems() { return _titleUpdateItems; }

        public void UpdateTitleUpdateItem(TitleUpdateItem tuItem) {
            for(var i = 0; i < _titleUpdateItems.Length; i++) {
                if(_titleUpdateItems[i].Id != tuItem.Id)
                    continue;
                _titleUpdateItems[i] = tuItem;
                return;
            }
        }

        public void UpdateContentItem(ContentItem contentItem) {
            for(var i = 0; i < _contentItems.Length; i++) {
                if(_contentItems[i].Id != contentItem.Id)
                    continue;
                _contentItems[i] = contentItem;
                return;
            }
        }

        public void SaveContentChanges() {
            foreach(var contentItem in _contentItems) {
                if (!contentItem.Changed)
                    continue;
                //TODO: Make it save content Changes
            }
            foreach(var titleUpdateItem in _titleUpdateItems) {
                if (!titleUpdateItem.Changed)
                    continue;
                //TODO: Make it save titleupdate Changes
            }
        }

        public void SaveSettingsChanges() {
            //TODO: Make it save settings DB Changes
        }

        public void CloseContentDb() {
            if(!IsContentOpen)
                return;
            var isDisposed = false;
            _content.Disposed += (sender, args) => isDisposed = true;
            _content.Close();
            GC.Collect();
            while (!isDisposed)
                Thread.Sleep(10);
        }
        public void CloseSettingsDb()
        {
            if (!IsSettingsOpen)
                return;
            var isDisposed = false;
            _settings.Disposed += (sender, args) => isDisposed = true;
            _settings.Close();
            GC.Collect();
            while (!isDisposed)
                Thread.Sleep(10);
        }
    }
}