// <copyright file="AttributeTestBase.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.Tests.Objects;

/// <summary>
/// Base class for polymorphic serialization testing.
/// </summary>
/// <param name="Id">The unique identifier.</param>
[PolymorphicSerialization]
public abstract partial record AttributeTestBase(string Id);