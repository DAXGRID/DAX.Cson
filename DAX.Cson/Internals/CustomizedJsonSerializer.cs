using System;
using System.Runtime.Serialization;
using DAX.Cson.Converters;
using Newtonsoft.Json;

namespace DAX.Cson.Internals
{
    class CustomizedJsonSerializer
    {
        static readonly JsonSerializerSettings SerializerSettings = GetSettings();

        static JsonSerializerSettings GetSettings()
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                NullValueHandling = NullValueHandling.Ignore,
                Binder = new ShortNameBinder(),
                Converters =
                {
                    //new Point2DConverter(),
                    //new CustomDateTimeConverter()
                    //new MeasurementTypeSerializer5(),
                    //new MeasurementTypeSerializer4(),

                    new ObjectReferenceSerializer(),
                    new MeasurementTypeSerializer3(),
                },
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                ContractResolver = new CustomizedContractResolver(),
                
                // it's important that 0, false, etc are serialized too
                //DefaultValueHandling = DefaultValueHandling.Ignore,
            };

            return settings;
        }

        public string Serialize(object obj)
        {
            try
            {
                var json = JsonConvert.SerializeObject(obj, SerializerSettings);

                return json;
            }
            catch (Exception exception)
            {
                throw new SerializationException($"Could not serialize object {obj} to JSON", exception);
            }
        }

        public object Deserialize(string json)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(json, SerializerSettings);

                return obj;
            }
            catch (Exception exception)
            {
                throw new SerializationException($"Could not deserialize the following JSON text: '{json}'", exception);
            }
        }
    }
}