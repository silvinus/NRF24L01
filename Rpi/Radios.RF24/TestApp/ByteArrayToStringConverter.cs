namespace TestApp
{
    using System;
    using System.Text;
    using Windows.UI.Xaml.Data;

    public class ByteArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is byte[])
            {
                return Encoding.UTF8.GetString((byte[])value);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return Encoding.UTF8.GetBytes((string)value);
            }

            return value;
        }
    }
}
