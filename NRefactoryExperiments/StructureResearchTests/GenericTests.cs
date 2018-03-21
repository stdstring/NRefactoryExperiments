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
    public class GenericTests
    {
        [Test]
        public void GenericType()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class SomeType<T>\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod(T value)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            MethodDeclaration methodDeclaration = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "SomeMethod");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof (Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(methodDeclaration);
        }

        [Test]
        public void GenericMethod()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class SomeType\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>(T value)\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            MethodDeclaration methodDeclaration = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "SomeMethod");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof (Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(methodDeclaration);
        }

        [Test]
        public void GenericMethodWithClassConstraint()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class SomeType\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>(T value) where T: class\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            MethodDeclaration methodDeclaration = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "SomeMethod");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof (Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(methodDeclaration);
        }

        [Test]
        public void GenericMethodWithGenericBaseClassConstraint()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A<T>\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<TData>(TData data) where TData : A<TData>\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "        public void OtherMethod<T, U>(T data1, U data2) where T : U\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            MethodDeclaration methodDeclaration1 = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "SomeMethod");
            MethodDeclaration methodDeclaration2 = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "OtherMethod");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult1 = resolver.Resolve(methodDeclaration1);
            ResolveResult resolveResult2 = resolver.Resolve(methodDeclaration2);
        }

        [Test]
        public void GenericMethodWithSeveralInheritanceConstraints()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A<T>\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>(T value) where T : A<T>, System.Collections.Generic.IList<T>, System.Collections.Generic.ICollection<T>, System.IComparable<T>, System.ICloneable\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            MethodDeclaration methodDeclaration = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "SomeMethod");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(methodDeclaration);
        }

        [Test]
        public void GenericMethodWithComplexGenericBaseClassConstraint()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A<T>\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class B<T>\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class C<T>\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class SomeClass\r\n" +
                                  "    {\r\n" +
                                  "        public void SomeMethod<T>(T value) where T : C<B<A<T>>>\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            MethodDeclaration methodDeclaration = syntaxTree.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "SomeMethod");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ResolveResult resolveResult = resolver.Resolve(methodDeclaration);
        }
    }
}