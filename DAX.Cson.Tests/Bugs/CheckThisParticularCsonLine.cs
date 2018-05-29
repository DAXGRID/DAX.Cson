using System;
using DAX.Cson.Tests.Extensions;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    [Description("Verifies that this piece of CSON, containing a single 'm' as the unit/multipler in the 'length' property, can be properly deserialized. Used to be a problem, because the single 'm' was erronously considered problematic. It's not problematic.")]
    public class CheckThisParticularCsonLine : FixtureBase
    {
        const string CsonString =
            @"{""$type"":""ACLineSegmentExt"",""length"":""0.0999871343374252 m"",""BaseVoltage"":400.0,""EquipmentContainer"":""0e8ba9af-a363-4bbb-9247-c818e427df0f"",""PSRType"":""InternalCable"",""Assets"":""08d3cc64-3317-4400-8054-619557610ab7"",""mRID"":""08d3cc64-3317-4400-9440-758143751ea3""}";

        [Test]
        public void CanDeserializeIt()
        {
            var obj = new CsonSerializer().DeserializeObject(CsonString);

            Console.WriteLine(obj.ToJson());
        }
    }
}