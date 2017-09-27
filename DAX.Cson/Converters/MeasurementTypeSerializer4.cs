namespace DAX.Cson.Converters
{
    //class MeasurementTypeSerializer4 : JsonConverter
    //{
    //    const string ValueFieldName = nameof(Resistance.Value);
    //    const string MultiplierFieldName = nameof(Resistance.multiplier);
    //    const string UnitFieldName = nameof(Resistance.unit);
    //    const string UnitSpecifiedFieldName = nameof(Resistance.unitSpecified);

    //    readonly Dictionary<Type, Mapper> _mappersByType;

    //    public MeasurementTypeSerializer4()
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
    //            var value = Convert.ToDecimal(typeAccessor[obj, ValueFieldName]);
    //            var multiplier = (UnitMultiplier)typeAccessor[obj, MultiplierFieldName];
    //            var unit = (UnitSymbol)typeAccessor[obj, UnitFieldName];
    //            var unitSpecified = (bool)typeAccessor[obj, UnitSpecifiedFieldName];

    //            var multiplierAndSymbol = new MultiplierAndSymbol(
    //                multiplier,
    //                unitSpecified ? unit : default(UnitSymbol?)
    //            );

    //            return string.Concat(value.ToString(CultureInfo.InvariantCulture), " ", multiplierAndSymbol.Key).Trim();
    //        };
    //    }

    //    static Func<string, object> Deserialize(Type type)
    //    {
    //        var propertyType = type.GetProperty(ValueFieldName).PropertyType;
    //        var typeAccessor = TypeAccessor.Create(type);

    //        return value =>
    //        {
    //            var parts = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

    //            if (parts.Length == 1)
    //            {
    //                var instance = typeAccessor.CreateNew();
    //                typeAccessor[instance, ValueFieldName] = ChangeType(parts[0], propertyType);
    //                return instance;
    //            }

    //            if (parts.Length == 2)
    //            {
    //                var instance = typeAccessor.CreateNew();

    //                typeAccessor[instance, ValueFieldName] = ChangeType(parts[0], propertyType);

    //                var multiplierAndUnit = MultiplierAndSymbol.GetMultiplierAndUnit(parts[1]);

    //                if (multiplierAndUnit.UnitMultiplier.HasValue)
    //                {
    //                    typeAccessor[instance, MultiplierFieldName] = multiplierAndUnit.UnitMultiplier.Value;
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


    //    static object ChangeType(string part, Type desiredType)
    //    {
    //        try
    //        {
    //            var value = decimal.Parse(part, CultureInfo.InvariantCulture);

    //            return Convert.ChangeType(value, desiredType);
    //        }
    //        catch (Exception exception)
    //        {
    //            throw new FormatException($"Could not turn '{part}' into {desiredType}", exception);
    //        }
    //    }

    //    static bool IsValueType(Type type)
    //    {
    //        return type.GetProperties().Length == 4
    //               && type.GetProperty(ValueFieldName) != null
    //               && type.GetProperty(MultiplierFieldName) != null
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
    //            return _getRef(value);
    //        }

    //        public object Deserialize(string value)
    //        {
    //            return _getObject(value);
    //        }
    //    }
    //}
}