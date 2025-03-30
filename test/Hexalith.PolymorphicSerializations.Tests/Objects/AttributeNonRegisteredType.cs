// <copyright file="AttributeNonRegisteredType.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.Tests.Objects;

/// <summary>
/// An unrelated class to test non-registered types.
/// </summary>
/// <param name="Code">The code value.</param>
/// <param name="Title">The title value.</param>
[PolymorphicSerialization]
public partial record AttributeNonRegisteredType(string Code, string Title);