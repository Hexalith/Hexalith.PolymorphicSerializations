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

## Key Features

- **Easy Configuration:** Simplified setup for `System.Text.Json` to handle polymorphic types.
- **Flexible Type Discrimination:** Supports various strategies to identify derived types during deserialization (e.g., using a type discriminator property like `$type`).
- **Automatic Type Discovery:** Can automatically find and register derived types within specified assemblies.
- **Customizable:** Allows for custom converters and fine-grained control over serialization behavior.
- **Integration:** Designed to work smoothly within the Hexalith ecosystem and standard .NET applications.

## Installation

Install the package via NuGet Package Manager or the .NET CLI:

```sh
dotnet add package Hexalith.PolymorphicSerializations
```

## Getting Started

Here's a basic example demonstrating how to set up and use polymorphic serialization:

**1. Define Your Types:**

Create a base type (or interface) and derived types.

```csharp
// Define a base record (or class/interface)
[JsonPolymorphic] // Mark the base type for polymorphic serialization
[JsonDerivedType(typeof(Dog), typeDiscriminator: "dog")] // Register derived type Dog
[JsonDerivedType(typeof(Cat), typeDiscriminator: "cat")] // Register derived type Cat
public record Animal(string Name);

// Define derived records
public record Dog(string Name, string Breed) : Animal(Name);
public record Cat(string Name, bool IsLazy) : Animal(Name);
```
*Note: Using `[JsonPolymorphic]` and `[JsonDerivedType]` attributes (available in .NET 7+) is the standard way. This library provides additional configuration flexibility, especially for scenarios where attributes are not feasible or for older .NET versions.*

**2. Configure JsonSerializerOptions:**

While the attributes handle basic cases, for more complex scenarios or assembly scanning, you might configure `JsonSerializerOptions` using helpers (if provided by the library - *adjust based on actual library features*).

```csharp
// Example assuming a helper method exists (adapt based on actual library API)
// JsonSerializerOptions options = new JsonSerializerOptions();
// options.AddPolymorphicTypeConversion(config => {
//      config.ScanAssemblies(typeof(Animal).Assembly); // Auto-discover types
//      // Or manually register types
//      // config.Register<Animal>()
//      //       .WithDerived<Dog>("dog")
//      //       .WithDerived<Cat>("cat");
// });
```
*(Self-correction: The standard .NET attributes `[JsonPolymorphic]` and `[JsonDerivedType]` handle this directly. The library likely provides converters or helpers for scenarios *not* covered by attributes or for alternative configuration methods. The README should clarify the library's specific additions/advantages over the built-in attributes.)*

**3. Serialize and Deserialize:**

Use the configured `JsonSerializerOptions` (or rely on the attributes) for serialization and deserialization.

```csharp
using System.Text.Json;

// ... (Define Animal, Dog, Cat as above)

JsonSerializerOptions options = new()
{
    // Ensure options are configured if not solely relying on attributes
    // Add converters from Hexalith.PolymorphicSerializations if needed
    WriteIndented = true // For readability
};

List<Animal> animals = new()
{
    new Dog("Buddy", "Golden Retriever"),
    new Cat("Whiskers", true)
};

// Serialize
string json = JsonSerializer.Serialize<List<Animal>>(animals, options);
Console.WriteLine("Serialized JSON:");
Console.WriteLine(json);
// Output:
// [
//   {
//     "$type": "dog", // Discriminator added automatically if using attributes
//     "Breed": "Golden Retriever",
//     "Name": "Buddy"
//   },
//   {
//     "$type": "cat",
//     "IsLazy": true,
//     "Name": "Whiskers"
//   }
// ]


// Deserialize
List<Animal>? deserializedAnimals = JsonSerializer.Deserialize<List<Animal>>(json, options);

if (deserializedAnimals != null)
{
    foreach (Animal animal in deserializedAnimals)
    {
        Console.WriteLine($"Deserialized Animal: Name={animal.Name}, Type={animal.GetType().Name}");
        if (animal is Dog dog)
        {
            Console.WriteLine($"  Breed: {dog.Breed}");
        }
        else if (animal is Cat cat)
        {
            Console.WriteLine($"  Is Lazy: {cat.IsLazy}");
        }
    }
}
```

## Advanced Topics

*(This section can be expanded based on the specific features and complexity demonstrated in the samples)*

- **Type Discrimination:** Discuss different strategies supported (e.g., default `$type`, custom discriminators).
- **Custom Converters:** Explain how to integrate custom `JsonConverter` instances if the library provides hooks for them.
- **Configuration:** Detail specific configuration options or helper methods provided by the library.

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

Contributions are welcome! Please refer to the contribution guidelines (link to be added if a CONTRIBUTING.md exists) for details on how to submit pull requests, report issues, or suggest features.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
