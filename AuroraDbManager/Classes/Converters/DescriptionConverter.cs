// 
// 	DescriptionConverter.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 24/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Classes.Converters {
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(string))] internal class DescriptionConverter: BaseConverter, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if(string.IsNullOrWhiteSpace(value as string))
                return "";
            var str = (string)value;
            var hasNewLine = Regex.IsMatch(str, "\r|\n");
            if(str.Length <= 50 && !hasNewLine)
                return str;
            if(hasNewLine)
                str = Regex.Replace(str, "\r|\n|\r\n", " ");
            str = Regex.Replace(str, "\\s+", " ");
            if(str.Length > 50)
                return str.Substring(0, 50).Trim() + "...";
            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}