using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Ploeh.AutoFixture.Kernel;

namespace DAX.Cson.Tests.Stubs
{
    class IgnoredPropertyOmitter : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (request is PropertyInfo propertyInfo
                && propertyInfo.GetCustomAttributes(typeof(IgnoreDataMemberAttribute)).Any())
            {
                return new OmitSpecimen();
            }

            return new NoSpecimen();
        }
    }
}