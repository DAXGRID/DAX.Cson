using System;
using System.Collections.Generic;
using System.Linq;
using DAX.CIM.PhysicalNetworkModel;
using Newtonsoft.Json.Serialization;

namespace DAX.Cson.Converters
{
    class ShortNameBinder : DefaultSerializationBinder
    {
        //const string MagicAssemlyName = "(DAX)";
        readonly Dictionary<Type, string> _typeToName = new Dictionary<Type, string>();
        readonly Dictionary<string, Type> _nameToType = new Dictionary<string, Type>();

        public ShortNameBinder()
        {
            var ns = typeof(IdentifiedObject).Namespace;
            var types = typeof(IdentifiedObject).Assembly.GetTypes()
                .Where(obj => obj.Namespace == ns);

            foreach (var type in types)
            {
                AddType(type);
            }
        }

        public override void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            string name;

            if (_typeToName.TryGetValue(serializedType, out name))
            {
                assemblyName = null;
                typeName = name;
            }
            else
            {
                base.BindToName(serializedType, out assemblyName, out typeName);
            }
        }

        public override Type BindToType(string assemblyName, string typeName)
        {
            Type type;

            if (string.IsNullOrWhiteSpace(assemblyName) && _nameToType.TryGetValue(typeName, out type))
            {
                return type;
            }

            return base.BindToType(assemblyName, typeName);
        }

        void AddType(Type type)
        {
            var name = type.Name;

            AddType(type, name);
            AddType(type.MakeArrayType(), $"{name}[]");
        }

        void AddType(Type type, string name)
        {
            _typeToName[type] = name;
            _nameToType[name] = type;
        }
    }
}