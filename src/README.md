# Hexalith.PolymorphicSerializations

This repository contains libraries for polymorphic serialization and deserialization in .NET using System.Text.Json.

## Overview

The Hexalith.PolymorphicSerializations project provides a comprehensive solution for handling polymorphic serialization scenarios in .NET applications. It enables type-based JSON serialization and deserialization by managing type discriminators and mapping between base types and their derived implementations.

## Project Structure

The solution consists of the following subprojects:

### [Hexalith.PolymorphicSerializations](./Hexalith.PolymorphicSerializations/README.md)

The core library that provides the fundamental components for polymorphic serialization:

- `PolymorphicRecordBase`: A marker base record for polymorphic hierarchies
- `PolymorphicSerializationAttribute`: Metadata annotation for polymorphic types
- `PolymorphicSerializationResolver`: JSON type info resolver for polymorphic handling
- `PolymorphicSerializationMapper`: Maps derived types to base types
- `PolymorphicHelper`: Utility methods for polymorphic serialization

### [Hexalith.PolymorphicSerializations.CodeGenerators](./Hexalith.PolymorphicSerializations.CodeGenerators/README.md)

A source generator package that automates mapper creation:

- Automatically generates serialization mapper classes
- Creates dependency injection registration code
- Provides compile-time validation
- Eliminates boilerplate code for polymorphic serialization

## Key Features

- **Type-Based Serialization**: Serialize and deserialize object hierarchies based on type discriminators
- **Versioning Support**: Handle different versions of the same type
- **Source Generation**: Automatically create mapper implementations at compile-time
- **Fluent API**: Configure serialization with a builder pattern
- **System.Text.Json Integration**: Works natively with the built-in JSON serialization system
- **Attribute-Based Configuration**: Use attributes to define serialization metadata

## Getting Started

To use these libraries in your project:

```bash
# Add the core library
dotnet add package Hexalith.PolymorphicSerializations

# Add the source generators (optional but recommended)
dotnet add package Hexalith.PolymorphicSerializations.CodeGenerators
```

## Basic Usage Example

```csharp
// Define a polymorphic type hierarchy
[PolymorphicSerialization]
public abstract record Shape(string Id);

[PolymorphicSerialization]
public record Circle(string Id, double Radius) : Shape(Id);

[PolymorphicSerialization]
public record Rectangle(string Id, double Width, double Height) : Shape(Id);

// Register mappers (automatically handled with CodeGenerators)
services.AddPolymorphicMappers();

// Use for serialization
JsonSerializerOptions options = PolymorphicHelper.DefaultJsonSerializerOptions;
string json = JsonSerializer.Serialize<Shape>(new Circle("1", 5.0), options);
Shape shape = JsonSerializer.Deserialize<Shape>(json, options);
```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Company

Developed and maintained by [ITANEO](https://www.itaneo.com)