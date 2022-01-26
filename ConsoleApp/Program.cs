using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using TestGeneratorLib;

namespace ConsoleApp {

    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Dasha_2\\RiderProjects\\SPP4\\ConsoleApp\\TestPrograms";
            string resultPath = @"C:\Users\Dasha_2\RiderProjects\SPP4\TestResult\";
            
            List<string> pathes = new List<string>(Directory.GetFiles(path));
            Pipeline pipeline = new Pipeline();
            pipeline.Start(pathes, resultPath);
        }
    }
    
    
    
}