// <copyright file="PolymorphicSerializationMapper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Text.Json.Serialization.Metadata;

namespace Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a serialization mapper used to map a type to its serialization information.
/// Initializes a new instance of the <see cref="PolymorphicSerializationMapper{TType, TBase}"/> class.
/// </summary>
/// <typeparam name="TType">The type to map.</typeparam>
/// <typeparam name="TBase">The base type of the type to map.</typeparam>
public record PolymorphicSerializationMapper<TType, TBase> : IPolymorphicSerializationMapper
    where TType : TBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PolymorphicSerializationMapper{TType, TBase}"/> class.
    /// </summary>
    /// <param name="typeDiscriminator">The type discriminator string.</param>
    public PolymorphicSerializationMapper(string typeDiscriminator)
        => TypeDiscriminator = typeDiscriminator;

    /// <inheritdoc/>
    public Type Base => typeof(TBase);

    /// <inheritdoc/>
    public JsonDerivedType JsonDerivedType => new JsonDerivedType(typeof(TType), TypeDiscriminator);

    /// <summary>
    /// Gets the type discriminator string.
    /// </summary>
    public string TypeDiscriminator { get; init; }

    /// <summary>
    /// Deconstructs the mapper into its type discriminator.
    /// </summary>
    /// <param name="typeDiscriminator">The type discriminator string.</param>
    public void Deconstruct(out string typeDiscriminator)
        => typeDiscriminator = TypeDiscriminator;
}
