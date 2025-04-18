// <copyright file="IPolymorphicSerializationMapper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations;

using System;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Interface for serialization mapper.
/// </summary>
public interface IPolymorphicSerializationMapper
{
    /// <summary>
    /// Gets the base type.
    /// </summary>
    /// <value>The base type.</value>
    Type Base { get; }

    /// <summary>
    /// Gets the Json derived type.
    /// </summary>
    /// <value>The Json derived type.</value>
    JsonDerivedType JsonDerivedType { get; }
}