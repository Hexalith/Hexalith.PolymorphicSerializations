// <copyright file="Say.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DeserializeFileMessages.Messages;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// SayBye message.
/// </summary>
[PolymorphicSerialization]
public abstract partial record Say(string To);