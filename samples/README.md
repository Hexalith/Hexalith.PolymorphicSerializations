# Sample Applications for Hexalith.PolymorphicSerializations

This directory contains sample applications demonstrating how to use the Hexalith.PolymorphicSerializations package in various scenarios. These samples serve as practical examples and reference implementations for developers who want to integrate polymorphic serialization into their projects.

## Available Samples

### DeserializeFileMessages

A sample application demonstrating how to deserialize polymorphic messages from files. This sample shows:

- Configuration of polymorphic serialization
- Reading and deserializing different message types from JSON files
- Type discrimination using various strategies
- Error handling during deserialization
- Best practices for polymorphic type resolution

[Browse the DeserializeFileMessages sample](./DeserializeFileMessages)

## Running the Samples

Each sample can be run independently. Navigate to the sample directory and follow the instructions in the sample-specific README.md file for detailed setup and execution instructions.

## Common Patterns

All samples demonstrate these common patterns for working with Hexalith.PolymorphicSerializations:

1. **Type Registration**: How to register polymorphic types for serialization/deserialization
2. **Type Discrimination**: Techniques for discriminating between different types during deserialization
3. **Custom Converters**: Implementation of custom converters for special serialization needs
4. **Configuration Options**: Different configuration options for serialization behaviors

## Contributing New Samples

If you'd like to contribute a new sample:

1. Create a new directory with a descriptive name
2. Include a README.md with clear instructions
3. Implement a minimal but complete example
4. Ensure your sample follows best practices for the Hexalith ecosystem
5. Submit a pull request for review

## Feedback

We welcome feedback on these samples! If you have suggestions for improvements or encounter any issues, please open an issue in the main repository. 