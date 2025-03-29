# Hexalith.PolymorphicSerializations.Tests

This repository contains unit tests for the Hexalith.PolymorphicSerializations library, a component of the Hexalith platform.

## Overview

Hexalith.PolymorphicSerializations.Tests provides comprehensive test coverage for all functionality within the Hexalith.PolymorphicSerializations library, ensuring reliability and correctness of the implementation.

## Project Structure

The test project follows these conventions:
- Test files mirror the structure of the main project
- Each test class corresponds to a class in the main project
- Tests use the xUnit framework with Shouldly for assertions

## Getting Started

### Prerequisites

- .NET 9 SDK or later
- Any other dependencies will be automatically restored via NuGet

### Running Tests

To run the tests locally:

## Test Classes

### DummyClassTest

Tests for the `DummyClass` record, ensuring that:
- The `SampleValue` property maintains the value passed in the constructor

## Contributing

When adding new tests, please follow these guidelines:
1. Match the namespace structure of the main project
2. Include XML documentation for test classes and methods
3. Follow the Arrange-Act-Assert pattern in test methods
4. Use descriptive test names that indicate what is being tested

## License

This project is licensed under the MIT License - see the LICENSE file in the project root for details.

Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
