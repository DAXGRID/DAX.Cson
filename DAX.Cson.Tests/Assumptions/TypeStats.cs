using System;
using System.Collections.Generic;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using DAX.Cson.Tests.Extensions;
using NUnit.Framework;

namespace DAX.Cson.Tests.Assumptions
{
    [TestFixture]
    public class TypeStats
    {
        [Test]
        public void PrintValueTypes()
        {
            var allTypes = GetAllTypes();

            var entityTypes = allTypes.Where(t => typeof(IdentifiedObject).IsAssignableFrom(t)).ToList();
            var valueTypes = allTypes.Except(entityTypes).Except(new[] { typeof(PhysicalNetworkModelEnvelope) }).ToList();

            var entityList = GetList(entityTypes);
            var valueList = GetList(valueTypes);

            Console.WriteLine($@"Entities:

{entityList}

Values:

{valueList}");
        }

        [Test]
        public void ObjectReferences()
        {
            var referenceTypes = GetAllTypes()
                .Where(t => t.GetProperties().Length == 2
                            && t.HasProperty(nameof(TapScheduleTapChanger.referenceType))
                            && t.HasProperty(nameof(TapScheduleTapChanger.@ref)))
                .ToList();

            Console.WriteLine($@"Object reference types:

{GetList(referenceTypes)}");
        }

        static List<Type> GetAllTypes()
        {
            var ns = typeof(IdentifiedObject).Namespace;

            var allTypes = typeof(IdentifiedObject).Assembly
                .GetTypes().Where(t => t.Namespace == ns)
                .ToList();
            return allTypes;
        }

        static string GetList(IEnumerable<Type> types)
        {
            return string.Join(Environment.NewLine, types.OrderBy(t => t.Name).Select(type => $"    {type}"));
        }
    }
}