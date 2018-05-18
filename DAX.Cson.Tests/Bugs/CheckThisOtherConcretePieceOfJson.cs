using System;
using DAX.CIM.PhysicalNetworkModel;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class CheckThisOtherConcretePieceOfJson : FixtureBase
    {
        CsonSerializer _serializer;

        protected override void SetUp()
        {
            _serializer = new CsonSerializer();
        }

        [Test]
        public void ItWorks()
        {
            var whenTrue = new GroundDisconnector { normalOpen = true };
            var whenFalse = new GroundDisconnector { normalOpen = false };

            Console.WriteLine($@"When true:

{_serializer.SerializeObject(whenTrue)}

When false:

{_serializer.SerializeObject(whenFalse)}");
        }

        [Test]
        public void VerifyJsonDotNetAssumption()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var whenTrue = JsonConvert.SerializeObject(new { BooleanProperty = true }, settings);
            var whenFalse = JsonConvert.SerializeObject(new { BooleanProperty = false }, settings);

            Console.WriteLine($@"When true:

{whenTrue}

When false:

{whenFalse}");
        }
    }
}