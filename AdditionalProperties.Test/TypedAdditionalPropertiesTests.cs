namespace TestProject1;

[TestFixture]
public class TypedAdditionalPropertiesTests
{
    [Test]
    public void RecordWithReadonlyAdditionalPropertiesInts_OnDeserialized_ShouldPopulateAdditionalProperties()
    {
        // Arrange
        const string json = """
                            {
                                "extra1": 42,
                                "extra2": 99
                            }
                            """;

        // Act
        var record = JsonUtils.Deserialize<RecordWithReadonlyAdditionalPropertiesInts>(json);

        // Assert
        Assert.That(record, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(record.AdditionalProperties["extra1"], Is.EqualTo(42));
            Assert.That(record.AdditionalProperties["extra2"], Is.EqualTo(99));
        });
    }

    [Test]
    public void RecordWithAdditionalPropertiesInts_OnSerialization_ShouldIncludeAdditionalProperties()
    {
        // Arrange
        var record = new RecordWithAdditionalPropertiesInts
        {
            AdditionalProperties =
            {
                ["extra1"] = 42,
                ["extra2"] = 99
            }
        };

        // Act
        var json = JsonUtils.Serialize(record);
        var deserializedRecord = JsonUtils.Deserialize<RecordWithAdditionalPropertiesInts>(json);

        // Assert
        Assert.That(deserializedRecord, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserializedRecord.AdditionalProperties["extra1"], Is.EqualTo(42));
            Assert.That(deserializedRecord.AdditionalProperties["extra2"], Is.EqualTo(99));
        });
    }

    [Test]
    public void RecordWithReadonlyAdditionalPropertiesDictionaries_OnDeserialized_ShouldPopulateAdditionalProperties()
    {
        // Arrange
        const string json = """
                            {
                                "extra1": { "key1": true, "key2": false },
                                "extra2": { "key3": true }
                            }
                            """;

        // Act
        var record = JsonUtils.Deserialize<RecordWithReadonlyAdditionalPropertiesDictionaries>(json);

        // Assert
        Assert.That(record, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(record.AdditionalProperties["extra1"]["key1"], Is.True);
            Assert.That(record.AdditionalProperties["extra1"]["key2"], Is.False);
            Assert.That(record.AdditionalProperties["extra2"]["key3"], Is.True);
        });
    }

    [Test]
    public void RecordWithAdditionalPropertiesDictionaries_OnSerialization_ShouldIncludeAdditionalProperties()
    {
        // Arrange
        var record = new RecordWithAdditionalPropertiesDictionaries
        {
            AdditionalProperties =
            {
                ["extra1"] = new Dictionary<string, bool> { { "key1", true }, { "key2", false } },
                ["extra2"] = new Dictionary<string, bool> { { "key3", true } }
            }
        };

        // Act
        var json = JsonUtils.Serialize(record);
        var deserializedRecord = JsonUtils.Deserialize<RecordWithAdditionalPropertiesDictionaries>(json);

        // Assert
        Assert.That(deserializedRecord, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(deserializedRecord.AdditionalProperties["extra1"]["key1"], Is.True);
            Assert.That(deserializedRecord.AdditionalProperties["extra1"]["key2"], Is.False);
            Assert.That(deserializedRecord.AdditionalProperties["extra2"]["key3"], Is.True);
        });
    }
}
