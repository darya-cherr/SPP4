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
        private static TestGenerator Generator = new TestGenerator();
        private static string resultPath = @"C:\Users\Dasha_2\RiderProjects\SPP4\TestResult\";
        private static async Task<PipeItem[]> Read(PipeItem[] items)
        {
            Console.WriteLine("Reading");
            return await Task<PipeItem[]>.Factory.StartNew(() => items.Select(x => new PipeItem(x.path, File.ReadAllText(x.path))).ToArray());
        }
        
        private static async Task<PipeItem[]> Generate(PipeItem[] items)
        {
            Console.WriteLine("Generating");
            return await Task<PipeItem[]>.Factory.StartNew(() => items.Select(x => new PipeItem(x.path, Generator.Generate(x.code).Result)).ToArray());
        }

        private static void Write(PipeItem[] items)
        {
            Console.WriteLine("Writing");
            foreach (PipeItem item in items)
            {
                string name = Path.GetFileNameWithoutExtension(item.path);
                name = name.Split('\\').Last();
                File.WriteAllTextAsync(resultPath + name + "Result" + Path.GetExtension(item.path), item.code);
            }
        }
        
        static void Main(string[] args)
        {
            string path = "C:\\Users\\Dasha_2\\RiderProjects\\SPP4\\ConsoleApp\\TestPrograms";
            
            
            List<string> pathes = new List<string>(Directory.GetFiles(path));
            List<IEnumerable<PipeItem>> items = new List<IEnumerable<PipeItem>>();
           
            
            for (int i = 0; i < pathes.Count; i += 2) 
            {
                items.Add(pathes.GetRange(i, Math.Min(2, pathes.Count - i)).Select(x => new PipeItem(x))); 
            }
            
            List<ActionBlock<PipeItem[]>> writes = new List<ActionBlock<PipeItem[]>>();

            foreach (IEnumerable<PipeItem> item in items)
            {
                var read = new TransformBlock<PipeItem[], PipeItem[]>(Read);
                var generate = new TransformBlock<PipeItem[], PipeItem[]>(Generate);
                var write = new ActionBlock<PipeItem[]>(Write);
                var linkOptions = new DataflowLinkOptions { PropagateCompletion = true };
                
                read.LinkTo(generate, linkOptions);
                generate.LinkTo(write, linkOptions);
                
                read.Post(item.ToArray());
                read.Complete();
                writes.Add(write);
            }
            
            foreach (ActionBlock<PipeItem[]> action in writes)
            {
                action.Completion.Wait();
            }
            Console.Write("End");
        }
    }
    
    
    
}