# Hexalith.PolymorphicSerializations

A library for handling polymorphic serialization and deserialization in .NET applications.

## Overview

This library provides tools to serialize and deserialize polymorphic types, allowing for flexible object hierarchies to be maintained across serialization boundaries. It's particularly useful in systems that need to work with derived types and maintain their specific implementations when converting to and from serialized formats.

## Classes

- **AbstractClassConverter<T>**: A JSON converter that handles serialization and deserialization of abstract classes, preserving their concrete implementations.

- **InterfaceConverter<T>**: A JSON converter specifically designed for interfaces, allowing implementations to be properly serialized and reconstituted.

- **PolymorphicConverter<T>**: Base converter class that provides core functionality for polymorphic type conversion.

- **PolymorphicJsonSerializerOptionsBuilder**: Utility to build JsonSerializerOptions preconfigured with polymorphic serialization support.

- **PolymorphicSerializationOptions**: Configuration options for controlling the behavior of polymorphic serialization.

- **PolymorphicTypeDiscriminatorNamingPolicy**: Defines naming policies for type discriminators used in polymorphic serialization.

- **TypeDiscriminatorAttribute**: An attribute to mark and configure type discriminators for classes participating in polymorphic serialization.

- **TypeDiscriminatorConverter**: Handles the conversion of type discriminators between their serialized and runtime representations.

- **TypeDiscriminatorNameCache**: Provides caching for type discriminator names to improve performance.

## Features

- Type-preserving serialization and deserialization
- Support for abstract classes and interfaces
- Customizable type discriminator naming
- Performance-optimized through caching
- Compatible with System.Text.Json

This library targets .NET Standard 2.0 and .NET 9, ensuring broad compatibility across .NET implementations.
