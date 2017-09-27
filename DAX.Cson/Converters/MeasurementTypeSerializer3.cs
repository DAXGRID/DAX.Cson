using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using FastMember;
using Newtonsoft.Json;

namespace DAX.Cson.Converters
{
    class MeasurementTypeSerializer3 : JsonConverter
    {
        const string ValueFieldName = nameof(Resistance.Value);
        const string MultiplierFieldName = nameof(Resistance.multiplier);
        const string UnitFieldName = nameof(Resistance.unit);

        readonly Dictionary<Type, Mapper> _mappersByType;

        public MeasurementTypeSerializer3()
        {
            var ns = typeof(Asset).Namespace;

            var mappers = typeof(Asset).Assembly.GetTypes()
                .Where(t => t.Namespace == ns)
                .Where(IsValueType)
                .Select(t => new
                {
                    Type = t,
                    Mapper = CreateMapper(t)
                })
                .ToList();

            _mappersByType = mappers.ToDictionary(a => a.Type, a => a.Mapper);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (ReferenceEquals(null, value))
            {
                writer.WriteNull();
                return;
            }

            var mapper = GetMapper(value.GetType());

            var serializedValue = mapper.Serialize(value);

            writer.WriteValue(serializedValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();

            if (string.IsNullOrWhiteSpace(value)) return null;

            var mapper = GetMapper(objectType);

            var deserializedObject = mapper.Deserialize(value);

            return deserializedObject;
        }

        public override bool CanConvert(Type objectType)
        {
            return _mappersByType.ContainsKey(objectType);
        }

        Mapper GetMapper(Type objectType)
        {
            return _mappersByType[objectType];
        }

        static Mapper CreateMapper(Type type)
        {
            return new Mapper(Serialize(type), Deserialize(type));
        }

        static Func<object, string> Serialize(Type type)
        {
            var typeAccessor = TypeAccessor.Create(type);
            var propertyType = type.GetProperty(ValueFieldName).PropertyType;

            return obj => Serialize(typeAccessor, obj, propertyType);
        }

        static object GetValueAsString(TypeAccessor typeAccessor, object obj, Type propertyType)
        {
            var value = typeAccessor[obj, ValueFieldName];

            if (ReferenceEquals(null, value)) return null;

            if (propertyType == typeof(string)) return value;

            if (propertyType == typeof(decimal)) return ((decimal)value).ToString(CultureInfo.InvariantCulture);

            if (propertyType == typeof(double)) return ((double)value).ToString(CultureInfo.InvariantCulture);

            if (propertyType == typeof(float)) return ((float)value).ToString(CultureInfo.InvariantCulture);

            throw new FormatException($"Cannot get value {value} of type {value?.GetType()} as a string");
        }

        static Func<string, object> Deserialize(Type type)
        {
            var propertyInfo = type.GetProperty(ValueFieldName);
            if (propertyInfo == null) throw new ApplicationException($"Could not find '{ValueFieldName}' property on measurement class {type}");

            var propertyType = propertyInfo.PropertyType;
            var typeAccessor = TypeAccessor.Create(type);

            return value => Deserialize(value, typeAccessor, propertyType);
        }

        static string Serialize(TypeAccessor typeAccessor, object obj, Type propertyType)
        {
            var value = GetValueAsString(typeAccessor, obj, propertyType);
            var multiplier = (UnitMultiplier)typeAccessor[obj, MultiplierFieldName];
            var unit = (UnitSymbol)typeAccessor[obj, UnitFieldName];

            var multiplierAndSymbol = new MultiplierAndSymbol(
                multiplier,
                unit
            );

            return string.Concat(value, " ", multiplierAndSymbol.Key).Trim();
        }

        static object Deserialize(string value, TypeAccessor typeAccessor, Type propertyType)
        {
            var parts = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 1)
            {
                var instance = typeAccessor.CreateNew();
                typeAccessor[instance, ValueFieldName] = ChangeType(parts[0], propertyType);
                return instance;
            }

            if (parts.Length == 2)
            {
                var instance = typeAccessor.CreateNew();

                typeAccessor[instance, ValueFieldName] = ChangeType(parts[0], propertyType);

                var multiplierAndUnit = MultiplierAndSymbol.GetMultiplierAndUnit(parts[1]);

                if (multiplierAndUnit.UnitMultiplier.HasValue)
                {
                    typeAccessor[instance, MultiplierFieldName] = multiplierAndUnit.UnitMultiplier.Value;
                }

                if (multiplierAndUnit.UnitSymbol.HasValue)
                {
                    typeAccessor[instance, UnitFieldName] = multiplierAndUnit.UnitSymbol.Value;
                }

                return instance;
            }

            throw new FormatException($"Could not turn '{value}' into proper measurement value");
        }


        static object ChangeType(string part, Type desiredType)
        {
            try
            {
                if (desiredType == typeof(decimal))
                {
                    var value = decimal.Parse(part, CultureInfo.InvariantCulture);

                    return Convert.ChangeType(value, desiredType);
                }

                if (desiredType == typeof(double))
                {
                    var value = double.Parse(part, CultureInfo.InvariantCulture);

                    return Convert.ChangeType(value, desiredType);
                }

                if (desiredType == typeof(float))
                {
                    var value = float.Parse(part, CultureInfo.InvariantCulture);

                    return Convert.ChangeType(value, desiredType);
                }

                if (desiredType == typeof(string))
                {
                    return part;
                }

                throw new FormatException($"Cannot deserialize value {part}");
            }
            catch (Exception exception)
            {
                throw new FormatException($"Could not turn '{part}' into {desiredType}", exception);
            }
        }

        static bool IsValueType(Type type)
        {
            return type.GetProperties().Length == 3
                   && type.GetProperty(ValueFieldName) != null
                   && type.GetProperty(MultiplierFieldName) != null
                   && type.GetProperty(UnitFieldName) != null;
        }

        class Mapper
        {
            readonly Func<object, string> _getRef;
            readonly Func<string, object> _getObject;

            public Mapper(Func<object, string> getRef, Func<string, object> getObject)
            {
                _getRef = getRef;
                _getObject = getObject;
            }

            public string Serialize(object value)
            {
                return _getRef(value);
            }

            public object Deserialize(string value)
            {
                return _getObject(value);
            }
        }
    }
}