// 
// 	App.xaml.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 14/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager {
    using System;
    using System.IO;

    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App {
        public static void SaveException(Exception ex) { File.AppendAllText("error.log", string.Format("[{0}]:{2}{1}{2}", DateTime.Now, ex, Environment.NewLine)); }
    }
}