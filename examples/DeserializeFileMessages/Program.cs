﻿// <copyright file="Program.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Text.Json;

using DeserializeFileMessages.Extensions;
using DeserializeFileMessages.Messages;

using Hexalith.PolymorphicSerializations;

DeserializeFileMessagesSerialization.RegisterPolymorphicMappers();

List<object> list =
[
    new SayHello("World"),
    new SayByeVersion2("World"),
    new Move("A", "B"),
];

// Serialize the list to a file
await File.WriteAllTextAsync("messages.json", JsonSerializer.Serialize(list, PolymorphicHelper.DefaultJsonSerializerOptions));

// Read the list from the file
List<Polymorphic> objects = JsonSerializer.Deserialize<List<Polymorphic>>(
    await File.ReadAllTextAsync("messages.json"),
    PolymorphicHelper.DefaultJsonSerializerOptions)
    ?? [];

foreach (Polymorphic item in objects)
{
    // Print the message
    Console.WriteLine(JsonSerializer.Serialize<Polymorphic>(item, PolymorphicHelper.DefaultJsonSerializerOptions));
}