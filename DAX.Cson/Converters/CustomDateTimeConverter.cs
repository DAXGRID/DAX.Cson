using System;
using System.Globalization;
using Newtonsoft.Json;

namespace DAX.Cson.Converters
{
    class CustomDateTimeConverter : JsonConverter
    {
        static readonly DateTime ZeroDateTime = new DateTime(1900, 1, 1);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dateTime = (DateTime)value;

            if (dateTime.TimeOfDay == TimeSpan.Zero)
            {
                writer.WriteValue($"{dateTime.Year:0000}{dateTime.Month:00}{dateTime.Day:00}");
                return;
            }

            writer.WriteValue(dateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.PropertyName)
            {
                reader.Read();
            }

            var stringValue = reader.ReadAsString();

            if (stringValue.Length == 0) return ZeroDateTime;

            try
            {
                if (stringValue.Length == 8)
                {
                    var year = int.Parse(stringValue.Substring(0, 4));
                    var month = int.Parse(stringValue.Substring(4, 2));
                    var day = int.Parse(stringValue.Substring(6, 2));
                    return new DateTime(year, month, day);
                }

                var dateTimeOrNull = DateTime.Parse(stringValue, CultureInfo.InvariantCulture);

                return dateTimeOrNull;
            }
            catch (Exception exception)
            {
                throw new FormatException($"Could not parse date/time value '{stringValue}'", exception);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }
    }
}