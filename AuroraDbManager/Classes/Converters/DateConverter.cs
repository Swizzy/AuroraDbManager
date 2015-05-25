// 
// 	DateConverter.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 25/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Classes.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(string))] internal class DateConverter: BaseConverter, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(!(value is DateTime))
                return "N/A";
            return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}