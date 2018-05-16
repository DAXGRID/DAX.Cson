using System;
using System.Collections.Generic;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using DAX.CIM.PhysicalNetworkModel.Changes;
using FastMember;
using Newtonsoft.Json;

namespace DAX.Cson.Converters
{
    class ObjectReferenceSerializer : JsonConverter
    {
        const string RefFieldName = nameof(TapScheduleTapChanger.@ref);
        const string ReferenceTypeFieldName = nameof(TapScheduleTapChanger.referenceType);

        readonly Dictionary<Type, Mapper> _mappersByType;

        public ObjectReferenceSerializer()
        {
            var ns = typeof(Asset).Namespace;

            var mappers = typeof(Asset).Assembly.GetTypes()
                .Where(t => t.Namespace == ns)
                .Where(IsReferenceType)
                .Concat(new[] { typeof(TargetObject) })
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

        static Mapper CreateMapper(Type type)
        {
            return new Mapper(Serialize(type), Deserialize(type));
        }

        static Func<object, string> Serialize(Type type)
        {
            var typeAccessor = TypeAccessor.Create(type);

            return obj =>
            {
                var referenceType = (string)typeAccessor[obj, ReferenceTypeFieldName];
                var @ref = (string)typeAccessor[obj, RefFieldName];
                return referenceType != null
                    ? $"{referenceType}/{@ref}"
                    : @ref;
            };
        }

        static Func<string, string, object> Deserialize(Type type)
        {
            var typeAccessor = TypeAccessor.Create(type);

            return (referenceType, @ref) =>
            {
                var instance = typeAccessor.CreateNew();
                typeAccessor[instance, RefFieldName] = @ref;
                typeAccessor[instance, ReferenceTypeFieldName] = string.IsNullOrWhiteSpace(referenceType) ? null : referenceType;
                return instance;
            };
        }

        static bool IsReferenceType(Type type)
        {
            return type.GetProperties().Length == 2
                   && type.GetProperty(RefFieldName) != null
                   && type.GetProperty(ReferenceTypeFieldName) != null;
        }

        Mapper GetMapper(Type objectType)
        {
            return _mappersByType[objectType];
        }

        class Mapper
        {
            readonly Func<object, string> _getRef;
            readonly Func<string, string, object> _getObject;

            public Mapper(Func<object, string> getRef, Func<string, string, object> getObject)
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
                var parts = value.Split('/');

                if (parts.Length == 1)
                {
                    return _getObject(null, parts[0]);
                }

                if (parts.Length == 2)
                {
                    return _getObject(parts[0], parts[1]);
                }

                throw new FormatException($"Could not interpret string '{value}' as a proper object reference - expected a format like 'referenceType/ref'");
            }
        }
    }
}