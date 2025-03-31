# Hexalith.PolymorphicSerializations

[![License: MIT](https://img.shields.io/github/license/hexalith/hexalith.PolymorphicSerializations)](https://github.com/hexalith/hexalith.polymorphicserializations/blob/main/LICENSE)
[![Discord](https://img.shields.io/discord/1063152441819942922?label=Discord&logo=discord&logoColor=white&color=d82679)](https://discordapp.com/channels/1102166958918610994/1102166958918610997)
[![Build status](https://github.com/Hexalith/Hexalith.PolymorphicSerializations/actions/workflows/build-release.yml/badge.svg)](https://github.com/Hexalith/Hexalith.PolymorphicSerializations/actions)
[![NuGet](https://img.shields.io/nuget/v/Hexalith.PolymorphicSerializations.svg)](https://www.nuget.org/packages/Hexalith.PolymorphicSerializations)
[![Latest](https://img.shields.io/github/v/release/Hexalith/Hexalith.PolymorphicSerializations?include_prereleases&label=preview)](https://github.com/Hexalith/Hexalith.PolymorphicSerializations/pkgs/nuget/Hexalith.PolymorphicSerializations)
[![Coverity Scan Build Status](https://scan.coverity.com/projects/31529/badge.svg)](https://scan.coverity.com/projects/hexalith-hexalith-PolymorphicSerializations)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d48f6d9ab9fb4776b6b4711fc556d1c4)](https://app.codacy.com/gh/Hexalith/Hexalith.PolymorphicSerializations/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.PolymorphicSerializations&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.PolymorphicSerializations)

## Overview

`Hexalith.PolymorphicSerializations` provides robust support for polymorphic serialization and deserialization in .NET applications, integrating seamlessly with `System.Text.Json`. It simplifies handling complex object hierarchies where instances of derived types need to be serialized and deserialized based on a common base type or interface.

This library is particularly useful when dealing with scenarios like:

- Event Sourcing (serializing different domain events)
- Message Queues (sending/receiving various message types)
- APIs returning different response types based on context
- Configuration systems loading diverse component types
- NoSQL Databases

## Features

- **Type Discrimination** - Automatically includes type information in serialized JSON to ensure proper deserialization.
- **Custom Type Resolution** - Flexible system for mapping between .NET types and JSON discriminator values.
- **Minimal Configuration** - Simple attribute-based setup with reasonable defaults.
- **High Performance** - Built on the high-performance `System.Text.Json` serialization engine.
- **Framework Agnostic** - Works with any .NET application using .NET Standard 2.0 or higher.
- **No External Dependencies** - Only depends on `System.Text.Json`.

## Requirements

- .NET 7.0 or higher
- System.Text.Json 7.0 or higher

## Installation

Install the packages via NuGet Package Manager or the .NET CLI:

```sh
dotnet add package Hexalith.PolymorphicSerializations
dotnet add package Hexalith.PolymorphicSerializations.CodeGenerators
```

## Quick Start

### 1. Configure serialization by adding attributes to your model

The library uses source generators to create serialization mappers at compile time. This requires:

1. Adding `partial` keyword to your classes/records
2. Applying `[PolymorphicSerialization]` attributes 
3. Installing the code generator package

```csharp
// Basic class with no inheritance
// The 'partial' keyword allows the source generator to extend this type
// The serialized JSON will include a discriminator with value "Car"
[PolymorphicSerialization]
public partial record Car(string Name, string EnergyType);

// Polymorphic inheritance example
// Base class must be marked as polymorphic
[PolymorphicSerialization]
public abstract partial record Animal(string Name);

// Derived class specifying its base type
// The serialized JSON will include a discriminator with value "Dog"
[PolymorphicSerialization(baseType: typeof(Animal))]
public partial record Dog(string Name, bool Dangerous) : Animal(Name);

// Another derived class
// The serialized JSON will include a discriminator with value "Cat"
[PolymorphicSerialization(baseType: typeof(Animal))]
public partial record Cat(string Name, string Color) : Animal(Name);

// Versioned class example
// The serialized JSON will include a discriminator with value "CatV2"
// This supports versioning of your data models
[PolymorphicSerialization("Cat", 2, typeof(Animal))]
public partial record NewCatVersion2(string Name, bool LikesCatnip) : Animal(Name);
```

### 2. Register the polymorphic mappers

The source generator creates a registration extension method in your project's namespace:

```csharp
using System.Text.Json;
using MyProject.Extensions; // Contains the generated extension method
using Hexalith.PolymorphicSerializations;

// Register all polymorphic mappers defined in your project
// This connects your model classes to the serialization system
MyProject.RegisterPolymorphicMappers();

// Serialize the Car object
string json = JsonSerializer.Serialize(new Car("Volvo", "Electric"), PolymorphicHelper.DefaultJsonSerializerOptions);

// Deserialize the Car object. You need to specify polymorphic deserialization by using the Polymorphic type.
var value = JsonSerializer.Deserialize<Polymorphic>(json, PolymorphicHelper.DefaultJsonSerializerOptions);
```

## Sample Application

For more detailed usage examples and demonstrations of various features, explore the sample applications:

- **[Sample Application README](./samples/README.md)**
  - Includes examples like deserializing polymorphic messages from files (`DeserializeFileMessages`).

## Repository Structure

```text
Hexalith.PolymorphicSerializations/
├── .github/             # GitHub workflows and configurations
├── Hexalith.Builds/     # Shared build configurations (submodule)
├── src/                 # Source code for the library
├── test/                # Unit and integration tests
├── samples/             # Sample applications demonstrating usage
├── .gitignore           # Git ignore file
├── .gitmodules          # Git submodules configuration
├── Directory.Build.props # MSBuild properties shared across projects
├── Directory.Packages.props # Central package management
├── Hexalith.PolymorphicSerializations.sln # Solution file
├── LICENSE              # MIT License
├── README.md            # This file
└── initialize.ps1       # Initialization script
```

## Contributing

Contributions are welcome! Here's how you can contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

Please ensure your code follows the project's coding standards and includes appropriate tests.

## Troubleshooting

### Common Issues

- **Type not recognized during deserialization**: Ensure you've applied the correct attributes and registered the converter with your JsonSerializerOptions.
- **Missing type discriminator**: Check that your base class has the [PolymorphicSerialization] attribute.
- **Serialization exceptions**: Verify that all derived types have the [PolymorphicSerialization] attribute with the correct baseType parameter.
- **Build errors**: Make sure all classes with polymorphic attributes are marked as `partial`.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
