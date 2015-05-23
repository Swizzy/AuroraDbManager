// 
// 	ScrollbarConverters.cs
// 	AuroraDbManager
// 
// 	Created by Swizzy on 23/05/2015
// 	Copyright (c) 2015 Swizzy. All rights reserved.

namespace AuroraDbManager.Classes.Converters {
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class ScrollbarOnFarLeftConverter: BaseConverter, IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) { return value != null && ((double)value > 0); }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }

    public class ScrollbarOnFarRightConverter: BaseConverter, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            if(values == null || values[0] == null || values[1] == null || values[2] == null)
                return false;
            if(values[0].Equals(double.NaN) || values[1].Equals(double.NaN) || values[2].Equals(double.NaN))
                return false;

            double horizontalOffset, viewportWidth, extentWidth;
            double.TryParse(values[0].ToString(), out horizontalOffset);
            double.TryParse(values[1].ToString(), out viewportWidth);
            double.TryParse(values[2].ToString(), out extentWidth);

            return Math.Round(horizontalOffset + viewportWidth, 2) < Math.Round(extentWidth, 2);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}