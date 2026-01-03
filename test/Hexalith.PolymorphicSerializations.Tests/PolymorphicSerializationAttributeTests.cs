// <copyright file="PolymorphicSerializationAttributeTests.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.Tests;

using System;

using Hexalith.PolymorphicSerializations.Tests.Objects;

using Shouldly;

/// <summary>
/// Tests for <see cref="PolymorphicSerializationAttribute"/> class.
/// </summary>
public class PolymorphicSerializationAttributeTests
{
    /// <summary>
    /// Tests that constructor parameters are optional with correct defaults.
    /// </summary>
    [Fact]
    public void ConstructorOptionalParametersUsesDefaults()
    {
        // Act
        PolymorphicSerializationAttribute attribute = new();

        // Assert
        attribute.Name.ShouldBeNull();
        attribute.Version.ShouldBe(1);
        attribute.BaseType.ShouldBeNull();
    }

    /// <summary>
    /// Tests that the constructor properly sets the properties.
    /// </summary>
    [Fact]
    public void ConstructorSetsProperties()
    {
        // Arrange
        Type baseType = typeof(AttributeTestBase);
        const string name = "CustomName";
        const int version = 2;

        // Act
        PolymorphicSerializationAttribute attribute = new(name, version, baseType);

        // Assert
        attribute.Name.ShouldBe(name);
        attribute.Version.ShouldBe(version);
        attribute.BaseType.ShouldBe(baseType);
    }

    /// <summary>
    /// Tests that the GetTypeName static method returns correct name with version 1.
    /// </summary>
    [Fact]
    public void GetTypeNameStaticWithVersion1ReturnsNameOnly()
    {
        // Act
        string result = PolymorphicSerializationAttribute.GetTypeName("TestType", 1);

        // Assert
        result.ShouldBe("TestType");
    }

    /// <summary>
    /// Tests that the GetTypeName static method returns name with version suffix for version > 1.
    /// </summary>
    [Fact]
    public void GetTypeNameStaticWithVersionGreaterThan1ReturnsNameWithVersion()
    {
        // Act
        string result = PolymorphicSerializationAttribute.GetTypeName("TestType", 2);

        // Assert
        result.ShouldBe("TestTypeV2");
    }

    /// <summary>
    /// Tests that the instance GetTypeName method uses the type name if Name is not set.
    /// </summary>
    [Fact]
    public void GetTypeNameWithNameNotSetUsesTypeName()
    {
        // Arrange
        PolymorphicSerializationAttribute attribute = new(version: 1);

        // Act
        string result = attribute.GetTypeName(typeof(TestType1));

        // Assert
        result.ShouldBe("TestType1");
    }

    /// <summary>
    /// Tests that the instance GetTypeName method uses the Name property if set.
    /// </summary>
    [Fact]
    public void GetTypeNameWithNameSetUsesNameProperty()
    {
        // Arrange
        PolymorphicSerializationAttribute attribute = new("CustomName", 1);

        // Act
        string result = attribute.GetTypeName(typeof(TestType1));

        // Assert
        result.ShouldBe("CustomName");
    }

    /// <summary>
    /// Tests that the instance GetTypeName method throws exception when type parameter is null.
    /// </summary>
    [Fact]
    public void GetTypeNameWithNullTypeThrowsArgumentNullException()
    {
        // Arrange
        PolymorphicSerializationAttribute attribute = new();

        // Act & Assert
        _ = Should.Throw<ArgumentNullException>(() => attribute.GetTypeName(null!));
    }

    /// <summary>
    /// Tests that the instance GetTypeName method includes version suffix for version > 1.
    /// </summary>
    [Fact]
    public void GetTypeNameWithVersionGreaterThan1IncludesVersionSuffix()
    {
        // Arrange
        PolymorphicSerializationAttribute attribute = new(version: 2);

        // Act
        string result = attribute.GetTypeName(typeof(TestType1));

        // Assert
        result.ShouldBe("TestType1V2");
    }
}