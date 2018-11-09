namespace TestApp
{
    using Radios.RF24;
    using System;
    using Windows.UI.Xaml.Data;

    public class DataRateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DataRate)
            {
                return ((DataRate)value).ToString();
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return Enum.Parse(typeof(DataRate), (string)value);
            }
            return value;
        }
    }
}
