using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPUtilities.Extension
{
    public static class JTokenExtension
    {
        /// <summary>To check if this <see cref="JToken"/> is null or if the contained value is null</summary>
        public static bool IsNull(this JToken jToken)
        {
            return jToken is null || jToken.Type == JTokenType.Null;
        }

        /// <summary>
        /// Returns true if this <see cref="JToken"/> is null or if the contained value is null 
        /// or if the contained value is an enumerable with no elements or if its an empty string.
        /// Useful to check if a JArray has no elements or if a JObject has no properties.
        /// </summary>
        public static bool IsNullOrEmpty(this JToken jToken)
        {
            return jToken.IsNull() ||
                   ((jToken.Type == JTokenType.Array || jToken.Type == JTokenType.Object) && !jToken.HasValues) ||
                   (jToken.Type == JTokenType.String && string.IsNullOrEmpty(jToken.ToString()));
        }

        /// <summary>
        /// Returns the value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to the given Type T
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static T Value<T>(this JToken jToken, object key, T defaultValue = default) where T : struct
        {
            return jToken.Value<T?>(key) ?? defaultValue;
        }

        /// <summary>
        /// Returns the value mapped by the property name if it exists, coercing it if necessary.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jToken"></param>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exists</param>
        /// <returns></returns>
        public static T ValueOfNullableType<T>(this JToken jToken, object key, T defaultValue = default)
        {
            var value = jToken.Value<T>(key);
            if (!EqualityComparer<T>.Default.Equals(value, default))
            {
                return value;
            }
            return defaultValue;
        }

        /// <summary>
        /// Returns an <see cref="int"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to an <see cref="int"/>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static int GetInt(this JToken jToken, object key, int defaultValue = default)
        {
            return jToken.Value(key, defaultValue);
        }

        /// <summary>
        /// Returns a <see cref="double"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to a <see cref="double"/>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static double GetDouble(this JToken jToken, object key, double defaultValue = default)
        {
            return jToken.Value(key, defaultValue);
        }

        /// <summary>
        /// Returns a <see cref="long"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to a <see cref="long"/>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static long GetLong(this JToken jToken, object key, long defaultValue = default)
        {
            return jToken.Value(key, defaultValue);
        }

        /// <summary>
        /// Returns a <see cref="bool"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to a <see cref="bool"/>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static bool GetBool(this JToken jToken, object key, bool defaultValue = default)
        {
            return jToken.Value(key, defaultValue);
        }

        /// <summary>
        /// Returns a <see cref="string"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to a <see cref="string"/>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static string GetString(this JToken jToken, object key, string defaultValue = default)
        {
            return jToken.ValueOfNullableType(key, defaultValue);
        }

        /// <summary>
        /// Returns a <see cref="string"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Throws if it cannot be explicitly cast to a <see cref="string"/>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        public static string GetString(this JToken jToken, object key)
        {
            return jToken.Value<string>(key);
        }

        /// <summary>
        /// <para>Returns an <see cref="Enum"/> value mapped by the property name if it exists, coercing it if necessary.
        /// Returns the defaultValue of the <see cref="Enum"/> if not able not explicitly cast it or if the value doesnt exist in the Enum.
        /// </para>
        /// <para>Important: This is not a performant way to get the value. If you are receiving an int value from server, 
        /// a more efficient way would be to use <see cref="GetInt(JToken, string, int)"/> method and cast it into desired Enum.
        /// This method is intended to be used in case where a <see cref="string"/> value is to be cast as <see cref="Enum"/></para>
        /// <para>Note: TypeChecking is not done. Make sure you pass an <see cref="Enum"/> as <typeparamref name="T"/>.</para>
        /// </summary>
        /// <param name="key">The key of the property. In case of JArray, key will be the index.</param>
        /// <param name="defaultValue">the fallback value if the property doesnt exist</param>
        public static T OptEnum<T>(this JToken jToken, object key, T defaultValue = default) where T : struct
        {
            //TODO: Should I have this validation in place?
            //if (!typeof(T).IsEnum) 
            //{ 
            //    throw new ArgumentException($"{typeof(T).Name} is not an Enumeration"); 
            //} 

            var value = jToken.GetString(key);
            if (Enum.TryParse(value, true, out T enumValue) && (Enum.IsDefined(typeof(T), enumValue) | enumValue.ToString().Contains(",")))
            {
                return enumValue;
            }

            return defaultValue;
        }

        /// <summary>
        /// Returns the value mapped by key if it exists and is a <see cref="JArray"/>, or throws otherwise.
        /// </summary>
        public static JArray GetJArray(this JToken jToken, object key)
        {
            return jToken.ValueOfNullableType<JArray>(key);
        }

        /// <summary>
        /// Returns the value mapped by key if it exists and is a <see cref="JObject"/>, or throws otherwise.
        /// </summary>
        public static JObject GetJObject(this JToken jToken, object key)
        {
            return jToken.ValueOfNullableType<JObject>(key);
        }
    }
}
