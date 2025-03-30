// <copyright file="TestType1.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.Tests.Objects;
/// <summary>
/// Derived class for polymorphic serialization testing.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The name property specific to TestType1.</param>
/// <param name="Value">The value property specific to TestType1.</param>
public record TestType1(string Id, string Name, int Value) : PolymorphicRecordBase;