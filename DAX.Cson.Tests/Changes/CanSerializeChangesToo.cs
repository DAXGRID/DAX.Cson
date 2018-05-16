using System;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using DAX.CIM.PhysicalNetworkModel.Changes;
using DAX.Cson.Tests.Extensions;
using DAX.Cson.Tests.Stubs;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace DAX.Cson.Tests.Changes
{
    [TestFixture]
    public class CanSerializeChangesToo : FixtureBase
    {
        CsonSerializer _serializer;
        CimObjectFactory _factory;

        protected override void SetUp()
        {
            _factory = new CimObjectFactory();

            _serializer = new CsonSerializer();
        }

        [Test]
        public void ObjectCreation()
        {
            var targetObject = _factory.Read().OfType<ACLineSegment>().First();

            var changeId = Guid.NewGuid().ToString();

            var change = new DataSetMember
            {
                Change = new ObjectCreation { Object = targetObject },
                TargetObject = new TargetObject
                {
                    referenceType = nameof(ACLineSegment),
                    @ref = targetObject.mRID
                },
                mRID = changeId
            };

            var cson = _serializer.SerializeObject(change);

            Console.WriteLine(cson.PrettifyJson());

            var roundtrippedChange = _serializer.DeserializeObject(cson);

            Assert.That(change.ToJson(), Is.EqualTo(roundtrippedChange.ToJson()));
        }

        [Test]
        public void ObjectDeletion()
        {
            var targetObject = _factory.Read().OfType<ACLineSegment>().First();

            var changeId = Guid.NewGuid().ToString();

            var change = new DataSetMember
            {
                Change = new ObjectDeletion(),
                TargetObject = new TargetObject
                {
                    referenceType = nameof(ACLineSegment),
                    @ref = targetObject.mRID
                },
                mRID = changeId
            };

            var cson = _serializer.SerializeObject(change);

            Console.WriteLine(cson.PrettifyJson());

            var roundtrippedChange = _serializer.DeserializeObject(cson);

            Assert.That(change.ToJson(), Is.EqualTo(roundtrippedChange.ToJson()));
        }

        [Test]
        public void ObjectModification()
        {
            var objectId = Guid.NewGuid().ToString();

            var newState = new ACLineSegment
            {
                mRID = objectId,
                BaseVoltage = 23,
                bch = new Susceptance { Value = 25, multiplier = UnitMultiplier.M, unit = UnitSymbol.Hz }, 
            };

            var oldState = new ACLineSegment
            {
                mRID = objectId,
                BaseVoltage = 20,
                bch = new Susceptance { Value = 16, multiplier = UnitMultiplier.M, unit = UnitSymbol.Hz }
            };

            var changeId = Guid.NewGuid().ToString();

            var change = new DataSetMember
            {
                Change = new ObjectModification { Object = newState },
                TargetObject = new TargetObject
                {
                    referenceType = nameof(ACLineSegment),
                    @ref = newState.mRID
                },
                mRID = changeId,
                ReverseChange = new ObjectReverseModification { Object = oldState }
            };

            var cson = _serializer.SerializeObject(change);

            Console.WriteLine(cson.PrettifyJson());

            var objectModificationJsonObject = JObject.Parse(cson);
            var newStateJsonObject = objectModificationJsonObject["Change"]?["Object"] as JObject;
            var oldStateJsonObject = objectModificationJsonObject["ReverseChange"]?["Object"] as JObject;

            Assert.That(newStateJsonObject, Is.Not.Null);
            Assert.That(oldStateJsonObject, Is.Not.Null);

            Assert.That(newStateJsonObject.Property("PSRType"), Is.Null);
            Assert.That(oldStateJsonObject.Property("PSRType"), Is.Null);

            var roundtrippedChange = _serializer.DeserializeObject(cson);

            Assert.That(change.ToJson(), Is.EqualTo(roundtrippedChange.ToJson()));
        }
    }
}