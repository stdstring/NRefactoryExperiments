using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Cecil;
using NUnit.Framework;
using Attribute = ICSharpCode.NRefactory.CSharp.Attribute;

namespace StructureResearchTests
{
    [TestFixture]
    public class AttributesTests
    {
        [Test]
        public void CheckAttributes()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Property, AllowMultiple = true)]\r\n" +
                                  "    public class AttrA : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Method)]\r\n" +
                                  "    public class AttrB : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        [AttrA]\r\n" +
                                  "        [AttrB]\r\n" +
                                  "        public void M()\r\n" +
                                  "        { }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            MethodDeclaration method = syntaxTree.Descendants.OfType<MethodDeclaration>().First(m => m.Name == "M");
            AstNodeCollection<AttributeSection> sections = method.Attributes;
            Console.WriteLine("sections.Count = {0}", sections.Count);
            Console.WriteLine();
            foreach (AttributeSection section in sections)
            {
                Console.WriteLine("section.AttributeTarget = {0}", section.AttributeTarget);
                AstNodeCollection<Attribute> attributes = section.Attributes;
                Console.WriteLine("attributes.Count = {0}", attributes.Count);
                foreach (Attribute attribute in attributes)
                {
                    Console.WriteLine("((SimpleType) attribute.Type).Identifier = {0}", ((SimpleType) attribute.Type).Identifier);
                }
                Console.WriteLine();
            }
        }

        [Test]
        public void ResolveTypeWithUnknownAttributes()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public enum EE {v1 = 13, v2 = 666}\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Method | System.AttributeTargets.Property, AllowMultiple = true)]\r\n" +
                                  "    public class AttrA : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Method)]\r\n" +
                                  "    public class AttrB : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "        public AttrB(int i, string s, EE e) {}\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        [AttrA]\r\n" +
                                  "        [AttrB(666, \"iddqd\", EE.v1)]\r\n" +
                                  "        [AttrC]\r\n" +
                                  "        public void M()\r\n" +
                                  "        { }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            MethodDeclaration method = syntaxTree.Descendants.OfType<MethodDeclaration>().First(m => m.Name == "M");
            ResolveResult result = resolver.Resolve(method);
            MemberResolveResult memberResult = (MemberResolveResult) result;
            IMember member = memberResult.Member;
            foreach (IAttribute attribute in member.Attributes)
            {
                Console.WriteLine("attribute.AttributeType = {0}, attribute.AttributeType.Kind = {1}", attribute.AttributeType.FullName, attribute.AttributeType.Kind);
                Console.WriteLine("attribute.PositionalArguments.Count = {0}", attribute.PositionalArguments.Count);
                ProcessPositionalArgs(attribute.PositionalArguments);
                Console.WriteLine("attribute.NamedArguments.Count = {0}", attribute.NamedArguments.Count);
                Console.WriteLine();
            }
        }

        [Test]
        public void MethodParamsReturnAttributes()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.Parameter)]\r\n" +
                                  "    public class AttrA : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    [System.AttributeUsage(System.AttributeTargets.ReturnValue)]\r\n" +
                                  "    public class AttrB : System.Attribute\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        [return: AttrB]\r\n" +
                                  "        public string SomeMethod(int p1, [AttrA]string p2) { return \"iddqd\"; }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            MethodDeclaration method = syntaxTree.Descendants.OfType<MethodDeclaration>().First(m => m.Name == "SomeMethod");
            ResolveResult result = resolver.Resolve(method);
            MemberResolveResult memberResult = (MemberResolveResult)result;
            IMember member = memberResult.Member;
        }

        private void ProcessPositionalArgs(IList<ResolveResult> args)
        {
            if (args.Count == 0)
                return;
            Console.WriteLine("attribute.PositionalArguments :");
            for (Int32 index = 0; index < args.Count; ++index)
            {
                ResolveResult argResult = args[index];
                Console.Write("index = {0}, type = {1}", index, argResult.GetType());
                if (argResult is ConstantResolveResult)
                {
                    ConstantResolveResult constResult = (ConstantResolveResult) argResult;
                    Console.Write(", value = {0}", constResult.ConstantValue);
                }
                if (argResult is MemberResolveResult)
                {
                    MemberResolveResult memberResult = (MemberResolveResult) argResult;
                    IMember member = memberResult.Member;
                    Console.Write(", value = {0}, memberKind = {1}, typeKind = {2}", member.Name, member.SymbolKind, memberResult.Type.Kind);
                }
                Console.WriteLine();
            }
        }
    }
}