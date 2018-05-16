using System;
using System.Collections.Concurrent;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Tests.Extensions;
using DAX.Cson.Tests.Stubs;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DAX.Cson.Tests.Serializer
{
    [TestFixture]
    public class TestCsonSerializer : FixtureBase
    {
        [Test]
        public void RoundtripJson()
        {
            var serializer = new CsonSerializer();
            var reader = new CimObjectFactory();

            Console.WriteLine(string.Join(Environment.NewLine, reader.Read().Select(serializer.SerializeObject).Take(20)));
        }

        [Test]
        public void ShowFullJson()
        {
            var serializer = new CsonSerializer();
            var reader = new CimObjectFactory();
            var jsonLines = reader.Read().Select(serializer.SerializeObject).Take(5);

            Console.WriteLine(string.Join(Environment.NewLine, jsonLines.Select(json => json.PrettifyJson())));
        }

        [Test]
        public void RoundtripManyObjects()
        {
            var counts = new ConcurrentDictionary<Type, int>();
            var reader = new CimObjectFactory();
            var serializer = new CsonSerializer();

            foreach (var obj in reader.Read())
            {
                Roundtrip(serializer, obj);

                counts.AddOrUpdate(obj.GetType(), 1, (_, value) => value + 1);
            }

            Console.WriteLine($@"Successfully roundtripped objects

{string.Join(Environment.NewLine, counts.Select(kvp => $"    {kvp.Key}: {kvp.Value}"))}

Total: {counts.Sum(kvp => kvp.Value)}
");
        }

        void Roundtrip(CsonSerializer serializer, IdentifiedObject identifiedObject)
        {
            var initialJson = JsonConvert.SerializeObject(identifiedObject, Formatting.Indented);
            var roundtrippedObject = serializer.DeserializeObject(serializer.SerializeObject(identifiedObject));
            var roundtrippedJson = JsonConvert.SerializeObject(roundtrippedObject, Formatting.Indented);

            if (initialJson != roundtrippedJson)
            {
                throw new AssertionException($@"Found object of type {identifiedObject.GetType().Name} that did not preserve everything when roundtripped:

{initialJson}

=>

{roundtrippedJson}

Here's the CSON for the two objects:

{serializer.SerializeObject(identifiedObject)}

=>

{serializer.SerializeObject(roundtrippedObject)}");
            }
        }
    }
}