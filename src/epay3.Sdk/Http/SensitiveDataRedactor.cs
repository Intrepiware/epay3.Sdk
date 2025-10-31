using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using epay3.Sdk.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace epay3.Sdk.Http
{
    /// <summary>
    /// Utility class for redacting sensitive payment information from logs using attribute-based configuration.
    /// </summary>
    internal static class SensitiveDataRedactor
    {
        private static readonly Dictionary<Type, List<PropertyRedactionInfo>> _redactionCache =
       new Dictionary<Type, List<PropertyRedactionInfo>>();
        private static readonly Dictionary<string, PropertyRedactionInfo> _propertyNameCache =
       new Dictionary<string, PropertyRedactionInfo>(StringComparer.OrdinalIgnoreCase);
        private static readonly object _cacheLock = new object();
        private static bool _cacheInitialized = false;

        private class PropertyRedactionInfo
        {
            public string PropertyName { get; set; }
            public RedactionMode Mode { get; set; }
            public string RedactedValue { get; set; }
            public char MaskCharacter { get; set; }
        }

        /// <summary>
        /// Redacts sensitive data from JSON content based on SensitiveDataAttribute decorations.
        /// </summary>
        /// <param name="content">The JSON content to redact.</param>
        /// <param name="modelType">The type of the model being logged (optional, for better type inference).</param>
        /// <returns>The redacted JSON content.</returns>
        public static string RedactSensitiveData(string content, Type modelType = null)
        {
            if (string.IsNullOrEmpty(content))
                return content;

            try
            {
                // Parse JSON
                var token = JToken.Parse(content);

                // Redact based on known types if provided
                if (modelType != null)
                {
                    RedactToken(token, modelType);
                }
                else
                {
                    // Ensure cache is initialized for convention-based redaction
                    EnsurePropertyNameCacheInitialized();
                    // Scan all types in the SDK assembly for redaction attributes
                    RedactTokenByConvention(token);
                }

                return token.ToString(Formatting.None);
            }
            catch
            {
                // If JSON parsing fails, return original content
                return content;
            }
        }

        /// <summary>
        /// Ensures the property name cache is initialized with all sensitive properties from the SDK.
        /// </summary>
        private static void EnsurePropertyNameCacheInitialized()
        {
            lock (_cacheLock)
            {
                if (_cacheInitialized)
                    return;

                // Get all types from the SDK assembly
                var assembly = typeof(SensitiveDataRedactor).Assembly;
                var modelTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.Namespace != null);

                foreach (var type in modelTypes)
                {
                    var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var prop in properties)
                    {
                        var attr = prop.GetCustomAttribute<SensitiveDataAttribute>();
                        if (attr != null)
                        {
                            var key = prop.Name.ToLowerInvariant();
                            if (!_propertyNameCache.ContainsKey(key))
                            {
                                _propertyNameCache[key] = new PropertyRedactionInfo
                                {
                                    PropertyName = prop.Name,
                                    Mode = attr.RedactionMode,
                                    RedactedValue = attr.RedactedValue,
                                    MaskCharacter = attr.MaskCharacter
                                };
                            }
                        }
                    }
                }

                _cacheInitialized = true;
            }
        }

        /// <summary>
        /// Redacts a JToken based on a specific model type.
        /// </summary>
        private static void RedactToken(JToken token, Type modelType)
        {
            if (token == null || modelType == null)
                return;

            var redactionInfo = GetRedactionInfo(modelType);

            if (token is JObject obj)
            {
                foreach (var prop in obj.Properties().ToList())
                {
                    var propName = prop.Name;
                    var redaction = redactionInfo.FirstOrDefault(r =>
                  string.Equals(r.PropertyName, propName, StringComparison.OrdinalIgnoreCase));

                    if (redaction != null)
                    {
                        // Redact this property
                        prop.Value = RedactValue(prop.Value?.ToString(), redaction);
                    }
                    else
                    {
                        // Check if this property is a complex type
                        var modelProp = modelType.GetProperty(propName,
                               BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                        if (modelProp != null && !modelProp.PropertyType.IsPrimitive &&
                                modelProp.PropertyType != typeof(string) &&
                       modelProp.PropertyType != typeof(decimal))
                        {
                            RedactToken(prop.Value, modelProp.PropertyType);
                        }
                    }
                }
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                {
                    RedactToken(item, modelType);
                }
            }
        }

        /// <summary>
        /// Redacts a JToken by scanning all SDK types for sensitive property names.
        /// </summary>
        private static void RedactTokenByConvention(JToken token)
        {
            if (token == null)
                return;

            if (token is JObject obj)
            {
                foreach (var prop in obj.Properties().ToList())
                {
                    var key = prop.Name.ToLowerInvariant();
                    if (_propertyNameCache.TryGetValue(key, out var redaction))
                    {
                        prop.Value = RedactValue(prop.Value?.ToString(), redaction);
                    }
                    else
                    {
                        // Recurse into nested objects
                        RedactTokenByConvention(prop.Value);
                    }
                }
            }
            else if (token is JArray array)
            {
                foreach (var item in array)
                {
                    RedactTokenByConvention(item);
                }
            }
        }

        /// <summary>
        /// Gets redaction info for a type, using cache.
        /// </summary>
        private static List<PropertyRedactionInfo> GetRedactionInfo(Type type)
        {
            lock (_cacheLock)
            {
                if (_redactionCache.TryGetValue(type, out var cached))
                    return cached;

                var info = new List<PropertyRedactionInfo>();
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in properties)
                {
                    var attr = prop.GetCustomAttribute<SensitiveDataAttribute>();
                    if (attr != null)
                    {
                        info.Add(new PropertyRedactionInfo
                        {
                            PropertyName = prop.Name,
                            Mode = attr.RedactionMode,
                            RedactedValue = attr.RedactedValue,
                            MaskCharacter = attr.MaskCharacter
                        });
                    }
                }

                _redactionCache[type] = info;
                return info;
            }
        }

        /// <summary>
        /// Redacts a value based on the redaction mode.
        /// </summary>
        private static JToken RedactValue(string value, PropertyRedactionInfo redaction)
        {
            if (string.IsNullOrEmpty(value))
                return JValue.CreateString(value);

            switch (redaction.Mode)
            {
                case RedactionMode.Complete:
                    return JValue.CreateString(redaction.RedactedValue);

                case RedactionMode.MaskShowLast4:
                    if (value.Length <= 4)
                        return JValue.CreateString(new string(redaction.MaskCharacter, value.Length));
                    return JValue.CreateString(new string(redaction.MaskCharacter, value.Length - 4) + value.Substring(value.Length - 4));

                case RedactionMode.MaskShowFirstAndLast4:
                    if (value.Length <= 8)
                        return JValue.CreateString(new string(redaction.MaskCharacter, value.Length));
                    var first4 = value.Substring(0, 4);
                    var last4 = value.Substring(value.Length - 4);
                    var middle = new string(redaction.MaskCharacter, value.Length - 8);
                    return JValue.CreateString(first4 + middle + last4);

                default:
                    return JValue.CreateString(value);
            }
        }
    }
}
