using System.Text.Json;

namespace TestProject1;

[TestFixture]
public class AdditionalPropertiesTests
{
    [Test]
    public void Record_OnDeserialized_ShouldPopulateAdditionalProperties()
    {
        // Arrange
        const string json = """
                            {
                                "id": "1",
                                "category": "fiction",
                                "title": "The Hobbit"
                            }
                            """;

        // Act
        var record = JsonUtils.Deserialize<Record>(json);

        // Assert
        Assert.That(record, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(record.Id, Is.EqualTo("1"));
            Assert.That(record.AdditionalProperties["category"].GetString(), Is.EqualTo("fiction"));
            Assert.That(record.AdditionalProperties["title"].GetString(), Is.EqualTo("The Hobbit"));
        });
    }

    [Test]
    public void RecordWithWriteableAdditionalProperties_OnSerialization_ShouldIncludeAdditionalProperties()
    {
        // Arrange
        var record = new RecordWithWriteableAdditionalProperties
        {
            Id = "1",
            AdditionalProperties =  
            {
                ["category"] = "fiction",
                ["title"] = "The Hobbit"
            }
        };

        // Act
        var json = JsonUtils.Serialize(record);
        var deserializedRecord = JsonUtils.Deserialize<RecordWithWriteableAdditionalProperties>(json);

        // Assert
        Assert.That(deserializedRecord, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserializedRecord.Id, Is.EqualTo("1"));
            Assert.That(deserializedRecord.AdditionalProperties["category"], Is.InstanceOf<JsonElement>());
            Assert.That(((JsonElement)deserializedRecord.AdditionalProperties["category"]!).GetString(), Is.EqualTo("fiction"));
            Assert.That(deserializedRecord.AdditionalProperties["title"], Is.InstanceOf<JsonElement>());
            Assert.That(((JsonElement)deserializedRecord.AdditionalProperties["title"]!).GetString(), Is.EqualTo("The Hobbit"));
        });
    }

    [Test]
    public void ReadOnlyAdditionalProperties_ShouldRetrieveValuesCorrectly()
    {
        // Arrange
        var extensionData = new Dictionary<string, JsonElement>
        {
            ["key1"] = JsonUtils.SerializeToElement("value1"),
            ["key2"] = JsonUtils.SerializeToElement(123)
        };
        var readOnlyProps = new ReadOnlyAdditionalProperties();
        readOnlyProps.CopyFromExtensionData(extensionData);

        // Act & Assert
        Assert.That(readOnlyProps["key1"].GetString(), Is.EqualTo("value1"));
        Assert.That(readOnlyProps["key2"].GetInt32(), Is.EqualTo(123));
    }

    [Test]
    public void AdditionalProperties_ShouldBehaveAsDictionary()
    {
        // Arrange
        var additionalProps = new AdditionalProperties
        {
            ["key1"] = "value1",
            ["key2"] = 123
        };

        // Act
        additionalProps["key3"] = true;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(additionalProps["key1"], Is.EqualTo("value1"));
            Assert.That(additionalProps["key2"], Is.EqualTo(123));
            Assert.That((bool)additionalProps["key3"]!, Is.True);
            Assert.That(additionalProps.Count, Is.EqualTo(3));
        });
    }

    [Test]
    public void AdditionalProperties_ToJsonObject_ShouldSerializeCorrectly()
    {
        // Arrange
        var additionalProps = new AdditionalProperties
        {
            ["key1"] = "value1",
            ["key2"] = 123
        };

        // Act
        var jsonObject = additionalProps.ToJsonObject();

        // Assert
        Assert.That(jsonObject["key1"]!.GetValue<string>(), Is.EqualTo("value1"));
        Assert.That(jsonObject["key2"]!.GetValue<int>(), Is.EqualTo(123));
    }

    [Test]
    public void AdditionalProperties_MixReadAndWrite_ShouldOverwriteDeserializedProperty()
    {
        // Arrange
        const string json = """
                            {
                                "id": "1",
                                "category": "fiction",
                                "title": "The Hobbit"
                            }
                            """;
        var record = JsonUtils.Deserialize<RecordWithWriteableAdditionalProperties>(json);

        // Act
        record.AdditionalProperties["category"] = "non-fiction";

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(record, Is.Not.Null);
            Assert.That(record.Id, Is.EqualTo("1"));
            Assert.That(record.AdditionalProperties["category"], Is.EqualTo("non-fiction"));
            Assert.That(record.AdditionalProperties["title"], Is.InstanceOf<JsonElement>());
            Assert.That(((JsonElement)record.AdditionalProperties["title"]!).GetString(), Is.EqualTo("The Hobbit"));
        });
    }
}

