using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

var indent = "";
void Indent() => indent += "  ";
void Unindent() => indent = indent[2..];
string[] tags = ["fantasy", "adventure"];

try
{
    Console.WriteLine();
    Console.WriteLine("Shape for reading and writing, symmetrical with custom class");
    Indent();
    var record = JsonUtils.Deserialize<Record>(
        """
        {
            "id":"1",
            "category":"fiction",
            "title":"The Hobbit",
            "author":"J.R.R. Tolkien",
            "tags": ["fantasy", "adventure"],
            "borrower": null
        }
        """
    ) ?? throw new Exception("Unexpected null record");
    var additionalProperties = record.AdditionalProperties;

    Indent();
    Console.WriteLine($"{indent}---Writing---");
    Indent();
    Console.WriteLine(
        $"{indent}ID: {record.Id} ({record.Id.GetType()})");
    Console.WriteLine(
        $"{indent}Category: {additionalProperties["category"]} ({additionalProperties["category"].GetType()})");
    Console.WriteLine(
        $"{indent}Tags: {additionalProperties["tags"]} ({additionalProperties["tags"].GetType()})");
    Console.WriteLine(
        $"{indent}Borrower: {additionalProperties["borrower"]} ({additionalProperties["borrower"].GetType()})");
    Unindent();

    record =
        JsonUtils.Deserialize<Record>(
            JsonUtils.Serialize(record)) ??
        throw new Exception("Unexpected null record");
    additionalProperties = record.AdditionalProperties;

    Console.WriteLine();
    Console.WriteLine($"{indent}---Reading---");
    Indent();
    Console.WriteLine(
        $"{indent}ID: {record.Id} ({record.Id.GetType()})");
    Console.WriteLine(
        $"{indent}Category: {additionalProperties["category"]} ({additionalProperties["category"].GetType()})");
    Console.WriteLine($"{indent}Tags: {additionalProperties["tags"]} ({additionalProperties["tags"].GetType()})");
    Console.WriteLine(
        $"{indent}Borrower: {additionalProperties["borrower"]} ({additionalProperties["borrower"].GetType()})");
    Unindent();
    Unindent();
    Unindent();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

try
{
    Console.WriteLine();
    Console.WriteLine("Shape for reading and writing, symmetrical");
    Indent();
    var recordSymmetrical = new RecordWithWriteableAdditionalProperties
    {
        Id = "1",
        AdditionalProperties = new AdditionalProperties
        {
            ["category"] = "fiction",
            ["title"] = "The Hobbit",
            ["author"] = "J.R.R. Tolkien",
            ["tags"] = JsonValue.Create(tags),
            ["borrower"] = null
        }
    };

    Indent();
    Console.WriteLine($"{indent}---Writing---");
    Indent();
    var additionalProperties = recordSymmetrical.AdditionalProperties;
    Console.WriteLine($"{indent}ID: {recordSymmetrical.Id} ({recordSymmetrical.Id.GetType()})");
    Console.WriteLine(
        $"{indent}Category: {additionalProperties["category"]} ({additionalProperties["category"].GetType()})");
    Console.WriteLine($"{indent}Tags: {additionalProperties["tags"]} ({additionalProperties["tags"].GetType()})");
    Console.WriteLine(
        $"{indent}Borrower: {additionalProperties["borrower"]} ({additionalProperties["borrower"]?.GetType()})");
    Unindent();

// doesn't work because of bug: https://github.com/dotnet/runtime/issues/97225
    recordSymmetrical =
        JsonUtils.Deserialize<RecordWithWriteableAdditionalProperties>(
            JsonUtils.Serialize(recordSymmetrical)) ??
        throw new Exception("Unexpected null record");

    additionalProperties = recordSymmetrical.AdditionalProperties;
    Console.WriteLine();
    Console.WriteLine($"{indent}---Reading---");
    Indent();
    Console.WriteLine($"{indent}ID: {recordSymmetrical.Id} ({recordSymmetrical.Id.GetType()})");
    Console.WriteLine(
        $"{indent}Category: {additionalProperties["category"]} ({additionalProperties["category"].GetType()})");
    Console.WriteLine($"{indent}Tags: {additionalProperties["tags"]} ({additionalProperties["tags"].GetType()})");
    Console.WriteLine(
        $"{indent}Borrower: {additionalProperties["borrower"]} ({additionalProperties["borrower"]?.GetType()})");
    Unindent();
    Unindent();
    Unindent();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

public record Record : IJsonOnDeserialized
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonExtensionData]
    private readonly IDictionary<string, JsonElement> _extensionData = new Dictionary<string, JsonElement>();

    [JsonIgnore] public ReadOnlyAdditionalProperties AdditionalProperties { get; } = new();

    void IJsonOnDeserialized.OnDeserialized()
    {
        AdditionalProperties.CopyFromExtensionData(_extensionData);
    }
}

public record RecordWithWriteableAdditionalProperties : IJsonOnDeserialized, IJsonOnSerializing
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonExtensionData]
    private readonly IDictionary<string, object?> _extensionData = new Dictionary<string, object?>();

    [JsonIgnore] public AdditionalProperties<object?> AdditionalProperties { get; set; } = new();

    void IJsonOnDeserialized.OnDeserialized()
    {
        AdditionalProperties.CopyFromExtensionData(_extensionData);
    }

    void IJsonOnSerializing.OnSerializing()
    {
        AdditionalProperties.CopyToExtensionData(_extensionData);
    }
}

