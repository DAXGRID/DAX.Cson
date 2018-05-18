using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DAX.Cson.Internals
{
    /// <summary>
    /// Contract resolver that avoids serializing properties decorated with <see cref="IgnoreDataMemberAttribute"/>
    /// </summary>
    class CustomizedContractResolver : DefaultContractResolver
    {
        static readonly Predicate<object> Yes = _ => true;
        static readonly Predicate<object> No = _ => false;

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProperty = base.CreateProperty(member, memberSerialization);

            var ignored = member.GetCustomAttributes(typeof(IgnoreDataMemberAttribute)).Any()
                          || member.GetCustomAttributes(typeof(XmlIgnoreAttribute)).Any();

            jsonProperty.ShouldSerialize = ignored ? No : Yes;

            return jsonProperty;
        }
    }
}