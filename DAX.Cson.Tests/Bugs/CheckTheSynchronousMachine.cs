using System;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Tests.Extensions;
using NUnit.Framework;
using Ploeh.AutoFixture.Kernel;

namespace DAX.Cson.Tests.Bugs
{
    [TestFixture]
    public class CheckTheSynchronousMachine : FixtureBase
    {
        CsonSerializer _serializer;

        protected override void SetUp()
        {
            _serializer = new CsonSerializer();
        }

        [Test]
        public void ItWorks()
        {
            var synchronousMachineWithMu = new SynchronousMachine { muSpecified = true };
            var synchronousMachineWithoutMu = new SynchronousMachine { muSpecified = false };

            var csonWithMu = _serializer.SerializeObject(synchronousMachineWithMu);
            var csonWithoutMu = _serializer.SerializeObject(synchronousMachineWithoutMu);

            Console.WriteLine(csonWithMu.PrettifyJson());
            Console.WriteLine(csonWithoutMu.PrettifyJson());
        }
    }
}