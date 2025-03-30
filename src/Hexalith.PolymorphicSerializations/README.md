# Hexalith.PolymorphicSerializations

A .NET library that simplifies polymorphic serialization and deserialization using System.Text.Json.

## Overview

Hexalith.PolymorphicSerializations provides utilities and helpers for handling polymorphic serialization scenarios in .NET applications. It enables type-based JSON serialization and deserialization by managing type discriminators and mapping between base types and their derived implementations.

## Features

- Polymorphic serialization and deserialization of object hierarchies
- Type discrimination using a customizable property name
- Version-aware type handling
- Fluent builder API for serialization configuration
- Custom attribute-based type metadata

## Installation

```bash
dotnet add package Hexalith.PolymorphicSerializations
```

## Usage

### Basic Example

```csharp
using Hexalith.PolymorphicSerializations;
using System.Text.Json;

// Create JSON serializer options with polymorphic resolution
JsonSerializerOptions options = new()
{
    WriteIndented = true,
    TypeInfoResolver = new PolymorphicSerializationResolver()
};

// Serialize polymorphic objects
string json = JsonSerializer.Serialize(myPolymorphicObject, options);

// Deserialize back to the correct concrete type
var deserialized = JsonSerializer.Deserialize<BaseType>(json, options);
```

### Defining Polymorphic Types

Create base types that derive from `PolymorphicRecordBase`:

```csharp
[DataContract]
public abstract record Animal : PolymorphicRecordBase
{
    public string Name { get; init; }
}

[PolymorphicSerialization(version: 1)]
public record Dog(string Name, string Breed) : Animal;

[PolymorphicSerialization(name: "Feline", version: 2)]
public record Cat(string Name, int Lives) : Animal;
```

### Registering Type Mappers

Use the builder pattern to register type mappers:

```csharp
// Create mappers with fluent API
var mappers = new PolymorphicSerializationResolverBuilder()
    .AddMapper(new PolymorphicSerializationMapper<Dog, Animal>("Dog"))
    .AddMapper(new PolymorphicSerializationMapper<Cat, Animal>("FelineV2"))
    .Build();

// Register mappers globally
PolymorphicSerializationResolver.TryAddDefaultMappers(mappers);
```

### Using Default JSON Serializer Options

The library provides default serializer options:

```csharp
// Use pre-configured default options
string json = JsonSerializer.Serialize(myAnimal, PolymorphicHelper.DefaultJsonSerializerOptions);
Animal deserializedAnimal = JsonSerializer.Deserialize<Animal>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
```

## Key Components

### PolymorphicSerializationAttribute

This attribute provides metadata for a polymorphic class:

```csharp
[PolymorphicSerialization(name: "CustomName", version: 2, baseType: typeof(BaseClass))]
public record DerivedClass : BaseClass;
```

Parameters:

- `name`: Custom name for the type (defaults to the class name)
- `version`: Version of the type (defaults to 1)
- `baseType`: Base type of the class (optional)

### PolymorphicRecordBase

A marker base record that can be used as the root of polymorphic hierarchies.

### PolymorphicSerializationResolver

Extends `DefaultJsonTypeInfoResolver` to handle polymorphic serialization:

- Maintains a registry of type mappers
- Configures JSON serialization with type discriminators
- Handles derived type resolution

### PolymorphicSerializationMapper

Maps derived types to their base types with type discriminators:

```csharp
var mapper = new PolymorphicSerializationMapper<ConcreteType, BaseType>("TypeDiscriminator");
```

### PolymorphicHelper

Provides utility methods:

- Default JSON serializer options
- Type discriminator property name (default: "$type")
- Helper methods to extract polymorphic type information

## Advanced Usage

### Custom Type Discrimination

You can extract type information from any type or object instance:

```csharp
Type myType = typeof(Dog);
var (name, typeName, version) = myType.GetPolymorphicTypeDiscriminator();

// Or from an instance
Dog dog = new("Rex", "Golden Retriever");
var typeInfo = dog.GetPolymorphicTypeDiscriminator();
```

### Versioning

The library supports versioning of types:

```csharp
// Version 1 (no suffix)
[PolymorphicSerialization(version: 1)]
public record OrderV1 : Order;

// Version 2 (with suffix V2)
[PolymorphicSerialization(version: 2)]
public record OrderV2 : Order;
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Company

Developed and maintained by [ITANEO](https://www.itaneo.com)
