// <copyright file="PolymorphicSerializationAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations;

using System;

/// <summary>
/// Represents a custom attribute used to provide metadata for a message.
/// Initializes a new instance of the <see cref="PolymorphicSerializationAttribute"/> class.
/// </summary>
/// <param name="name">The name of the message. If null, the class name is used.</param>
/// <param name="version">The version of the message. Defaults to 1.</param>
/// <param name="baseType">The base type for polymorphism. If null, the direct base class is used.</param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PolymorphicSerializationAttribute(string? name = null, int version = 1, Type? baseType = null) : Attribute
{
    /// <summary>
    /// Gets the base type.
    /// </summary>
    /// <value>The base type.</value>
    public Type? BaseType { get; } = baseType;

    /// <summary>
    /// Gets the name of the message.
    /// </summary>
    /// <value>The name of the message.</value>
    public string? Name { get; } = name;

    /// <summary>
    /// Gets the version of the message.
    /// </summary>
    /// <value>The version of the message.</value>
    public int Version { get; } = version;

    /// <summary>
    /// Gets the type name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="version">The version.</param>
    /// <returns>The type name.</returns>
    public static string GetTypeName(string name, int version)
            => version < 2 ? name : $"{name}V{version}";

    /// <summary>
    /// Gets the polymorphic type name.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>The polymorphic type name.</returns>
    /// <exception cref="ArgumentNullException">The type is null.</exception>
    /// <exception cref="InvalidOperationException">The type name is null.</exception>
    public string GetTypeName(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return GetTypeName(
                (string.IsNullOrWhiteSpace(Name) ? type.Name : Name) ?? throw new InvalidOperationException("The type name is null."),
                Version);
    }
}