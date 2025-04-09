using System.Text.Json;
using System.Text.Json.Serialization;

public record RecordWithReadonlyAdditionalPropertiesInts : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData = new Dictionary<string, JsonElement>();

    [JsonIgnore] public ReadOnlyAdditionalProperties<int> AdditionalProperties { get; } = new();

    void IJsonOnDeserialized.OnDeserialized()
    {
        AdditionalProperties.CopyFromExtensionData(_extensionData);
    }
}

public record RecordWithAdditionalPropertiesInts : IJsonOnDeserialized, IJsonOnSerializing
{
    [JsonExtensionData]
    private readonly IDictionary<string, object?> _extensionData = new Dictionary<string, object?>();

    [JsonIgnore] public AdditionalProperties<int> AdditionalProperties { get; } = new();

    void IJsonOnDeserialized.OnDeserialized()
    {
        AdditionalProperties.CopyFromExtensionData(_extensionData);
    }

    void IJsonOnSerializing.OnSerializing()
    {
        AdditionalProperties.CopyToExtensionData(_extensionData);
    }
}

public record RecordWithReadonlyAdditionalPropertiesDictionaries : IJsonOnDeserialized
{
    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData = new Dictionary<string, JsonElement>();

    [JsonIgnore] public ReadOnlyAdditionalProperties<Dictionary<string, bool>> AdditionalProperties { get; } = new();

    void IJsonOnDeserialized.OnDeserialized()
    {
        AdditionalProperties.CopyFromExtensionData(_extensionData);
    }
}

public record RecordWithAdditionalPropertiesDictionaries : IJsonOnDeserialized, IJsonOnSerializing
{
    [JsonExtensionData]
    private readonly IDictionary<string, object?> _extensionData = new Dictionary<string, object?>();

    [JsonIgnore] public AdditionalProperties<Dictionary<string, bool>> AdditionalProperties { get; } = new();

    void IJsonOnDeserialized.OnDeserialized()
    {
        AdditionalProperties.CopyFromExtensionData(_extensionData);
    }

    void IJsonOnSerializing.OnSerializing()
    {
        AdditionalProperties.CopyToExtensionData(_extensionData);
    }
}