﻿/*------------------------------------------------------------------------------
Wintellect.Analyzers - .NET Compiler Platform ("Roslyn") Analyzers and CodeFixes
Copyright (c) Wintellect. All rights reserved
Licensed under the MIT license
------------------------------------------------------------------------------*/
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

namespace Wintellect.Analyzers.Test
{
    [TestClass]
    public class IfAndElseMustHaveBracesUnitTests : CodeFixVerifier
    {
        static String ifMissingBraces = @"
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SomeTests
{
    public class BasicClass
    {
        public void SomeWork()
        {
            if (DateTime.Now > new DateTime(1))
                Console.WriteLine(""It's time"");
        }
    }
}
";

        static String ifFixedBraces = @"
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SomeTests
{
    public class BasicClass
    {
        public void SomeWork()
        {
            if (DateTime.Now > new DateTime(1))
            {
                Console.WriteLine(""It's time"");
            }
        }
    }
}
";

        static String ElseMissingBraces = @"
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SomeTests
{
    public class BasicClass
    {
        public void SomeWork()
        {
            if (DateTime.Now > new DateTime(1))
            {
                Console.WriteLine(""It's time"");
            }
            else Console.WriteLine(""Not time yet!"");
        }
    }
}
";

        static String ElseFixedBraces = @"
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace SomeTests
{
    public class BasicClass
    {
        public void SomeWork()
        {
            if (DateTime.Now > new DateTime(1))
            {
                Console.WriteLine(""It's time"");
            }
            else
            {
                Console.WriteLine(""Not time yet!"");
            }
        }
    }
}
";

        [TestMethod]
        [TestCategory("IfAndElseMustHaveBracesUnitTests")]
        public void TestMissingIfBraces()
        {
            var expected = new DiagnosticResult
            {
                Id = IfAndElseMustHaveBracesAnalyzer.DiagnosticId,
                Message = String.Format(IfAndElseMustHaveBracesAnalyzer.MessageFormat, "if"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 14, 13)
                        }
            };

            VerifyCSharpDiagnostic(ifMissingBraces, expected);
        }

        [TestMethod]
        [TestCategory("IfAndElseMustHaveBracesUnitTests")]
        public void TestMissingElseBraces()
        {
            var expected = new DiagnosticResult
            {
                Id = IfAndElseMustHaveBracesAnalyzer.DiagnosticId,
                Message = String.Format(IfAndElseMustHaveBracesAnalyzer.MessageFormat, "else"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 18, 13)
                        }
            };

            VerifyCSharpDiagnostic(ElseMissingBraces, expected);
        }

        [TestMethod]
        [TestCategory("IfAndElseMustHaveBracesUnitTests")]
        public void TestFixingIfBraces()
        {
            VerifyCSharpFix(ifMissingBraces, ifFixedBraces);
        }

        [TestMethod]
        [TestCategory("IfAndElseMustHaveBracesUnitTests")]
        public void TestFixingElseBraces()
        {
            VerifyCSharpFix(ElseMissingBraces, ElseFixedBraces);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new IfAndElseMustHaveBracesCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new IfAndElseMustHaveBracesAnalyzer();
        }

    }
}
