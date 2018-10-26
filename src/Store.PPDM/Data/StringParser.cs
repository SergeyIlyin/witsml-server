using PDS.WITSMLstudio.Framework;
using System;
using System.Xml.Linq;

namespace YARUS.WITSML.Application
{
    public static class StringParser
    {
        public static T ParseString<T>(string stringValue)
        {
            var result = ParseString(typeof(T), stringValue);
        Energistics.DataAccess.EnergisticsConverter.XmlToObject<T>(stringValue);
            return (T)result;
        }
        public static object ParseString(Type targetType, string stringValue)
        {
            if (targetType == typeof(string)) return stringValue;

            object value = targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

            if (string.IsNullOrEmpty(stringValue)) return value;

            if (Nullable.GetUnderlyingType(targetType) != null)
            {
                var argType = Nullable.GetUnderlyingType(targetType);
                var parsed = ParseString(argType, stringValue);
                return parsed;
            }

            if (targetType.IsEnum)
            {
                return targetType.ParseEnum(stringValue);
            }
            if (targetType == typeof(int))
            {
                return Convert.ToInt32(stringValue);
            }
            if (targetType == typeof(double))
            {
                return Convert.ToDouble(stringValue);
            }
            if (targetType == typeof(float))
            {
                return Convert.ToDouble(stringValue);
            }
            if (targetType == typeof(bool))
            {
                return Convert.ToBoolean(stringValue);
            }
            if (stringValue.StartsWith("<") && stringValue.EndsWith(">"))
            {
                return XmlToObject(targetType, stringValue);
            };

            throw new System.NotImplementedException($"Парсинг {targetType} Не реализован");
        }

        static object XmlToObject(Type targetType, string  xml)
        {
            var converterType = typeof(Energistics.DataAccess.EnergisticsConverter);
            var genericMethod = converterType.GetMethod(nameof(Energistics.DataAccess.EnergisticsConverter.XmlToObject));
            var method= genericMethod.MakeGenericMethod(new Type[] { targetType });

            return method.Invoke(null, new object[] { xml });
        }
    }
}
