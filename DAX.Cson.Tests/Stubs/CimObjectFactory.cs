using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using DAX.CIM.PhysicalNetworkModel;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace DAX.Cson.Tests.Stubs
{
    public class CimObjectFactory
    {
        readonly Fixture _fixture;
        readonly SpecimenContext _specimenContext;
        readonly List<Type> _objectTypes;
        Random _random;

        public CimObjectFactory()
        {
            _fixture = new Fixture();
            _specimenContext = new SpecimenContext(_fixture);

            _objectTypes = typeof(IdentifiedObject).Assembly
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Where(t => typeof(IdentifiedObject).IsAssignableFrom(t))
                .ToList();

            _random = new Random(DateTime.Now.GetHashCode());
        }

        public IEnumerable<IdentifiedObject> Read()
        {
            while (true)
            {
                var type = _objectTypes[_random.Next(_objectTypes.Count)];

                yield return (IdentifiedObject)_specimenContext.Resolve(type);
            }
        }
    }
}