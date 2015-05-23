// 
// 	MainWindow.xaml.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 14/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager {
    using System;
    using System.ComponentModel;
    using System.Reflection;
    using System.Windows;
    using AuroraDbManager.Views;

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        private readonly ContentDbView _contentDbView = new ContentDbView();
        private readonly SettingsDbView _settingsDbView = new SettingsDbView();

        public MainWindow() {
            InitializeComponent();
            var ver = Assembly.GetAssembly(typeof(MainWindow)).GetName().Version;
            Title = string.Format(Title, ver.Major, ver.Minor);
            ContentDbViewTab.Content = _contentDbView;
            _contentDbView.StatusChanged += (sender, args) => Dispatcher.Invoke(new Action(() => Status.Text = args.Status));
            SettingsDbViewTab.Content = _settingsDbView;
            _settingsDbView.StatusChanged += (sender, args) => Dispatcher.Invoke(new Action(() => Status.Text = args.Status));
        }

        private void SelectContentDb(object sender, RoutedEventArgs e) {
            var bw = new BackgroundWorker();
            bw.DoWork += (o, args) => _contentDbView.OpenDb();
            bw.RunWorkerAsync();
        }

        private void SelectSettingsDb(object sender, RoutedEventArgs e) {
            var bw = new BackgroundWorker();
            bw.DoWork += (o, args) => _settingsDbView.OpenDb();
            bw.RunWorkerAsync();
        }
    }
}