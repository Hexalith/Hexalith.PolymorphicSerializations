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

Install the package via NuGet Package Manager or the .NET CLI:

```sh
dotnet add package Hexalith.PolymorphicSerializations
```

## Quick Start

### 1. Define your class hierarchy

```csharp
// Base class or interface
public abstract class Animal
{
    public string Name { get; set; }
}

// Derived classes
public class Dog : Animal
{
    public string Breed { get; set; }
    public bool IsGoodBoy { get; set; } = true;
}

public class Cat : Animal
{
    public bool LikesCatnip { get; set; }
    public int LivesRemaining { get; set; } = 9;
}
```

### 2. Configure serialization with attributes

```csharp
// Add discriminator attribute to the base class
[PolymorphicType]
public abstract class Animal
{
    public string Name { get; set; }
    
    // Optional: Override the discriminator property name (default is "$type")
    [PolymorphicTypeDiscriminator]
    public string Kind { get; set; }
}

// Add type information to derived classes
[PolymorphicTypeValue("dog")]
public class Dog : Animal
{
    public string Breed { get; set; }
    public bool IsGoodBoy { get; set; } = true;
}

[PolymorphicTypeValue("cat")]
public class Cat : Animal
{
    public bool LikesCatnip { get; set; }
    public int LivesRemaining { get; set; } = 9;
}
```

### 3. Register the converter with System.Text.Json

```csharp
using System.Text.Json;
using Hexalith.PolymorphicSerializations.Json;

// Configure JsonSerializerOptions
var options = new JsonSerializerOptions
{
    WriteIndented = true
};

// Register the polymorphic converter
options.Converters.Add(new PolymorphicJsonConverter());

// Now you can serialize and deserialize
var dog = new Dog { Name = "Rex", Breed = "German Shepherd" };
string json = JsonSerializer.Serialize<Animal>(dog, options);
// Result: {"Name":"Rex","Kind":"dog","Breed":"German Shepherd","IsGoodBoy":true}

Animal deserializedAnimal = JsonSerializer.Deserialize<Animal>(json, options);
// deserializedAnimal will be a Dog instance with proper properties
```

## Advanced Usage

### Using with Dependency Injection

```csharp
// In your startup.cs or program.cs
services.AddPolymorphicJsonConverters();

// Then in your services:
public class MyService
{
    private readonly JsonSerializerOptions _options;
    
    public MyService(IOptions<JsonSerializerOptions> options)
    {
        _options = options.Value;
    }
    
    public string SerializeObject<T>(T obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }
}
```

### Custom Type Resolution

```csharp
// Creating a custom type resolver
public class CustomAnimalTypeResolver : IPolymorphicTypeResolver
{
    public Type ResolveType(string discriminator)
    {
        return discriminator switch
        {
            "canine" => typeof(Dog),
            "feline" => typeof(Cat),
            _ => throw new JsonException($"Unknown animal type: {discriminator}")
        };
    }
    
    public string ResolveDiscriminator(Type type)
    {
        if (type == typeof(Dog)) return "canine";
        if (type == typeof(Cat)) return "feline";
        throw new JsonException($"Unknown animal type: {type.Name}");
    }
}

// Register your custom resolver
var options = new JsonSerializerOptions();
options.Converters.Add(new PolymorphicJsonConverter
{
    TypeResolvers = { new CustomAnimalTypeResolver() }
});
```

## Sample Application

For more detailed usage examples and demonstrations of various features, explore the sample applications:

- **[Sample Application README](./samples/README.md)**
  - Includes examples like deserializing polymorphic messages from files (`DeserializeFileMessages`).

## Repository Structure

```
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
- **Missing type discriminator**: Check that your base class has the [PolymorphicType] attribute.
- **Serialization exceptions**: Verify that all derived types have the [PolymorphicTypeValue] attribute with unique discriminator values.

### Debugging Tips

Enable detailed logging for System.Text.Json to troubleshoot serialization issues:

```csharp
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
};
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
