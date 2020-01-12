using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Extension
{
    public static class JObjectExtension
    {
        public static bool ContainsKeyAndValue(this JObject jObject, string propertyName)
        {
            if (!jObject.ContainsKey(propertyName)) { return false; }
            var jToken = jObject.Value<JToken>(propertyName);
            return jToken.HasValues;
        }

        /// <summary>
        /// Returns true if this <see cref="JObject"/> has a property with the given key.
        /// </summary>
        //public static bool ContainsKey(this JObject jObject, string propertyName)
        //{
        //    //TODO: Replace this with the public JObject.ContainsKey method once you update NewtonSoftJson to > v11.0
        //    return jObject.Properties().Any(p => p.Name == propertyName);
        //}

        /// <summary>
        /// Returns true if this object has no mapping for the key or if it has a mapping whose value is null.
        /// Throws NullReferenceException if the <see cref="JObject"/> itself is null.
        /// Caution: Doesn't check whether the JObject in question is null or not.
        /// </summary>
        public static bool IsNull(this JObject jObject, string propertyName)
        {
            return jObject[propertyName].IsNull();
        }

        /// <summary>
        /// Returns true if this <see cref="JObject"/> has no mapping for the key or if it has a mapping whose value is null,
        /// or if the value is an array which in turn has no values.
        /// Caution: Doesn't check whether the JObject in question is null or not.
        /// </summary>
        public static bool IsNullOrEmpty(this JObject jObject, string propertyName)
        {
            return jObject[propertyName].IsNullOrEmpty();
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{string}"/> containing the keys in this object.
        /// </summary>
        public static IEnumerable<string> Keys(this JObject jObject)
        {
            return jObject.Properties().Select(p => p.Name);
        }

        /// <summary>
        /// Returns the number of key/value mappings in this object. 
        /// </summary>
        public static int Length(this JObject jObject)
        {
            return jObject.Properties().Count();
        }
    }
}
