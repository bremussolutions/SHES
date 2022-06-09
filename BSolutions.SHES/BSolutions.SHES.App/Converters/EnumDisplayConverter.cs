using Microsoft.UI.Xaml.Data;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace BSolutions.SHES.App.Converters
{
    public class EnumDisplayConverter : IValueConverter
    {
        private string GetEnumDisplayInfo(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
                return enumObj.ToString();
            else
            {
                DisplayAttribute attrib = null;

                foreach (var att in attribArray)
                {
                    if (att is DisplayAttribute)
                    {
                        attrib = att as DisplayAttribute;
                    }
                }

                if (attrib != null)
                {
                    return attrib.Name;
                }

                return enumObj.ToString();
            }
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Enum myEnum = (Enum)value;
            return this.GetEnumDisplayInfo(myEnum);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return string.Empty;
        }
    }
}
