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
                
                return root.NormalizeWhitespace().ToFullString();
            });
            
            return result;
        }
        
        
        
    }
}