using System.Linq;
using DAX.Cson.Tests.Extensions;
using DAX.Cson.Tests.Stubs;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class TestRoundtripOddity : FixtureBase
    {
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
{roundtrippedJson}");
            }
        }
    }
}