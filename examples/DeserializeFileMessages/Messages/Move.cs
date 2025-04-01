// <copyright file="Move.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DeserializeFileMessages.Messages;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Move message.
/// </summary>
/// <param name="From">From.</param>
/// <param name="To">To.</param>
[PolymorphicSerialization]
public partial record class Move(
    [property: DataMember(Order = 1)] string From,
    [property: DataMember(Order = 2)] string To);