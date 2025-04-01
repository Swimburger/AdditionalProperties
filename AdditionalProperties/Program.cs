using System.Collections.ObjectModel;
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
Console.WriteLine(
    $"{indent}Tags: {recordNotReallySymmetrical["tags"]} ({recordNotReallySymmetrical["tags"].GetType()})");
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
Console.WriteLine(
    $"{indent}Tags: {recordNotReallySymmetrical["tags"]} ({recordNotReallySymmetrical["tags"].GetType()})");
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

#region Shape for reading and writing, symmetrical with custom class

try
{
    Console.WriteLine();
    Console.WriteLine("Shape for reading and writing, symmetrical with custom class");
    Indent();
    var recordWithAdditionalPropertiesClass = new RecordWithAdditionalPropertiesClass
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
    Console.WriteLine(
        $"{indent}ID: {recordWithAdditionalPropertiesClass.Id} ({recordWithAdditionalPropertiesClass.Id.GetType()})");
    Console.WriteLine(
        $"{indent}Category: {recordWithAdditionalPropertiesClass["category"]} ({recordWithAdditionalPropertiesClass["category"].GetType()})");
    Console.WriteLine(
        $"{indent}Tags: {recordWithAdditionalPropertiesClass["tags"]} ({recordWithAdditionalPropertiesClass["tags"].GetType()})");
    Unindent();

    recordWithAdditionalPropertiesClass =
        JsonSerializer.Deserialize<RecordWithAdditionalPropertiesClass>(
            JsonSerializer.Serialize(recordWithAdditionalPropertiesClass)) ??
        throw new Exception("Unexpected null record");
    var additionalProperties = recordWithAdditionalPropertiesClass.AdditionalProperties.ToJsonElementDictionary();

    Console.WriteLine();
    Console.WriteLine($"{indent}---Reading---");
    Indent();
    Console.WriteLine(
        $"{indent}ID: {recordWithAdditionalPropertiesClass.Id} ({recordWithAdditionalPropertiesClass.Id.GetType()})");
    Console.WriteLine(
        $"{indent}Category: {additionalProperties["category"]} ({additionalProperties["category"].GetType()})");
    Console.WriteLine($"{indent}Tags: {additionalProperties["tags"]} ({additionalProperties["tags"].GetType()})");
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

public record RecordWithAdditionalPropertiesClass
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonIgnore]
    public object? this[string key]
    {
        get => AdditionalProperties[key] ?? throw new KeyNotFoundException();
        set => AdditionalProperties[key] = value;
    }

    [JsonExtensionData] public AdditionalProperties AdditionalProperties { get; set; } = new();
}


public record AdditionalProperties : AdditionalProperties<object?>;

public record AdditionalProperties<T> : IDictionary<string, T>
{
    private readonly Dictionary<string, T> _dictionary = new();

    public JsonObject ToJsonObject()
        => (JsonSerializer.SerializeToNode(this)
            ?? throw new InvalidOperationException("Failed to serialize AdditionalProperties to JSON Node.")
            ).AsObject();

    public JsonNode ToJsonNode() => JsonSerializer.SerializeToNode(this) ??
                                    throw new InvalidOperationException(
                                        "Failed to serialize AdditionalProperties to JSON Node.");

    public JsonElement ToJsonElement() => JsonSerializer.SerializeToElement(this);

    public JsonDocument ToJsonDocument() => JsonSerializer.SerializeToDocument(this);

    public IDictionary<string, JsonElement> ToJsonElementDictionary()
    {
        return new ReadOnlyDictionary<string, JsonElement>(this.ToDictionary(
            kvp => kvp.Key,
            kvp =>
            {
                if (kvp.Value is JsonElement jsonElement)
                {
                    return jsonElement;
                }

                return JsonSerializer.SerializeToElement(kvp.Value);
            }
        ));
    }

    public ICollection<string> Keys => _dictionary.Keys;
    public ICollection<T> Values => _dictionary.Values;
    public int Count => _dictionary.Count;
    public bool IsReadOnly => false;

    public T this[string key]
    {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public void Add(string key, T value) => _dictionary.Add(key, value);
    public void Add(KeyValuePair<string, T> item) => _dictionary.Add(item.Key, item.Value);
    public bool Remove(string key) => _dictionary.Remove(key);
    public bool Remove(KeyValuePair<string, T> item) => _dictionary.Remove(item.Key);
    public bool ContainsKey(string key) => _dictionary.ContainsKey(key);

    public bool Contains(KeyValuePair<string, T> item) => _dictionary.ContainsKey(item.Key) &&
                                                          EqualityComparer<T>.Default.Equals(_dictionary[item.Key],
                                                              item.Value);

    public bool TryGetValue(string key, out T value) => _dictionary.TryGetValue(key, out value);
    public void Clear() => _dictionary.Clear();

    public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));

        if (array.Length - arrayIndex < Count)
            throw new ArgumentException(
                "The number of elements in the source dictionary is greater than the available space from arrayIndex to the end of the destination array.");

        var i = arrayIndex;
        foreach (var kvp in _dictionary)
        {
            array[i++] = kvp;
        }
    }

    public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _dictionary.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
}