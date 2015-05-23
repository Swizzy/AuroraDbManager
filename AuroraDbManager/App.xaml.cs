// 
// 	App.xaml.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 14/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager {
    using System;
    using System.IO;
    using AuroraDbManager.Classes;
    using AuroraDbManager.Database;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        internal static readonly AuroraDbManager DbManager = new AuroraDbManager();
        internal static EventHandler<StatusEventArgs> StatusChanged;

        public static void SaveException(Exception ex) { File.AppendAllText("error.log", string.Format("[{0}]:{2}{1}{2}", DateTime.Now, ex, Environment.NewLine)); }
    }
}