# Polymorphic Serialization File Messages Sample

This sample console application demonstrates how to use the Hexalith.PolymorphicSerializations library to serialize and deserialize polymorphic objects to and from a JSON file.

## Overview

The sample shows how to:

1. Define a set of polymorphic message types using C# records
2. Apply the `PolymorphicSerialization` attribute to enable type discrimination
3. Serialize a list of different message types to a JSON file
4. Deserialize the file back into the correct message types
5. Use source-generated serialization mappers

## Project Structure

- **Program.cs**: Main application logic for serializing and deserializing messages
- **Messages/**: Contains the message type definitions
  - `Say.cs`: Abstract base record for say messages
  - `SayHello.cs`: Concrete implementation of a hello message
  - `SayByeVersion2.cs`: Version 2 implementation of a goodbye message
  - `Move.cs`: Another message type demonstrating a different inheritance hierarchy

## Message Type Hierarchy

The sample demonstrates two message hierarchies:

1. `Say` base class with derived types:
   - `SayHello`: A simple hello message
   - `SayByeVersion2`: A versioned goodbye message 

2. `Move`: A standalone message type directly inheriting from `Polymorphic`

## Key Features Demonstrated

### 1. Primary Constructor Records

All message types use C# records with primary constructors for concise property definition:

```csharp
public abstract partial record Say(string To);
public partial record SayHello(string To) : Say(To);
```

### 2. Polymorphic Type Discrimination

The `PolymorphicSerialization` attribute is used to specify type discriminators:

```csharp
[PolymorphicSerialization]
public abstract partial record Say(string To);

[PolymorphicSerialization(baseType: typeof(Say))]
public partial record SayHello(string To) : Say(To);

[PolymorphicSerialization(baseType: typeof(Say), version: 2, name: "Bye")]
public partial record SayByeVersion2(string To) : Say(To);
```

### 3. Source-Generated Serialization Mappers

The sample uses the `Hexalith.PolymorphicSerializations.CodeGenerators` package to automatically generate serialization mapper code at compile time. The source generator creates a registration method that is called at application startup:

```csharp
DeserializeFileMessagesSerialization.RegisterPolymorphicMappers();
```

### 4. Serializing and Deserializing Polymorphic Lists

The sample demonstrates how to serialize and deserialize a heterogeneous list of message types:

```csharp

DeserializeFileMessagesSerialization.RegisterPolymorphicMappers();

List<object> list =
[
    new SayHello("World"),
    new SayByeVersion2("World"),
    new Move("A", "B"),
];

// Serialize the list to a file
await File.WriteAllTextAsync("messages.json", JsonSerializer.Serialize(list, PolymorphicHelper.DefaultJsonSerializerOptions));

// Read the list from the file
List<Polymorphic> objects = JsonSerializer.Deserialize<List<Polymorphic>>(
    await File.ReadAllTextAsync("messages.json"),
    PolymorphicHelper.DefaultJsonSerializerOptions)
    ?? [];

foreach (Polymorphic item in objects)
{
    // Print the message
    Console.WriteLine(JsonSerializer.Serialize<Polymorphic>(item, PolymorphicHelper.DefaultJsonSerializerOptions));
}
```

## Running the Sample

1. Execute the application
2. The application will:
   - Create a list with three different message types
   - Serialize the list to a file named "messages.json"
   - Read the file and deserialize it back into the original message types
   - Print each message to the console, preserving its original type

## JSON Output

The serialized JSON will include type discriminators (`$type` property) to preserve type information:

```json
[
  {
    "$type": "SayHello",
    "To": "World"
  },
  {
    "$type": "ByeV2",
    "To": "World"
  },
  {
    "$type": "Move",
    "From": "A",
    "To": "B"
  }
]
```

Note: The `$type` discriminator is derived from the class name when no custom name is specified in the `[PolymorphicSerialization]` attribute. For versioned types (version > 1), the format is `{Name}V{Version}` (e.g., "ByeV2" for `name: "Bye", version: 2`).

## Dependencies

This sample references:
- `Hexalith.PolymorphicSerializations`: Core library for polymorphic serialization
- `Hexalith.PolymorphicSerializations.CodeGenerators`: Source generator for automatic serialization mapper creation

## Learn More

For more information about the libraries used in this sample, refer to:
- [Main Project README](../../README.md)
- [Examples Overview](../README.md)