using System;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.Semantics;
using ICSharpCode.NRefactory.TypeSystem;
using Mono.Cecil;
using NUnit.Framework;

namespace StructureResearchTests
{
    [TestFixture]
    public class OperatorsTests
    {
        [Test]
        public void OperatorAdd()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public static A operator +(A a1, A a2) { throw new System.NotImplementedException(); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            TypeDeclaration typeDeclaration = syntaxTree.Descendants.OfType<TypeDeclaration>().First(declaration => declaration.Name == "A");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof (Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(typeDeclaration);
            IType type = resolveResult.Type;
            IMethod[] methods = type.GetMethods().ToArray();
        }

        [Test]
        public void ExplicitCastOperators()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public static explicit operator A(int i) { throw new System.NotImplementedException(); }\r\n" +
                                  "        public static explicit operator int(A a) { throw new System.NotImplementedException(); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            TypeDeclaration typeDeclaration = syntaxTree.Descendants.OfType<TypeDeclaration>().First(declaration => declaration.Name == "A");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(typeDeclaration);
            IType type = resolveResult.Type;
            IMethod[] methods = type.GetMethods().ToArray();
        }

        [Test]
        public void ImplicitCastOperators()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public static implicit operator A(int i) { throw new System.NotImplementedException(); }\r\n" +
                                  "        public static implicit operator int(A a) { throw new System.NotImplementedException(); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            TypeDeclaration typeDeclaration = syntaxTree.Descendants.OfType<TypeDeclaration>().First(declaration => declaration.Name == "A");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(typeDeclaration);
            IType type = resolveResult.Type;
            IMethod[] methods = type.GetMethods().ToArray();
        }
    }
}