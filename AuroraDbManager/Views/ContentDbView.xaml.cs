// 
// 	ContentDbView.xaml.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 17/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Views {
    using System;
    using System.Windows.Controls;
    using AuroraDbManager.Classes;
    using AuroraDbManager.Database;
    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for ContentDbView.xaml
    /// </summary>
    public partial class ContentDbView {
        private readonly AuroraDbManager _dbManager = new AuroraDbManager();
        public EventHandler<StatusEventArgs> StatusChanged;

        public ContentDbView() { InitializeComponent(); }

        public void OpenDb() {
            try {
                var ofd = new OpenFileDialog();
                if (ofd.ShowDialog() != true)
                    return;
                SendStatusChanged("Loading {0}...", ofd.FileName);
                _dbManager.ConnectToContent(ofd.FileName);
                Dispatcher.Invoke(new Action(() => DbViewBox.ItemsSource = _dbManager.GetContentItems()));
                SendStatusChanged("Finished loading Content DB...");
            }
            catch(Exception ex) {
                App.SaveException(ex);
                SendStatusChanged("Failed loading Content DB... Check error.log for more information...");
            }
        }

        private void DbViewChanged(object sender, SelectionChangedEventArgs e) {
            //TODO: Open Editor
        }

        private void SendStatusChanged(string msg, params object[] param) {
            var handler = StatusChanged;
            if(handler != null)
                handler.Invoke(this, new StatusEventArgs(string.Format(msg, param)));
        }
    }
}