using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using ACBr.Net.Core.Extensions;
using ACBr.Net.DFe.Core.Attributes;

namespace ACBr.Net.DFe.Core.Serializer
{
    internal static class InterfaceSerializer
    {
        public static XObject[] Serialize(PropertyInfo prop, object parentObject, SerializerOptions options)
        {
            var value = prop.GetValue(parentObject, null);
            var objectType = ObjectType.From(value);
            var attributes = prop.GetAttributes<DFeItemAttribute>();
            var itemAttribute = attributes.SingleOrDefault(x => x.Tipo == value.GetType()) ?? attributes[0];

            if (objectType.IsIn(ObjectType.ListType, ObjectType.ArrayType, ObjectType.EnumerableType))
            {
                var list = (ICollection)prop.GetValue(parentObject, null);
                return ListSerializer.SerializeObjects(list, itemAttribute, options);
            }

            return new XObject[] { ObjectSerializer.Serialize(value, value.GetType(), itemAttribute.Name, itemAttribute.NameSpace, options) };
        }

        public static object Deserialize(PropertyInfo prop, XElement parentElement, object item, SerializerOptions options)
        {
            var tags = prop.GetAttributes<DFeItemAttribute>();
            foreach (var att in tags)
            {
                var node = parentElement.ElementsAnyNs(att.Name).FirstOrDefault();
                if (node == null) continue;

                var objectType = ObjectType.From(att.Tipo);
                if (objectType.IsIn(ObjectType.ArrayType, ObjectType.EnumerableType))
                {
                    var listElement = parentElement.ElementsAnyNs(att.Name);
                    var list = (ArrayList)ListSerializer.Deserialize(typeof(ArrayList), listElement, options);
                    var type = ListSerializer.GetItemType(att.Tipo);
                    return objectType == ObjectType.ArrayType ? list.ToArray(type) : list.Cast(type);
                }

                if (objectType == ObjectType.ListType)
                {
                    var listElement = parentElement.ElementsAnyNs(att.Name);
                    return ListSerializer.Deserialize(att.Tipo, listElement, options);
                }

                return ObjectSerializer.Deserialize(att.Tipo, node, options);
            }

            return null;
        }
    }
}