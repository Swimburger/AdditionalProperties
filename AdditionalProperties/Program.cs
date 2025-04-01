using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

var indent = "";
void Indent() => indent += "  ";
void Unindent() => indent = indent[2..];
string[] tags = ["fantasy", "adventure"];

#region Ideal shape for reading

Console.WriteLine("Ideal shape for only reading");
Indent();
var recordForReading = JsonSerializer.Deserialize<RecordForReading>(
    """
    {
        "id":"1",
        "category":"fiction",
        "title":"The Hobbit",
        "author":"J.R.R. Tolkien",
        "tags": ["fantasy", "adventure"]
    }
    """
) ?? throw new Exception("Unexpected null record");

Console.WriteLine($"{indent}ID: {recordForReading.Id} ({recordForReading.Id.GetType()})");
Console.WriteLine(
    $"{indent}Category: {recordForReading["category"].GetString()} ({recordForReading["category"].GetType()})");
Console.WriteLine($"{indent}Tags: {recordForReading["tags"]} ({recordForReading["tags"].GetType()})");
Unindent();

#endregion

#region Ideal shape for writing

Console.WriteLine();
Console.WriteLine("Ideal shape for only writing");
Indent();
var recordForWriting = new RecordForWriting
{
    Id = "1",
    ["category"] = "fiction",
    ["title"] = "The Hobbit",
    ["author"] = "J.R.R. Tolkien",
    ["tags"] = tags
};

Console.WriteLine($"{indent}ID: {recordForWriting.Id} ({recordForWriting.Id.GetType()})");
Console.WriteLine($"{indent}Category: {recordForWriting["category"]} ({recordForWriting["category"].GetType()})");
Console.WriteLine($"{indent}Tags: {recordForWriting["tags"]} ({recordForWriting["tags"].GetType()})");
Unindent();

#endregion

#region Shape for reading and writing, not really symmetrical

Console.WriteLine();
Console.WriteLine("Shape for reading and writing, not really symmetrical");
Indent();
var recordNotReallySymmetrical = new RecordNotReallySymmetrical
{
    Id = "1",
    ["category"] = "fiction",
    ["title"] = "The Hobbit",
    ["author"] = "J.R.R. Tolkien",
    ["tags"] = tags
};

Indent();
Console.WriteLine($"{indent}---Writing---");
Indent();
Console.WriteLine($"{indent}ID: {recordNotReallySymmetrical.Id} ({recordNotReallySymmetrical.Id.GetType()})");
Console.WriteLine(
    $"{indent}Category: {recordNotReallySymmetrical["category"]} ({recordNotReallySymmetrical["category"].GetType()})");
Console.WriteLine($"{indent}Tags: {recordNotReallySymmetrical["tags"]} ({recordNotReallySymmetrical["tags"].GetType()})");
Unindent();
recordNotReallySymmetrical =
    JsonSerializer.Deserialize<RecordNotReallySymmetrical>(JsonSerializer.Serialize(recordNotReallySymmetrical)) ??
    throw new Exception("Unexpected null record");

Console.WriteLine();
Console.WriteLine($"{indent}---Reading---");
Indent();
Console.WriteLine($"{indent}ID: {recordNotReallySymmetrical.Id} ({recordNotReallySymmetrical.Id.GetType()})");
Console.WriteLine(
    $"{indent}Category: {recordNotReallySymmetrical["category"]} ({recordNotReallySymmetrical["category"].GetType()})");
Console.WriteLine($"{indent}Tags: {recordNotReallySymmetrical["tags"]} ({recordNotReallySymmetrical["tags"].GetType()})");
Unindent();
Unindent();
Unindent();

#endregion

#region Shape for reading and writing, symmetrical

try
{
    Console.WriteLine();
    Console.WriteLine("Shape for reading and writing, symmetrical");
    Indent();
    var recordSymmetrical = new RecordSymmetrical
    {
        Id = "1",
        ["category"] = "fiction",
        ["title"] = "The Hobbit",
        ["author"] = "J.R.R. Tolkien",
        ["tags"] = JsonValue.Create(tags)
    };

    Indent();
    Console.WriteLine($"{indent}---Writing---");
    Indent();
    Console.WriteLine($"{indent}ID: {recordSymmetrical.Id} ({recordSymmetrical.Id.GetType()})");
    Console.WriteLine($"{indent}Category: {recordSymmetrical["category"]} ({recordSymmetrical["category"].GetType()})");
    Console.WriteLine($"{indent}Tags: {recordSymmetrical["tags"]} ({recordSymmetrical["tags"].GetType()})");
    Unindent();

// doesn't work because of bug: https://github.com/dotnet/runtime/issues/97225
    recordSymmetrical =
        JsonSerializer.Deserialize<RecordSymmetrical>(JsonSerializer.Serialize(recordSymmetrical)) ??
        throw new Exception("Unexpected null record");

    Console.WriteLine();
    Console.WriteLine($"{indent}---Reading---");
    Indent();
    Console.WriteLine($"{indent}ID: {recordSymmetrical.Id} ({recordSymmetrical.Id.GetType()})");
    Console.WriteLine($"{indent}Category: {recordSymmetrical["category"]} ({recordSymmetrical["category"].GetType()})");
    Console.WriteLine($"{indent}Tags: {recordSymmetrical["tags"]} ({recordSymmetrical["tags"].GetType()})");
    Unindent();
    Unindent();
    Unindent();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}

#endregion

// ideal shape for only reading
public record RecordForReading
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonIgnore] public JsonElement this[string key] => AdditionalProperties[key];

    [JsonExtensionData]
    public IDictionary<string, JsonElement> AdditionalProperties { get; set; } = new Dictionary<string, JsonElement>();
}

// ideal shape for only writing
public record RecordForWriting
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonIgnore]
    public object? this[string key]
    {
        get => AdditionalProperties[key];
        set => AdditionalProperties[key] = value;
    }

    [JsonExtensionData]
    public IDictionary<string, object?> AdditionalProperties { get; set; } = new Dictionary<string, object?>();
}

// compromise between reading and writing for serialization, type looks symmetrical, but is not
public record RecordNotReallySymmetrical
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonIgnore]
    public object? this[string key]
    {
        get => AdditionalProperties[key];
        set => AdditionalProperties[key] = value;
    }

    [JsonExtensionData]
    public IDictionary<string, object?> AdditionalProperties { get; set; } = new Dictionary<string, object?>();
}

// compromise between reading and writing for symmetrical serialization (but has bug in JSON library)
public record RecordSymmetrical
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonIgnore]
    public JsonNode this[string key]
    {
        get => AdditionalProperties[key] ?? throw new KeyNotFoundException();
        set => AdditionalProperties[key] = value;
    }

    [JsonExtensionData] public JsonObject AdditionalProperties { get; set; } = new();
}