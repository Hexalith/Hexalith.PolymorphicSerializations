// <copyright file="SayByeVersion2.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using Hexalith.PolymorphicSerializations;

namespace DeserializeFileMessages.Messages;

/// <summary>
/// SayBye message.
/// </summary>
/// <param name="To">The recipient of the message.</param>
[PolymorphicSerialization(baseType: typeof(Say), version: 2, name: "Bye")]
public partial record SayByeVersion2(string To) : Say(To);
