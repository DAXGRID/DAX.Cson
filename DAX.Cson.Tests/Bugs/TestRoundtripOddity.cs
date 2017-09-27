using System;
using System.Collections.Generic;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Internals;
using DAX.Cson.Tests.Extensions;
using DAX.Cson.Tests.Stubs;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class TestRoundtripOddity : FixtureBase
    {
        CustomizedJsonSerializer _customizedJsonSerializer;

        protected override void SetUp()
        {
            _customizedJsonSerializer = new CustomizedJsonSerializer();
        }

        [Test]
        public void TestProblematicSymbols()
        {
            var multipliers = Enum.GetValues(typeof(UnitMultiplier)).Cast<UnitMultiplier>().Select(e => e.ToString());
            var symbols = Enum.GetValues(typeof(UnitSymbol)).Cast<UnitSymbol>().Select(e => e.ToString());

            var dupes = multipliers.Intersect(symbols);

            Console.WriteLine($@"The following strings are dupes:

{string.Join(Environment.NewLine, dupes)}");
        }

        [Test]
        public void CheckCombinations()
        {
            var multipliers = Enum.GetValues(typeof(UnitMultiplier)).Cast<UnitMultiplier>();
            var symbols = Enum.GetValues(typeof(UnitSymbol)).Cast<UnitSymbol>();

            string GetKey(UnitMultiplier m, UnitSymbol s) => string.Concat(
                m == UnitMultiplier.none ? "" : m.ToString(),
                s == UnitSymbol.none ? "" : s.ToString()
            );

            var combinations = (from m in multipliers
                                from s in symbols
                                select new { m, s, key = GetKey(m, s) })
                .ToList();

            Console.WriteLine("All combinations:");
            Console.WriteLine(string.Join(Environment.NewLine, combinations));

            Console.WriteLine();
            Console.WriteLine("Dupes:");
            var dupes = combinations.GroupBy(a => a.key).Where(g => g.Count() > 1);

            Console.WriteLine(string.Join(Environment.NewLine, dupes.Select(g => $"{g.Key}: {string.Join(", ", g)}")));
        }

        [TestCaseSource(nameof(GetProblematicJsons))]
        public void ZeroInOnProblematicCases(JsonCase jsonCase)
        {
            PoundIt(jsonCase);
        }

        public class JsonCase
        {
            public JsonCase(string json, Type type)
            {
                Json = json;
                Type = type;
            }

            public Type Type { get; }
            public string Json { get; }
        }

        static IEnumerable<JsonCase> GetProblematicJsons()
        {
            yield return new JsonCase(@"
{
     ""Value"": {
        ""$type"": ""DAX.CIM.PhysicalNetworkModel.Seconds, DAX.CIM.PhysicalNetworkModel"",
        ""multiplier"": 4,
        ""unit"": 14,
        ""Value"": 23394.0
    }
}
", typeof(Seconds));

            yield return new JsonCase(@"{
    ""Value"": {
        ""$type"": ""DAX.CIM.PhysicalNetworkModel.PU, DAX.CIM.PhysicalNetworkModel"",
        ""multiplier"": 8,
        ""unit"": 14,
        ""Value"": 201.0
    }
}", typeof(PU));
        }

        void PoundIt(JsonCase jsonCase)
        {
            var problematicJson = jsonCase.Json;
            var containingType = typeof(ClassWithValue<>).MakeGenericType(jsonCase.Type);

            Console.WriteLine($@"Checking the CSON roundtripping of an object that looks like this:

{problematicJson}
");

            var obj = JsonConvert.DeserializeObject(problematicJson, containingType);

            Console.WriteLine($@"(this is just an ordinary JSON representation - the CSON representation looks like this:

{_customizedJsonSerializer.Serialize(obj)}");

            var cson = _customizedJsonSerializer.Serialize(obj);
            var roundtrippedObj = _customizedJsonSerializer.Deserialize(cson);

            if (string.Equals(obj.ToJson().PrettifyJson(), roundtrippedObj.ToJson().PrettifyJson())) return;

            throw new AssertionException($@"This is the problematic object:

{problematicJson.PrettifyJson()}

=>

{cson}

=>

{roundtrippedObj.ToJson().PrettifyJson()}

");
        }

        class ClassWithValue<T>
        {
            public T Value { get; }

            public ClassWithValue(T value)
            {
                Value = value;
            }
        }

        [TestCase(20)]
        [TestCase(30)]
        [TestCase(50)]
        [TestCase(100)]
        public void ProvokeError(int tests)
        {
            var serializer = new CsonSerializer();

            foreach (var obj in new CimObjectFactory().Read().Take(tests))
            {
                var originalJson = obj.ToJson();
                var roundtrippedJson = serializer.DeserializeObject(serializer.SerializeObject(obj)).ToJson();

                if (string.Equals(originalJson, roundtrippedJson)) continue;

                throw new AssertionException($@"The roundtripped JSON

{roundtrippedJson.PrettifyJson()}

does not match the original JSON

{originalJson.PrettifyJson()}

Here are the REAL objects:

{obj.ToJson()}
{roundtrippedJson}

Here is the CSON:

{serializer.SerializeObject(obj)}");
            }
        }
    }
}