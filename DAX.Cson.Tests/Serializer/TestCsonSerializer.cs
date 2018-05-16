using System;
using System.Collections.Concurrent;
using System.Diagnostics;
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

        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [Ignore("Found a 'bug', which is not a bug - it's just a matter of coming up with an INVALID and nonsensical combination of Unit/Multipler symbols")]
        public void RoundtripManyObjects(int howMany)
        {
            var counts = new ConcurrentDictionary<Type, int>();
            var reader = new CimObjectFactory();
            var serializer = new CsonSerializer();

            var objects = reader.Read().Take(howMany).ToList();
            var stopwatch = Stopwatch.StartNew();

            foreach (var obj in objects)
            {
                Roundtrip(serializer, obj);

                counts.AddOrUpdate(obj.GetType(), 1, (_, value) => value + 1);
            }

            var elapsed = stopwatch.Elapsed;

            Console.WriteLine($@"Successfully roundtripped objects

{string.Join(Environment.NewLine, counts.Select(kvp => $"    {kvp.Key}: {kvp.Value}"))}

in {elapsed.TotalSeconds:0.0} s

Total: {counts.Sum(kvp => kvp.Value)} => {howMany / elapsed.TotalSeconds:0.0} obj/s
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