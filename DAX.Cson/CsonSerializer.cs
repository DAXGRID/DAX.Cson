using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Internals;

namespace DAX.Cson
{
    /// <summary>
    /// CIM JSON serializer
    /// </summary>
    public class CsonSerializer
    {
        readonly int _lineBufferSize;
        static readonly byte[] LineBreakBytes = Encoding.UTF8.GetBytes(Environment.NewLine);

        readonly CustomizedJsonSerializer _serializer = new CustomizedJsonSerializer();

        /// <summary>
        /// Creates the CSON serializer with the given line buffer size
        /// </summary>
        public CsonSerializer(int lineBufferSize = 1024)
        {
            _lineBufferSize = lineBufferSize;
        }

        /// <summary>
        /// Serializes a single object to a JSON string
        /// </summary>
        public string SerializeObject(IdentifiedObject obj)
        {
            return _serializer.Serialize(obj);
        }

        /// <summary>
        /// Deserializes a single object into its <see cref="IdentifiedObject"/> subclass
        /// </summary>
        public IdentifiedObject DeserializeObject(string json)
        {
            var obj = _serializer.Deserialize(json);

            try
            {
                return (IdentifiedObject)obj;
            }
            catch (Exception exception)
            {
                throw new SerializationException($"The type returned from deserialization {obj.GetType()} could be turned into IdentifiedObject", exception);
            }
        }

        /// <summary>
        /// Returns a JSONL stream from the given <paramref name="objects"/>
        /// </summary>
        public Stream SerializeObjects(IEnumerable<IdentifiedObject> objects)
        {
            var enumerator = objects.GetEnumerator();

            var callbackStream = new CallbackStream(request =>
            {
                var lines = new List<string>(_lineBufferSize);

                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    var line = SerializeObject(current);
                    lines.Add(string.Concat(line, Environment.NewLine));

                    if (lines.Count >= _lineBufferSize)
                    {
                        request.Write(Encoding.UTF8.GetBytes(string.Concat(lines)));
                        return;
                    }
                }

                if (lines.Count >= 0)
                {
                    request.Write(Encoding.UTF8.GetBytes(string.Concat(lines)));
                }

                enumerator.Dispose();

                //if (enumerator.MoveNext())
                //{
                //    var current = enumerator.Current;
                //    var line = SerializeObject(current);
                //    var bytes = Encoding.UTF8.GetBytes(line);

                //    request.Write(bytes);
                //    request.Write(LineBreakBytes);
                //}
                //else
                //{
                //    enumerator.Dispose();
                //}
            });

            return callbackStream;
        }

        /// <summary>
        /// Deserializes the given JSON stream and returns <see cref="IdentifiedObject"/> while traversing it
        /// </summary>
        public IEnumerable<IdentifiedObject> DeserializeObjects(Stream source)
        {
            var lineCounter = 0;

            using (var reader = new StreamReader(source, Encoding.UTF8))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    lineCounter++;

                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var obj = DeserializeObjectFromLine(line, lineCounter);

                    yield return obj;
                }
            }
        }

        IdentifiedObject DeserializeObjectFromLine(string line, int lineCounter)
        {
            try
            {
                return DeserializeObject(line);
            }
            catch (Exception exception)
            {
                throw new SerializationException($"Could not get object from line {lineCounter}", exception);
            }
        }
    }
}