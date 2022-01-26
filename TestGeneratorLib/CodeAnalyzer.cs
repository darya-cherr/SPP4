using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGeneratorLib
{
    public class CodeAnalyzer
    {
        public NamespaceInfo Analyse(string code)
        {
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();
            var namespaceDecalaration = root.Members.OfType<NamespaceDeclarationSyntax>().First();
            var classDeclaration = namespaceDecalaration.Members.OfType<ClassDeclarationSyntax>().First();
            var methodDeclaration = classDeclaration.Members.OfType<MethodDeclarationSyntax>().Where(method => method.Modifiers.Where(modifier => modifier.Kind() == SyntaxKind.PublicKeyword).Any());
            
            IEnumerable<MethodInfo> methods = methodDeclaration.Select(method => new MethodInfo(method.Identifier.ToString()));
            ClassInfo classes = new ClassInfo(classDeclaration.Identifier.ToString(), methods);
            NamespaceInfo namespaces = new NamespaceInfo(namespaceDecalaration.Name.ToString(), classes);

            return namespaces;
        }
    }
}