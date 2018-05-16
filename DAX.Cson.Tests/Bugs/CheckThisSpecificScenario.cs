using System;
using System.Collections.Generic;
using DAX.CIM.PhysicalNetworkModel;
using DAX.CIM.PhysicalNetworkModel.FeederInfo;
using DAX.Cson.Tests.Extensions;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class CheckThisSpecificScenario : FixtureBase
    {
        CsonSerializer _serializer;

        protected override void SetUp()
        {
            _serializer = new CsonSerializer();
        }

        [Test]
        public void DoesNotIncludeAsset()
        {
            var acLineSegment = new ACLineSegment
            {
                InternalFeeders = new List<Feeder> { new Feeder { } }
            };

            var cson = _serializer.SerializeObject(acLineSegment);

            Console.WriteLine(cson.PrettifyJson());

            var jObject = JObject.Parse(cson);

            Assert.That(jObject.Property("Asset"), Is.Null);
        }
    }
}