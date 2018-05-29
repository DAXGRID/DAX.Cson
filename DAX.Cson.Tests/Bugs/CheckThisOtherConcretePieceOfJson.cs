using System;
using DAX.CIM.PhysicalNetworkModel;
using Newtonsoft.Json.Linq;
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

            var whenTrueCson = _serializer.SerializeObject(whenTrue);
            var whenFalseCson = _serializer.SerializeObject(whenFalse);

            Console.WriteLine($@"When true:

{whenTrueCson}

When false:

{whenFalseCson}");

            Assert.That(JObject.Parse(whenTrueCson).Property("normalOpen"), Is.Not.Null, "Expected the 'normalOpen' property to be there");
            Assert.That(JObject.Parse(whenFalseCson).Property("normalOpen"), Is.Not.Null, "Expected the 'normalOpen' property to be there ALSO when the value is false");
        }
    }
}