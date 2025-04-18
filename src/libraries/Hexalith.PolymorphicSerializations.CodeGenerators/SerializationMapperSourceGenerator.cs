﻿// <copyright file="SerializationMapperSourceGenerator.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.PolymorphicSerializations.CodeGenerators;

using System.Collections.Immutable;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

/// <summary>
/// The source generator that generates the source code for the DI registration of the serialization mappers.
/// </summary>
[Generator(LanguageNames.CSharp)]
public class SerializationMapperSourceGenerator : IIncrementalGenerator
{
    private const string _serializationMapperAttributeFullName = "Hexalith.PolymorphicSerializations.PolymorphicSerializationAttribute";

    private const string _serializationMapperAttributeName = "PolymorphicSerializationAttribute";

    /// <inheritdoc/>
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<(TypeDeclarationSyntax?, AttributeData?)> classOrRecordDeclarations = context
            .SyntaxProvider
            .ForAttributeWithMetadataName(
                _serializationMapperAttributeFullName,
                predicate: static (node, _) => node is ClassDeclarationSyntax or RecordDeclarationSyntax,
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx))
            .Where(static m => m.Type is not null && m.Data is not null);

        IncrementalValueProvider<(Compilation, ImmutableArray<(TypeDeclarationSyntax?, AttributeData?)>)>
            compilationAndClasses
                = context.CompilationProvider.Combine(classOrRecordDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses, static (spc, source)
            => Execute(source.Item1, source.Item2, spc));
    }

    /// <summary>
    /// Executes the source generation process.
    /// </summary>
    /// <param name="compilation">The compilation context.</param>
    /// <param name="classesOrRecords">The collection of class/record declarations and their attribute data.</param>
    /// <param name="context">The source production context.</param>
    private static void Execute(
        Compilation compilation,
        ImmutableArray<(TypeDeclarationSyntax? Type, AttributeData? Data)> classesOrRecords,
        SourceProductionContext context)
    {
        IEnumerable<(TypeDeclarationSyntax, AttributeData)> types = classesOrRecords
            .Where(p => p.Type != null && p.Data != null)
            .Select(p => (p.Type!, p.Data!)!);
        List<INamedTypeSymbol> symbols = [];
        foreach ((TypeDeclarationSyntax Type, AttributeData Data) classOrRecord in types)
        {
            SemanticModel semanticModel = compilation.GetSemanticModel(classOrRecord.Type.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(classOrRecord.Type) is not INamedTypeSymbol symbol)
            {
                continue;
            }

            symbols.Add(symbol);
            string? namespaceName = symbol.ContainingNamespace.ToDisplayString();
            string domainName = classOrRecord.Type.Identifier.Text;

            context.AddSource(
                $"{domainName}Mapper.g.cs",
                SourceText.From(
                    GenerateMapperClass(classOrRecord, symbol, namespaceName, context),
                    Encoding.UTF8));
        }

        string code = GenerateAddServicesExtension(symbols);

        if (!string.IsNullOrEmpty(code))
        {
            context.AddSource("SerializationMapperExtension.g.cs", SourceText.From(code, Encoding.UTF8));
        }
    }

    /// <summary>
    /// Generates the source code for the dependency injection extension method.
    /// </summary>
    /// <param name="symbols">The list of named type symbols to include.</param>
    /// <returns>The generated C# code as a string, or an empty string if no symbols are provided.</returns>
    private static string GenerateAddServicesExtension(List<INamedTypeSymbol> symbols)
    {
        if (symbols.Count == 0)
        {
            return string.Empty;
        }

        string namespaceName = symbols[0].ContainingAssembly.MetadataName;
        string usings = symbols
            .Select(s => s.ContainingNamespace.ToDisplayString())
            .Distinct()
            .Select(n => $"using {n};")
            .Aggregate((a, b) => $"{a}\n{b}");
        string addSingletonMappers = symbols
            .Where(s => !s.IsAbstract)
            .Select(s =>
                $"        _ = services.AddSingleton<IPolymorphicSerializationMapper, {s.MetadataName}Mapper>();")
            .Aggregate((a, b) => $"{a}\n{b}");
        string addResolverMappers = symbols
            .Where(s => !s.IsAbstract)
            .Select(s => $"        PolymorphicSerializationResolver.TryAddDefaultMapper(new {s.MetadataName}Mapper());")
            .Aggregate((a, b) => $"{a}\n{b}");
        string project = namespaceName.Replace(".", string.Empty);
        return $$"""
                         // <auto-generated/>
                         
                         namespace {{namespaceName}}.Extensions;

                         {{usings}}
                         using Microsoft.Extensions.DependencyInjection;
                         using Microsoft.Extensions.DependencyInjection.Extensions;
                         using System;
                         using Hexalith.PolymorphicSerializations;

                         /// <summary>
                         /// Extension methods for registering polymorphic serialization mappers.
                         /// </summary>
                         public static class {{project}}Serialization
                         {
                             /// <summary>
                             /// Adds the polymorphic mappers to the service collection.
                             /// </summary>
                             /// <param name="services">The service collection to add the mappers to.</param>
                             /// <returns>The service collection for chaining.</returns>
                             /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
                             public static IServiceCollection Add{{project}}PolymorphicMappers(this IServiceCollection services)
                             {
                                 services.TryAddSingleton<PolymorphicSerializationResolver>();
                         {{addSingletonMappers}}
                                 return services;
                             }

                             /// <summary>
                             /// Registers the polymorphic mappers with the default resolver.
                             /// </summary>
                             public static void RegisterPolymorphicMappers()
                             {
                         {{addResolverMappers}}
                             }
                         }
                         """;
    }

    /// <summary>
    /// Generates the source code for a specific mapper class.
    /// </summary>
    /// <param name="syntax">The syntax and attribute data for the class/record.</param>
    /// <param name="classSymbol">The named type symbol for the class/record.</param>
    /// <param name="namespaceName">The namespace of the class/record.</param>
    /// <param name="context">The source production context.</param>
    /// <returns>The generated C# code for the mapper class as a string.</returns>
    private static string GenerateMapperClass(
        (TypeDeclarationSyntax Type, AttributeData Data) syntax,
        INamedTypeSymbol classSymbol,
        string? namespaceName,
        SourceProductionContext context)
    {
        INamedTypeSymbol? baseTypeSymbol = classSymbol.BaseType;

        bool hasParent = baseTypeSymbol != null && baseTypeSymbol.SpecialType != SpecialType.System_Object;
        bool validBaseType = false;

        while (baseTypeSymbol != null && baseTypeSymbol.SpecialType != SpecialType.System_Object)
        {
            if (baseTypeSymbol.Name is "Polymorphic")
            {
                validBaseType = true;
                break;
            }

            // Check if the base type has the PolymorphicSerialization attribute
            if (baseTypeSymbol
                .GetAttributes()
                .Any(a =>
                    a.AttributeClass?.ToDisplayString() == _serializationMapperAttributeFullName))
            {
                validBaseType = true;
                break;
            }

            baseTypeSymbol = baseTypeSymbol.BaseType;
        }

        if (hasParent && !validBaseType)
        {
            // Emit an error
            context.ReportDiagnostic(Diagnostic.Create(
                new DiagnosticDescriptor(
                    "HM001",
                    "Invalid Inheritance",
                    "Record {0} has a parent but does not inherit from Polymorphic .",
                    "CodeGeneration",
                    DiagnosticSeverity.Error,
                    true),
                syntax.Type.GetLocation(),
                syntax.Type.Identifier.Text));
        }

        // get the name and the version from the attribute
        TypedConstant nameParam = syntax.Data.ConstructorArguments.Length > 0
            ? syntax.Data.ConstructorArguments[0]
            : default;
        TypedConstant versionParam = syntax.Data.ConstructorArguments.Length > 1
            ? syntax.Data.ConstructorArguments[1]
            : default;
        string name = nameParam.Value == null || string.IsNullOrWhiteSpace((string?)nameParam.Value)
            ? classSymbol.MetadataName
            : (string)nameParam.Value;

        int version = versionParam.Value == null ? 1 : (int)versionParam.Value;
        string typeDiscriminator = GetTypeName(name, version);
        string inheritance = hasParent ? string.Empty : " : Polymorphic";

        return $$"""
                 // <auto-generated/>
                 
                 namespace {{namespaceName}};

                 using Microsoft.Extensions.DependencyInjection;
                 using Hexalith.PolymorphicSerializations;
                 using System.Runtime.Serialization;

                 /// <summary>
                 /// Represents a polymorphic serializable record.
                 /// </summary>
                 [DataContract]
                 public partial record {{classSymbol.MetadataName}}{{inheritance}} {}

                 /// <summary>
                 /// Polymorphic serialization mapper for <see cref="{{classSymbol.MetadataName}}"/>.
                 /// </summary>
                 /// <remarks>
                 /// Maps between <see cref="{{classSymbol.MetadataName}}"/> and <see cref="Polymorphic"/>.
                 /// Using type discriminator "{{typeDiscriminator}}".
                 /// </remarks>
                 public sealed record {{classSymbol.MetadataName}}Mapper() : PolymorphicSerializationMapper<{{classSymbol.MetadataName}},Polymorphic>("{{typeDiscriminator}}")
                 {
                 }
                 """;
    }

    /// <summary>
    /// Gets the semantic target (class/record declaration and attribute data) for generation.
    /// </summary>
    /// <param name="context">The generator attribute syntax context.</param>
    /// <returns>A tuple containing the type declaration syntax and attribute data, or (null, null) if not applicable.</returns>
    private static (TypeDeclarationSyntax? Type, AttributeData? Data) GetSemanticTargetForGeneration(
        GeneratorAttributeSyntaxContext context)
    {
        if (context.TargetNode is not TypeDeclarationSyntax classDeclaration)
        {
            return (null, null);
        }

        AttributeData? attribute = context.Attributes
            .FirstOrDefault(a => a.AttributeClass?.Name == _serializationMapperAttributeName);

        return (classDeclaration, attribute);
    }

    /// <summary>
    /// Gets the type name based on name and version.
    /// </summary>
    /// <param name="name">The base name.</param>
    /// <param name="version">The version number.</param>
    /// <returns>The formatted type name (e.g., "MyType" or "MyTypeV2").</returns>
    private static string GetTypeName(string name, int version)
            => version < 2 ? name : $"{name}V{version}";
}