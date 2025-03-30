# Hexalith.PolymorphicSerializations.CodeGenerators

## Overview

The Hexalith.PolymorphicSerializations.CodeGenerators package provides a source generator for System.Text.Json polymorphic serialization in .NET. It automatically generates serialization mapper classes and DI registration code for types marked with the `PolymorphicSerializationAttribute`.

This library simplifies the implementation of polymorphic serialization in .NET applications by eliminating the need to write boilerplate code for serialization mapping.

## Features

- Automatically generates mapper classes for polymorphic serialization
- Creates DI registration extension methods for easy integration with dependency injection
- Supports versioning of polymorphic types
- Works with both class and record types
- Provides compile-time validation of inheritance hierarchy
- Integrates with System.Text.Json

## Installation

Add the package to your project:

```bash
dotnet add package Hexalith.PolymorphicSerializations.CodeGenerators
```

Also add the core package:

```bash
dotnet add package Hexalith.PolymorphicSerializations
```

## Usage

### 1. Define Your Polymorphic Types

Create a base class/record that inherits from `PolymorphicRecordBase`:

```csharp
using Hexalith.PolymorphicSerializations;

[PolymorphicSerialization]
public abstract record MyBaseRecord(string Id);
```

Create derived classes/records and mark them with the `PolymorphicSerializationAttribute`:

```csharp
[PolymorphicSerialization]
public record FirstDerivedType(string Id, string Name, int Value) 
    : MyBaseRecord(Id);

[PolymorphicSerialization("CustomType", 2)]
public record SecondDerivedType(string Id, string Description, bool IsActive) 
    : MyBaseRecord(Id);
```

### 2. Use the Generated Extension Methods

The source generator will create extension methods to register the mappers with dependency injection:

```csharp
// In your startup/program.cs
using YourNamespace.Extensions;

services.AddYourNamespacePolymorphicMappers();

// Or for scenarios without DI
YourNamespace.Extensions.YourNamespace.RegisterPolymorphicMappers();
```

### 3. Configure JSON Serialization

Use the `PolymorphicSerializationResolver` with System.Text.Json:

```csharp
using System.Text.Json;
using Hexalith.PolymorphicSerializations;

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    TypeInfoResolver = new PolymorphicSerializationResolver()
};

// Serialize polymorphic objects
string json = JsonSerializer.Serialize<MyBaseRecord>(derivedInstance, options);

// Deserialize to the correct type
MyBaseRecord result = JsonSerializer.Deserialize<MyBaseRecord>(json, options);
```

Or use the pre-configured options:

```csharp
using Hexalith.PolymorphicSerializations;

// Get default options with PolymorphicSerializationResolver configured
var options = PolymorphicHelper.DefaultJsonSerializerOptions;
```

## Generated Code

For each class/record marked with `PolymorphicSerializationAttribute`, the generator produces:

1. A mapper class that implements `IPolymorphicSerializationMapper`
2. Extension methods for registering all mappers with dependency injection
3. Static methods for registering with the default `PolymorphicSerializationResolver`

## Attribute Parameters

The `PolymorphicSerializationAttribute` accepts the following parameters:

- `name` (optional): Custom type discriminator name (defaults to class name)
- `version` (optional): Version number (defaults to 1)
- `baseType` (optional): Explicit base type specification

## Type Discriminator Format

Type discriminators are generated as follows:

- For version 1: `{name}`
- For version 2+: `{name}V{version}`

## Inheritance Requirements

Classes/records marked with `PolymorphicSerializationAttribute` must either:

- Inherit from `PolymorphicRecordBase`
- Inherit from a class/record that is also marked with `PolymorphicSerializationAttribute`

The source generator emits compilation errors when these requirements are not met.

## JSON Format

Serialized objects include a type discriminator property (`$type`) that is used during deserialization:

```json
{
  "$type": "FirstDerivedType",
  "Id": "123",
  "Name": "Example",
  "Value": 42
}
```

## License

Licensed under the MIT License. See the LICENSE file in the project root for details.
