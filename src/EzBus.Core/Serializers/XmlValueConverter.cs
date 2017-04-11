using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;

namespace EzBus.Core.Serializers
{
    public class XmlValueConverter
    {
        private static readonly IDictionary<Type, Func<string, object>> typeConvertActions;

        static XmlValueConverter()
        {
            typeConvertActions = new Dictionary<Type, Func<string, object>>
            {
                {typeof (short), s => XmlConvert.ToInt16(s)},
                {typeof (int), s => XmlConvert.ToInt32(s)},
                {typeof (long), s => XmlConvert.ToInt64(s)},
                {typeof (ushort), s => XmlConvert.ToUInt16(s)},
                {typeof (uint), s => XmlConvert.ToUInt32(s)},
                {typeof (ulong), s => XmlConvert.ToUInt64(s)},
                {typeof (bool), s => XmlConvert.ToBoolean(s.ToLower())},
                {typeof (double), s => XmlConvert.ToDouble(s)},
                {typeof (decimal), s => XmlConvert.ToDecimal(s)},
                {typeof (DateTime), s => XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Unspecified)},
                {typeof (TimeSpan), s => XmlConvert.ToTimeSpan(s)},
                {typeof (byte), s => XmlConvert.ToByte(s)},
                {typeof (string), s => s},
                {typeof (Guid), s => XmlConvert.ToGuid(s)},
                {
                    typeof (short?), s =>
                    {
                        short i;
                        if (short.TryParse(s, out i)) return i;
                        return null;
                    }
                },
                {
                    typeof (int?), s =>
                    {
                        int i;
                        if (int.TryParse(s, out i)) return i;
                        return null;
                    }
                },
                {
                    typeof (long?), s =>
                    {
                        long i;
                        if (long.TryParse(s, out i)) return i;
                        return null;
                    }
                },
            };
        }

        public static object Convert(object obj, Type convertTo)
        {
            if (convertTo.GetTypeInfo().IsEnum)
            {
                return Enum.Parse(convertTo, obj.ToString());
            }

            if (!typeConvertActions.ContainsKey(convertTo))
                throw new SerializationException(string.Format("Cannot deserialize type: {0}", convertTo.Name));

            var value = obj == null ? string.Empty : obj.ToString();
            return typeConvertActions[convertTo](value);
        }
    }
}