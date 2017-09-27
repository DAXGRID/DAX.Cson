using System;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Internals;
using DAX.Cson.Tests.Extensions;
using NUnit.Framework;

namespace DAX.Cson.Tests.Serializer
{
    [TestFixture]
    public class TestCustomizedJsonSerializer : FixtureBase
    {
        CustomizedJsonSerializer _serializer;

        protected override void SetUp()
        {
            _serializer = new CustomizedJsonSerializer();
        }

        [Test]
        public void CanRoundtripAssetOrgnizationRoles()
        {
            var original = new ObjectWithRoles
            {
                Role = new TapScheduleTapChanger { referenceType = "type", @ref = "ref1" }
            };

            var json = _serializer.Serialize(original);

            Console.WriteLine(json.PrettifyJson());

            var roundtrippedRolesObject = (ObjectWithRoles)_serializer.Deserialize(json);

            Assert.That(roundtrippedRolesObject.Role.referenceType, Is.EqualTo("type"));
            Assert.That(roundtrippedRolesObject.Role.@ref, Is.EqualTo("ref1"));
        }

        class ObjectWithRoles
        {
            public TapScheduleTapChanger Role { get; set; }
        }

        [Test]
        public void WorksWithMeasurementTypesToo()
        {
            var original = new ObjectWithConductance
            {
                Conductance = new Conductance
                {
                    Value = 3434,
                    multiplier = UnitMultiplier.k,
                    unit = UnitSymbol.Wh,
                }
            };

            var json = _serializer.Serialize(original);

            Console.WriteLine(json.PrettifyJson());

            var roundtrippedConductanceObject = (ObjectWithConductance)_serializer.Deserialize(json);

            Assert.That(roundtrippedConductanceObject.Conductance.Value, Is.EqualTo(3434));
            Assert.That(roundtrippedConductanceObject.Conductance.multiplier, Is.EqualTo(UnitMultiplier.k));
            Assert.That(roundtrippedConductanceObject.Conductance.unit, Is.EqualTo(UnitSymbol.Wh));
        }

        class ObjectWithConductance
        {
            public Conductance Conductance { get; set; }
        }
    }
}