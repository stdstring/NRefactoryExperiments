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
    public class OverloadMembersTests
    {
        [Test]
        public void MethodThisBaseOverloadings()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public void Do(int i) {}\r\n" +
                                  "        protected void Do(int i, int j) {}\r\n" +
                                  "        private void Do(int i, int j, int k) {}\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void Do(string s) {}\r\n" +
                                  "        public void M() {}\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            TypeDeclaration typeDeclaration = syntaxTree.Descendants.OfType<TypeDeclaration>().First(declaration => declaration.Name == "B");
            MethodDeclaration methodDeclaration = typeDeclaration.Descendants.OfType<MethodDeclaration>().First(declaration => declaration.Name == "Do");
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            //ResolveResult resolveResult = resolver.Resolve(methodDeclaration);
            ResolveResult resolveResult = resolver.Resolve(typeDeclaration);
            IType type = resolveResult.Type;
            IMethod[] methods = type.GetMethods().ToArray();
        }
    }


    [TestFixture]
    public class NestedTypesTests
    {
        [Test]
        public void NestedClasses()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public class B\r\n" +
                                  "        {\r\n" +
                                  "        }\r\n" +
                                  "    }\r\n" +
                                  "}";
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            //TypeDeclaration typeDeclaration = syntaxTree.Descendants.OfType<TypeDeclaration>().First(declaration => declaration.Name == "B");
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
        }
    }
}