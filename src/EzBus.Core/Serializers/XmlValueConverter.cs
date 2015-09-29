using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace EzBus.Core.Serilizers
{
    public class XmlValueConverter
    {
        private static readonly IDictionary<Type, Func<string, object>> typeConvertActions;

        static XmlValueConverter()
        {
            typeConvertActions = new Dictionary<Type, Func<string, object>>
            {
                {typeof (Int16), s => XmlConvert.ToInt16(s)},
                {typeof (Int32), s => XmlConvert.ToInt32(s)},
                {typeof (Int64), s => XmlConvert.ToInt64(s)},
                {typeof (UInt16), s => XmlConvert.ToUInt16(s)},
                {typeof (UInt32), s => XmlConvert.ToUInt32(s)},
                {typeof (UInt64), s => XmlConvert.ToUInt64(s)},
                {typeof (Boolean), s => XmlConvert.ToBoolean(s.ToLower())},
                {typeof (Double), s => XmlConvert.ToDouble(s)},
                {typeof (Decimal), s => XmlConvert.ToDecimal(s)},
                {typeof (DateTime), s => XmlConvert.ToDateTime(s, XmlDateTimeSerializationMode.Unspecified)},
                {typeof (TimeSpan), s => XmlConvert.ToTimeSpan(s)},
                {typeof (Byte), s => XmlConvert.ToByte(s)},
                {typeof (String), s => s},
                {typeof (Guid), s => XmlConvert.ToGuid(s)},
                {
                    typeof (short?), s =>
                    {
                        short i;
                        if (Int16.TryParse(s, out i)) return i;
                        return null;
                    }
                },
                {
                    typeof (int?), s =>
                    {
                        int i;
                        if (Int32.TryParse(s, out i)) return i;
                        return null;
                    }
                },
                {
                    typeof (long?), s =>
                    {
                        long i;
                        if (Int64.TryParse(s, out i)) return i;
                        return null;
                    }
                },
            };
        }

        public static object Convert(object obj, Type convertTo)
        {
            if (convertTo.IsEnum)
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