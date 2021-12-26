using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace ExampleProject.Api.Helpers
{
    public class JsonHelper
    {
        public class IgnoreJsonAttributesResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                IList<JsonProperty> props = base.CreateProperties(type, memberSerialization);
                foreach (var prop in props)
                {
                    prop.Ignored = false;
                    prop.Converter = null;
                    prop.PropertyName = prop.UnderlyingName;
                }
                return props;
            }
        }
    }
}
