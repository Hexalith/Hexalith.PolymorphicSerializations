# Source Code

This directory contains the source code for the Hexalith.PolymorphicSerializations packages.

## Libraries

### Hexalith.PolymorphicSerializations

The core library providing polymorphic serialization support for System.Text.Json. Key components include:

- **PolymorphicSerializationAttribute**: Attribute to mark types for polymorphic serialization
- **Polymorphic**: Base record class that all polymorphic types inherit from
- **PolymorphicHelper**: Helper methods and default serializer options
- **PolymorphicSerializationResolver**: Custom JSON type resolver for polymorphic types
- **PolymorphicSerializationMapper**: Generic mapper for type discrimination

### Hexalith.PolymorphicSerializations.CodeGenerators

Source generator package that automatically generates serialization mappers at compile time. Features:

- Automatic mapper class generation for types decorated with `[PolymorphicSerialization]`
- Generation of registration extension methods for dependency injection
- Static registration method for non-DI scenarios

## Building

The projects in this directory are built as part of the main solution. See the [main README](../README.md) for build instructions.
