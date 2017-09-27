namespace DAX.Cson.Converters
{
    //class MeasurementTypeSerializer5 : JsonConverter
    //{
    //    const string ValueFieldName = nameof(Conductance.Value);
    //    const string MultiplierFieldName = nameof(Conductance.multiplier);
    //    const string MultiplierSpecifiedFieldName = nameof(Conductance.multiplierSpecified);
    //    const string UnitFieldName = nameof(Conductance.unit);
    //    const string UnitSpecifiedFieldName = nameof(Conductance.unitSpecified);

    //    readonly Dictionary<Type, Mapper> _mappersByType;

    //    public MeasurementTypeSerializer5()
    //    {
    //        var ns = typeof(Asset).Namespace;

    //        var mappers = typeof(Asset).Assembly.GetTypes()
    //            .Where(t => t.Namespace == ns)
    //            .Where(IsValueType)
    //            .Select(t => new
    //            {
    //                Type = t,
    //                Mapper = CreateMapper(t)
    //            })
    //            .ToList();

    //        _mappersByType = mappers.ToDictionary(a => a.Type, a => a.Mapper);
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        if (ReferenceEquals(null, value))
    //        {
    //            writer.WriteNull();
    //            return;
    //        }

    //        var mapper = GetMapper(value.GetType());

    //        var serializedValue = mapper.Serialize(value);

    //        writer.WriteValue(serializedValue);
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        var value = reader.Value?.ToString();

    //        if (string.IsNullOrWhiteSpace(value)) return null;

    //        var mapper = GetMapper(objectType);

    //        var deserializedObject = mapper.Deserialize(value);

    //        return deserializedObject;
    //    }

    //    public override bool CanConvert(Type objectType)
    //    {
    //        return _mappersByType.ContainsKey(objectType);
    //    }

    //    Mapper GetMapper(Type objectType)
    //    {
    //        return _mappersByType[objectType];
    //    }

    //    static Mapper CreateMapper(Type type)
    //    {
    //        return new Mapper(Serialize(type), Deserialize(type));
    //    }

    //    static Func<object, string> Serialize(Type type)
    //    {
    //        var typeAccessor = TypeAccessor.Create(type);

    //        return obj =>
    //        {
    //            var value = (string)typeAccessor[obj, ValueFieldName];
    //            var multiplier = (UnitMultiplier)typeAccessor[obj, MultiplierFieldName];
    //            var multiplerSpecified = (bool)typeAccessor[obj, MultiplierSpecifiedFieldName];
    //            var unit = (UnitSymbol)typeAccessor[obj, UnitFieldName];
    //            var unitSpecified = (bool)typeAccessor[obj, UnitSpecifiedFieldName];

    //            var multiplierAndSymbol = new MultiplierAndSymbol(
    //                multiplerSpecified ? multiplier : default(UnitMultiplier?),
    //                unitSpecified ? unit : default(UnitSymbol?)
    //            );

    //            return string.Concat(value, " ", multiplierAndSymbol.Key).Trim();
    //        };
    //    }

    //    static Func<string, object> Deserialize(Type type)
    //    {
    //        var typeAccessor = TypeAccessor.Create(type);

    //        return value =>
    //        {
    //            var parts = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

    //            if (parts.Length == 1)
    //            {
    //                var instance = typeAccessor.CreateNew();
    //                typeAccessor[instance, ValueFieldName] = parts[0];
    //                return instance;
    //            }

    //            if (parts.Length == 2)
    //            {
    //                var instance = typeAccessor.CreateNew();
    //                typeAccessor[instance, ValueFieldName] = parts[0];

    //                var multiplierAndUnit = MultiplierAndSymbol.GetMultiplierAndUnit(parts[1]);

    //                if (multiplierAndUnit.UnitMultiplier.HasValue)
    //                {
    //                    typeAccessor[instance, MultiplierFieldName] = multiplierAndUnit.UnitMultiplier.Value;
    //                    typeAccessor[instance, MultiplierSpecifiedFieldName] = true;
    //                }

    //                if (multiplierAndUnit.UnitSymbol.HasValue)
    //                {
    //                    typeAccessor[instance, UnitFieldName] = multiplierAndUnit.UnitSymbol.Value;
    //                    typeAccessor[instance, UnitSpecifiedFieldName] = true;
    //                }

    //                return instance;
    //            }

    //            throw new FormatException($"Could not turn '{value}' into proper measurement value");
    //        };
    //    }

    //    static bool IsValueType(Type type)
    //    {
    //        return type.GetProperties().Length == 5
    //               && type.GetProperty(ValueFieldName) != null
    //               && type.GetProperty(MultiplierFieldName) != null
    //               && type.GetProperty(MultiplierSpecifiedFieldName) != null
    //               && type.GetProperty(UnitFieldName) != null
    //               && type.GetProperty(UnitSpecifiedFieldName) != null;
    //    }

    //    class Mapper
    //    {
    //        readonly Func<object, string> _getRef;
    //        readonly Func<string, object> _getObject;

    //        public Mapper(Func<object, string> getRef, Func<string, object> getObject)
    //        {
    //            _getRef = getRef;
    //            _getObject = getObject;
    //        }

    //        public string Serialize(object value)
    //        {
    //            try
    //            {
    //                return _getRef(value);
    //            }
    //            catch (Exception exception)
    //            {
    //                throw new SerializationException($"Could not serialize value '{value}'", exception);
    //            }
    //        }

    //        public object Deserialize(string value)
    //        {
    //            try
    //            {
    //                return _getObject(value);
    //            }
    //            catch (Exception exception)
    //            {
    //                throw new SerializationException($"Could not deserialize string '{value}'", exception);
    //            }
    //        }
    //    }
    //}
}