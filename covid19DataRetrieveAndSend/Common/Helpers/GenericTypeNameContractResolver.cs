using System;
using System.Reflection;
using covid19DataRetrieveAndSend.Common.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace covid19DataRetrieveAndSend.Common.Helpers
{
    public class GenericTypeNameContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            var attribute = member.GetCustomAttribute<JsonPropertyGenericTypeNameAttribute>();

            if (attribute == null) return property;

            var type = member.DeclaringType;
            if (!type.IsGenericType)
                throw new InvalidOperationException($"{type} is not a generic type");
            if (type.IsGenericTypeDefinition)
                throw new InvalidOperationException($"{type} is a generic type definition, it must be a constructed generic type");
            var typeArgs = type.GetGenericArguments();
            if (attribute.TypeParameterPosition >= typeArgs.Length)
                throw new ArgumentException($"Can't get type argument at position {attribute.TypeParameterPosition}; {type} has only {typeArgs.Length} type arguments");
            property.PropertyName = typeArgs[attribute.TypeParameterPosition].Name;

            return property;
        }
    }
}
