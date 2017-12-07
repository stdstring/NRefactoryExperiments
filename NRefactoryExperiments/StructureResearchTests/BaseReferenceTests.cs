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
    public class BaseReferenceTests
    {
        [Test]
        public void BaseMethodInvocation()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "    }\r\n" +
                                  "    public class C : B\r\n" +
                                  "    {\r\n" +
                                  "    public void Do() { base.M(); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void OverloadBaseMethodInvocation()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { }\r\n" +
                                  "        public void M(string s, int i) { }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void M(int i) { }\r\n" +
                                  "        public void M(string s) { }\r\n" +
                                  "    }\r\n" +
                                  "    public class C : B\r\n" +
                                  "    {\r\n" +
                                  "        public void Do() { base.M(\"iddqd\", 666); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBaseFieldInMethodCall()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class S\r\n" +
                                  "    {\r\n" +
                                  "        public static void Do(string s, int i) { }\r\n" +
                                  "    }\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int value;\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { S.Do(\"iddqd\", base.value); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBasePropertyInMethodCall()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class S\r\n" +
                                  "    {\r\n" +
                                  "        public static void Do(string s, int i) { }\r\n" +
                                  "    }\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int Value { get; set; }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { S.Do(\"iddqd\", base.Value); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBaseMethodInMethodCall()
        {
            const String source = "namespace nsnamespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class S\r\n" +
                                  "    {\r\n" +
                                  "        public static void Do(string s, int i) {}\r\n" +
                                  "    }\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int GetValue() { return 666; }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { S.Do(\"iddqd\", base.GetValue()); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBaseOverloadMethodInMethodCall()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class S\r\n" +
                                  "    {\r\n" +
                                  "        public static void Do(string s, int i) { }\r\n" +
                                  "    }\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int GetValue() { return 666; }\r\n" +
                                  "        public int GetValue(string s, int i) { return 667; }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public int GetValue(int i) { return 13; }\r\n" +
                                  "        public int GetValue(string s) { return 17; }\r\n" +
                                  "    }\r\n" +
                                  "    public class C : B\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { S.Do(\"iddqd\", base.GetValue(\"idkfa\", 999)); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBaseFieldInExpression()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int value;\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void Do() { System.Console.WriteLine(13 + base.value * 2); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBasePropertyInExpression()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int Value { get; set; }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void Do() { System.Console.WriteLine(13 + base.Value * 2); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBaseMethodInExpression()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int GetValue() { return 666; }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public void Do() { System.Console.WriteLine(13 + base.GetValue() * 2); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void UseBaseOverloadMethodInExpression()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public int GetValue() { return 666; }\r\n" +
                                  "        public int GetValue(string s, int i) { return 667; }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public int GetValue(int i) { return 13; }\r\n" +
                                  "        public int GetValue(string s) { return 17; }\r\n" +
                                  "    }\r\n" +
                                  "    public class C : B\r\n" +
                                  "    {\r\n" +
                                  "        public void Do() { System.Console.WriteLine(13 + 2 * base.GetValue(\"idkfa\", 999)); }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void BaseMethodInEvent()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public delegate void SomeDelegate();\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public event SomeDelegate SomeEvent;\r\n" +
                                  "        public void Do() { SomeEvent += base.M; }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        [Test]
        public void BaseOverloadMethodInEvent()
        {
            const String source = "namespace ns\r\n" +
                                  "{\r\n" +
                                  "    public delegate void SomeDelegate();\r\n" +
                                  "    public class A\r\n" +
                                  "    {\r\n" +
                                  "        public void M() { }\r\n" +
                                  "        public void M(int i) { }\r\n" +
                                  "    }\r\n" +
                                  "    public class B : A\r\n" +
                                  "    {\r\n" +
                                  "        public event SomeDelegate SomeEvent;\r\n" +
                                  "        public void Do() { SomeEvent += base.M; }\r\n" +
                                  "    }\r\n" +
                                  "}";
            TestCommonBody(source);
        }

        private void TestCommonBody(String source)
        {
            CSharpParser parser = new CSharpParser();
            SyntaxTree syntaxTree = parser.Parse(source);
            syntaxTree.FileName = "example.cs";
            BaseReferenceExpression baseRef = syntaxTree.Descendants.OfType<BaseReferenceExpression>().First();
            ExpressionStatement expr = MoveToParent<ExpressionStatement>(baseRef);
            CSharpUnresolvedFile unresolvedTypeSystem = syntaxTree.ToTypeSystem();
            IProjectContent content = new CSharpProjectContent();
            content = content.AddOrUpdateFiles(unresolvedTypeSystem);
            CecilLoader loader = new CecilLoader();
            AssemblyDefinition mscorlibAssemblyDefinition = AssemblyDefinition.ReadAssembly(typeof(Object).Assembly.Location);
            IUnresolvedAssembly mscorlibAssembly = loader.LoadAssembly(mscorlibAssemblyDefinition);
            content = content.AddAssemblyReferences(mscorlibAssembly);
            ICompilation compilation = content.CreateCompilation();
            CSharpAstResolver resolver = new CSharpAstResolver(compilation, syntaxTree);
            ShowSubtree(expr, 0, resolver);
        }

        private TNode MoveToParent<TNode>(AstNode node) where TNode : AstNode
        {
            AstNode parent = node;
            while (parent != null)
            {
                if (parent.GetType() == typeof (TNode))
                    return (TNode) parent;
                parent = parent.Parent;
            }
            return null;
        }

        private void ShowSubtree(AstNode node, Int32 indent, CSharpAstResolver resolver)
        {
            ResolveResult result = resolver.Resolve(node);
            Console.WriteLine("{0} node type = {1}, value {2}, resolve result = {3}", new String(' ', indent), node.GetType().Name, Prepare(node.ToString()), GetResolveResultRepresentation(result));
            foreach (AstNode child in node.Children)
                ShowSubtree(child, indent + IndentDelta, resolver);
        }

        private String Prepare(String source)
        {
            return source.Replace("\r", "\\r").Replace("\n", "\\n");
        }

        private String GetResolveResultRepresentation(ResolveResult result)
        {
            if (result is MethodGroupResolveResult)
            {
                String methodsRepresentation = String.Join(", ", ((MethodGroupResolveResult) result).Methods.Select(m => m.ToString()));
                return String.Concat(result, " : ", methodsRepresentation);
            }
            return result.ToString();
        }

        private const Int32 IndentDelta = 2;
    }
}