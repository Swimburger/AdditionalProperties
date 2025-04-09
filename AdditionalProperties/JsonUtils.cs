using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

public static class JsonUtils
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers =
            {
                static typeInfo =>
                {
                    if (typeInfo.Kind == JsonTypeInfoKind.Object &&
                        typeInfo.Properties.All(prop => !prop.IsExtensionData))
                    {
                        var extensionProp = typeInfo.Type
                            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                            .FirstOrDefault(prop => prop.GetCustomAttribute<JsonExtensionDataAttribute>() != null);

                        if (extensionProp is not null)
                        {
                            var jsonPropertyInfo =
                                typeInfo.CreateJsonPropertyInfo(extensionProp.FieldType, extensionProp.Name);
                            jsonPropertyInfo.Get = extensionProp.GetValue;
                            jsonPropertyInfo.Set = extensionProp.SetValue;
                            jsonPropertyInfo.IsExtensionData = true;
                            typeInfo.Properties.Add(jsonPropertyInfo);
                        }
                    }
                }
            }
        }
    };

    public static T? Deserialize<T>(string json) => JsonSerializer.Deserialize<T>(json, _options);

    public static string Serialize<T>(T obj) => JsonSerializer.Serialize(obj, _options);

    public static JsonElement SerializeToElement(object value) => JsonSerializer.SerializeToElement(value, _options);
}