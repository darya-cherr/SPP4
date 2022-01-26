using System.Collections.Generic;


namespace TestGeneratorLib
{
    public class ClassInfo
    {
        public string Name { get; }
        public List<MethodInfo> Methods { get; }
        
        public ClassInfo(string name, IEnumerable<MethodInfo> methods) 
        { 
            Name = name; 
            Methods = new List<MethodInfo>(methods);
        }
    }
}