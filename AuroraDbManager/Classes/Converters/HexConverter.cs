// 
// 	HexConverter.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 18/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Classes.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    public abstract class BaseConverter: MarkupExtension {
        public override object ProvideValue(IServiceProvider serviceProvider) { return this; }
    }

    [ValueConversion(typeof(object), typeof(string))] internal class IntHexConverter: BaseConverter, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return ((int)value).ToString("X08"); }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { return value == null ? 0 : int.Parse((string)value, NumberStyles.HexNumber); }
    }
}