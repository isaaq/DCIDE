using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DC.IDE.UI.Converter
{
    public class CVBase64 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return Encoding.UTF8.GetString(System.Convert.FromBase64String(value.ToString()));
            }
            catch (Exception)
            {
                return "该文档内容不是Base64, 出错";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(value.ToString()));
        }
    }
}
