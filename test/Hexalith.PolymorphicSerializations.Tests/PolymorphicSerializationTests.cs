// <copyright file="PolymorphicSerializationTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.Tests;

using System.Text.Json;
using System.Text.Json.Serialization;

using Hexalith.PolymorphicSerializations.Tests.Objects;

using Shouldly;

/// <summary>
/// Tests for polymorphic serialization and deserialization.
/// </summary>
public class PolymorphicSerializationTests
{
    /// <summary>
    /// The JSON serializer options with polymorphic serialization.
    /// </summary>
    private readonly JsonSerializerOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolymorphicSerializationTests"/> class.
    /// </summary>
    public PolymorphicSerializationTests()
    {
        // Register the mappers
        PolymorphicSerializationResolver.TryAddDefaultMapper(new PolymorphicSerializationMapper<TestType1, Polymorphic>("TestType1"));

        // Configure the options
        _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            TypeInfoResolver = new PolymorphicSerializationResolver(),
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
        };
    }

    /// <summary>
    /// Tests that the discriminator is required.
    /// </summary>
    [Fact]
    public void DeserializeMissingDiscriminatorReturnsBaseType()
    {
        // Arrange
        const string json = """
        {
          "Id": "id1",
          "Name": "Test Name",
          "Value": 42
        }
        """;

        // Act & Assert
        Polymorphic? result = JsonSerializer.Deserialize<Polymorphic>(json, _options);

        // Assert
        _ = result.ShouldNotBeNull();
    }

    /// <summary>
    /// Tests that a base class can be correctly deserialized into its specific derived type.
    /// </summary>
    [Fact]
    public void DeserializeTestBaseWithType1DiscriminatorReturnsTestType1()
    {
        // Arrange
        const string json = """
        {
          "$type": "TestType1",
          "Id": "id1",
          "Name": "Test Name",
          "Value": 42
        }
        """;

        // Act
        Polymorphic? result = JsonSerializer.Deserialize<Polymorphic>(json, _options);

        // Assert
        _ = result.ShouldNotBeNull();
        _ = result.ShouldBeOfType<TestType1>();
        var type1 = (TestType1)result;
        type1.Id.ShouldBe("id1");
        type1.Name.ShouldBe("Test Name");
        type1.Value.ShouldBe(42);
    }

    /// <summary>
    /// Tests that serializing with a different discriminator throws a JsonException when deserializing.
    /// </summary>
    [Fact]
    public void DeserializeWrongDiscriminatorThrowsJsonException()
    {
        // Arrange
        const string json = """
        {
          "$type": "NonExistentType",
          "Id": "id3",
          "Name": "Test Name",
          "Value": 42
        }
        """;

        // Act & Assert
        _ = Should.Throw<JsonException>(() => JsonSerializer.Deserialize<Polymorphic>(json, _options));
    }

    /// <summary>
    /// Tests that the PolymorphicHelper returns correct discriminator information.
    /// </summary>
    [Fact]
    public void GetPolymorphicTypeDiscriminatorReturnsCorrectInfo()
    {
        // Arrange
        TestType1 value = new("id1", "Test Name", 42);

        // Act
        (string name, string typeName, int version) = value.GetPolymorphicTypeDiscriminator();

        // Assert
        name.ShouldBe("TestType1");
        typeName.ShouldBe("TestType1");
        version.ShouldBe(1);
    }

    /// <summary>
    /// Tests non-registered type to ensure it throws a JsonException during serialization.
    /// </summary>
    [Fact]
    public void SerializeNonRegisteredTypeThrowsJsonException()
    {
        // Arrange
        NonRegisteredType value = new("ABC123", "Test Title");

        // Act & Assert
        _ = Should.Throw<NotSupportedException>(() => JsonSerializer.Serialize<Polymorphic>(value, _options));
    }

    /// <summary>
    /// Tests non-registered type to ensure it throws a JsonException during serialization.
    /// </summary>
    [Fact]
    public void SerializeNonRegisteredTypeWithoutBaseTypeSucceeds()
    {
        // Arrange
        NonRegisteredType value = new("ABC123", "Test Title");

        // Act & Assert
        string json = JsonSerializer.Serialize(value, _options);

        // Assert
        json.ShouldContain("\"Code\": \"ABC123\"");
        json.ShouldContain("\"Title\": \"Test Title\"");
    }

    /// <summary>
    /// Tests that TestType1 is correctly serialized with its discriminator.
    /// </summary>
    [Fact]
    public void SerializeTestType1IncludesTypeDiscriminator()
    {
        // Arrange
        TestType1 value = new("id1", "Test Name", 42);

        // Act
        string json = JsonSerializer.Serialize<Polymorphic>(value, _options);

        // Assert
        json.ShouldContain("\"$type\": \"TestType1\"");
        json.ShouldContain("\"Id\": \"id1\"");
        json.ShouldContain("\"Name\": \"Test Name\"");
        json.ShouldContain("\"Value\": 42");
    }
}