using System;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Tests.Extensions;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class CheckRoundtripEntityWithThatSillySpecifiedProperty
    {
        [Test]
        public void CanDoIt()
        {
            var coil = new PetersenCoil
            {
                mRID = "known-id",
                aggregate = true,
                aggregateSpecified = true
            };

            var serializer = new CsonSerializer();
            
            var cson = serializer.SerializeObject(coil);
            var roundtrippedCoil = (PetersenCoil)serializer.DeserializeObject(cson);

            var coilJson = coil.ToJson();
            var roundtrippedCoilJson = roundtrippedCoil.ToJson();

            Console.WriteLine($@"OK - this Petersen coil

{coilJson.PrettifyJson()}

got turned into this Petersen coil

{roundtrippedCoilJson.PrettifyJson()}

when roundtripped");

            Assert.That(roundtrippedCoilJson, Is.EqualTo(coilJson));
        }
    }
}