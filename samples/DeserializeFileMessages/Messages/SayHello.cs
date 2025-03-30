// <copyright file="SayHello.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace DeserializeFileMessages.Messages;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// SayHello message.
/// </summary>
[PolymorphicSerialization(baseType: typeof(Say))]
public partial record SayHello(string To) : Say(To);