using System;
using System.Globalization;
using DAX.CIM.PhysicalNetworkModel;
using Newtonsoft.Json;

namespace DAX.Cson.Converters
{
    class Point2DConverter : JsonConverter
    {
        static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            try
            {
                var point = (Point2D)value;

                var stringValue = string.Concat(
                    point.X.ToString(InvariantCulture),
                    ",",
                    point.Y.ToString(InvariantCulture));

                writer.WriteValue(stringValue);
            }
            catch (InvalidCastException exception)
            {
                throw new InvalidCastException($"The value {value} is not of the expected Point2D type", exception);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();

            if (value == null) return new Point2D();

            var parts = value.Split(',');

            try
            {
                var x = double.Parse(parts[0], InvariantCulture);
                var y = double.Parse(parts[1], InvariantCulture);
                return new Point2D(x, y);
            }
            catch (Exception exception)
            {
                throw new FormatException($"The string {value} could not be interpreted as a proper coordinate set - it must be on the form 'X,Y', e.g. something like 45.23,343.12", exception);
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Point2D);
        }
    }
}