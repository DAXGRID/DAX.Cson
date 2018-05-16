using DAX.Cson.Tests.Extensions;
using NUnit.Framework;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class CheckThisConcretePieceOfJson : FixtureBase
    {
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public void CanDoIt(int lineBufferSize)
        {
            var serializer = new CsonSerializer(lineBufferSize);
            var o = serializer.DeserializeObject(Json);
            var json1 = o.ToJson();
            var json2 = serializer.DeserializeObject(serializer.SerializeObject(o)).ToJson();

            Assert.That(json1, Is.EqualTo(json2));
        }

         const string Json = @"{
  ""$type"": ""DAX.CIM.PhysicalNetworkModel.PowerTransformerEndExt, DAX.CIM.PhysicalNetworkModel"",
  ""excitingCurrentZero"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.PerCent, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 4,
    ""unit"": 26,
    ""Value"": 239.0
  },
  ""loss"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.KiloActivePower, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 5,
    ""unit"": 0,
    ""Value"": 216.0
  },
  ""lossZero"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.KiloActivePower, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 6,
    ""unit"": 1,
    ""Value"": 73.0
  },
  ""nominalVoltage"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Voltage, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 7,
    ""unit"": 2,
    ""Value"": 6.0
  },
  ""uk"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.PerCent, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 8,
    ""unit"": 3,
    ""Value"": 64.0
  },
  ""b"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Susceptance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 9,
    ""unit"": 4,
    ""Value"": 192.0
  },
  ""b0"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Susceptance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 10,
    ""unit"": 5,
    ""Value"": 89.0
  },
  ""g"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Conductance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 0,
    ""unit"": 6,
    ""Value"": 101.0
  },
  ""g0"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Conductance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 1,
    ""unit"": 7,
    ""Value"": 143.0
  },
  ""phaseAngleClock"": ""phaseAngleClock48b0bcef-91a9-4244-a378-155532ef6f54"",
  ""r"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Resistance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 2,
    ""unit"": 8,
    ""Value"": 252.0
  },
  ""r0"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Resistance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 3,
    ""unit"": 9,
    ""Value"": 62.0
  },
  ""ratedS"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.ApparentPower, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 4,
    ""unit"": 10,
    ""Value"": 46.0
  },
  ""ratedU"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Voltage, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 5,
    ""unit"": 11,
    ""Value"": 117.0
  },
  ""x"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Reactance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 6,
    ""unit"": 12,
    ""Value"": 24.0
  },
  ""x0"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Reactance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 7,
    ""unit"": 13,
    ""Value"": 226.0
  },
  ""PowerTransformer"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.PowerTransformerEndPowerTransformer, DAX.CIM.PhysicalNetworkModel"",
    ""referenceType"": ""referenceTypeee786732-4c4f-4f3d-ab6a-1069ce8cba89"",
    ""ref"": ""ref969e39a7-e858-4918-a577-63482d23784b""
  },
  ""endNumber"": ""endNumberd80cd73e-1603-4ace-840e-2ef3aa94e89e"",
  ""grounded"": true,
  ""rground"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Resistance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 8,
    ""unit"": 14,
    ""Value"": 109.0
  },
  ""xground"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.Reactance, DAX.CIM.PhysicalNetworkModel"",
    ""multiplier"": 9,
    ""unit"": 15,
    ""Value"": 171.0
  },
  ""BaseVoltage"": 99.0,
  ""Terminal"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.TransformerEndTerminal, DAX.CIM.PhysicalNetworkModel"",
    ""referenceType"": ""referenceTypedc51b42c-0467-46a3-bf6e-a85332de1f6d"",
    ""ref"": ""ref5fdc2da5-c899-4ce6-8c17-dcba537b3f09""
  },
  ""mRID"": ""mRID911d60bb-0a76-4f72-a346-f7e0f1730079"",
  ""description"": ""description109192d7-1b65-4602-99bd-7e905d97e209"",
  ""name"": ""name2ad8fc95-003b-4858-8b96-360c5e7cbd67"",
  ""Names"": {
    ""$type"": ""DAX.CIM.PhysicalNetworkModel.IdentifiedObjectNames[], DAX.CIM.PhysicalNetworkModel"",
    ""$values"": [
      {
        ""$type"": ""DAX.CIM.PhysicalNetworkModel.IdentifiedObjectNames, DAX.CIM.PhysicalNetworkModel"",
        ""referenceType"": ""referenceTypee9147523-4bd3-46ae-80fe-95296004f59a"",
        ""ref"": ""refa6a106a4-52d6-46fa-af3c-d51be7395a8c""
      },
      {
        ""$type"": ""DAX.CIM.PhysicalNetworkModel.IdentifiedObjectNames, DAX.CIM.PhysicalNetworkModel"",
        ""referenceType"": ""referenceTypec6eecd7a-4c00-498d-aeb4-b376bad6466b"",
        ""ref"": ""ref5af380c7-0431-4e93-acc8-345ed6e6da83""
      },
      {
        ""$type"": ""DAX.CIM.PhysicalNetworkModel.IdentifiedObjectNames, DAX.CIM.PhysicalNetworkModel"",
        ""referenceType"": ""referenceType5705eceb-1107-474f-a8c4-f80600f44942"",
        ""ref"": ""ref55c6ef0d-144b-4b07-b0ec-3ceff885b842""
      }
    ]
  }
}";
    }
}