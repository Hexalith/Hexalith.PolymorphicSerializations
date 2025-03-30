// <copyright file="AttributeTestType2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.Tests.Objects;
/// <summary>
/// Another derived class for polymorphic serialization testing.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Description">The description property specific to TestType2.</param>
/// <param name="Enabled">The enabled flag specific to TestType2.</param>
[PolymorphicSerialization(baseType: typeof(AttributeTestBase))]
public record AttributeTestType2(string Id, string Description, bool Enabled) : AttributeTestBase(Id);