namespace TestApp
{
    using Radios.RF24;
    using System;
    using Windows.UI.Xaml.Data;

    public class PowerLevelToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is PowerLevel)
            {
                return ((PowerLevel)value).ToString();
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return Enum.Parse(typeof(PowerLevel), ((string)value));
            }

            return value;
        }
    }
}
