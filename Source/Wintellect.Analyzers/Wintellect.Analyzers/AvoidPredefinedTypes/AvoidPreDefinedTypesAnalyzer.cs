﻿/*------------------------------------------------------------------------------
Wintellect.Analyzers - .NET Compiler Platform ("Roslyn") Analyzers and CodeFixes
Copyright (c) Wintellect. All rights reserved
Licensed under the MIT license
------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;


namespace Wintellect.Analyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AvoidPreDefinedTypesAnalyzer : DiagnosticAnalyzer
    {
        // The ID shown in the Error window
        public const String DiagnosticId = "Wintellect004";
        // TODO: Needs to be internationalized.
        public const String Title = "Use explicit types instead of predefined for better portability";
        // TODO: Needs to be internationalized.
        public const String MessageFormat = "Convert '{0}' to the explicit type '{1}'";
        // TODO: Needs to be internationalized.
        public const String Category = "Usage";

        internal static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId,
                                                                             Title,
                                                                             MessageFormat,
                                                                             Category,
                                                                             DiagnosticSeverity.Warning,
                                                                             true);

        public static readonly Dictionary<String, String> TypeMap = new Dictionary<String, String>
        {
            // This is the fancy, new C# 6.0 initialization.
            ["bool"] = "Boolean",
            ["byte"] = "Byte",
            ["char"] = "Char",
            ["decimal"] = "Decimal",
            ["double"] = "Double",
            ["float"] = "Single",
            ["int"] = "Int32",
            ["long"] = "Int64",
            ["object"]  = "Object",
            ["sbyte"] = "SByte",
            ["short"] = "Int16",
            ["string"]  = "String",
            ["ulong"] = "Uint64",
            ["ushort"]  = "UInt16",
        };

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            // I only need to look at predefined types.
            context.RegisterSyntaxNodeAction(AnalyzePredefinedType, SyntaxKind.PredefinedType);
        }

        private void AnalyzePredefinedType(SyntaxNodeAnalysisContext context)
        {
            PredefinedTypeSyntax predefinedType = context.Node as PredefinedTypeSyntax;

            // Don't touch the void. :)
            if (!predefinedType.ToString().Equals("void", StringComparison.OrdinalIgnoreCase))
            {
                String typeString = predefinedType.ToString();
                String realString = TypeMap[typeString];
                var diagnostic = Diagnostic.Create(Rule,
                                                   predefinedType.GetLocation(),
                                                   typeString,
                                                   realString);
                context.ReportDiagnostic(diagnostic);
            }
        }
    }
}