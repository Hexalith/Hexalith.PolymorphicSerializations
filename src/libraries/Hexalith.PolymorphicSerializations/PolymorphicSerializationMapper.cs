// <copyright file="PolymorphicSerializationMapper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations;

using System;
using System.Text.Json.Serialization.Metadata;

/// <summary>
/// Represents a serialization mapper used to map a type to its serialization information.
/// Initializes a new instance of the <see cref="PolymorphicSerializationMapper{TType, TBase}"/> class.
/// </summary>
/// <typeparam name="TType">The type to map.</typeparam>
/// <typeparam name="TBase">The base type of the type to map.</typeparam>
/// <param name="TypeDiscriminator">The type discriminator string.</param>
public record PolymorphicSerializationMapper<TType, TBase>(string TypeDiscriminator)
    : IPolymorphicSerializationMapper
    where TType : TBase
{
    /// <inheritdoc/>
    public Type Base => typeof(TBase);

    /// <inheritdoc/>
    public JsonDerivedType JsonDerivedType => new(typeof(TType), TypeDiscriminator);
}