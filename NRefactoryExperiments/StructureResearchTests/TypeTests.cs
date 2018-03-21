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
    public class TypeTests
    {
        [Test]
        public void ParseStaticClass()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public static class A\r\n" +
                                  "    {\r\n" +
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
        }
    }
}