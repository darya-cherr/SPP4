using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TestGeneratorLib
{
    public class TestGenerator
    {
        public Task<string> Generate(string code)
        {
            Task<string> result = Task<string>.Factory.StartNew(() =>
            {
                CodeAnalyzer analyzer = new CodeAnalyzer();
                CompilationUnitSyntax root = SyntaxFactory.CompilationUnit();
                
                NamespaceInfo namespaceInfo = analyzer.Analyse(code);
                SyntaxList<UsingDirectiveSyntax> usings = GenerateUsings(namespaceInfo);
                
                return root.NormalizeWhitespace().ToFullString();
            });
            
            return result;
        }
        private SyntaxList<UsingDirectiveSyntax> GenerateUsings(NamespaceInfo declaration)
        {
            return SyntaxFactory.List<UsingDirectiveSyntax>(new UsingDirectiveSyntax[]{
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName("NUnit.Framework")),
                SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(declaration.Name))});
        }
        
    }
}