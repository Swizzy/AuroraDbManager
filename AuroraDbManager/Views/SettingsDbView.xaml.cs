// 
// 	SettingsDbView.xaml.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 23/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Views {
    using System;
    using System.IO;
    using AuroraDbManager.Classes;
    using Microsoft.Win32;

    /// <summary>
    ///     Interaction logic for ContentDbView.xaml
    /// </summary>
    public partial class SettingsDbView {
        public SettingsDbView() { InitializeComponent(); }

        public void OpenDb(string filename = null) {
            try {
                if(string.IsNullOrWhiteSpace(filename)) {
                    var ofd = new OpenFileDialog();
                    if(ofd.ShowDialog() != true)
                        return;
                    filename = ofd.FileName;
                }
                if(!File.Exists(filename)) {
                    SendStatusChanged("ERROR: {0} Does not exist", filename);
                    return;
                }
                SendStatusChanged("Loading {0}...", filename);
                App.DbManager.ConnectToSettings(filename);
                //Dispatcher.Invoke(new Action(() => ContentDbViewBox.ItemsSource = App.DbManager.GetContentItems()));
                //TODO: Make it load all settings
                SendStatusChanged("Finished loading Settings DB...");
            }
            catch(Exception ex) {
                App.SaveException(ex);
                SendStatusChanged("Failed loading Settings DB... Check error.log for more information...");
            }
        }

        private void SendStatusChanged(string msg, params object[] param) {
            var handler = App.StatusChanged;
            if(handler != null)
                handler(this, new StatusEventArgs(string.Format(msg, param)));
        }
    }
}