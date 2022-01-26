using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using TestGeneratorLib;

namespace ConsoleApp
{
    public class Pipeline
    {
         public static string resultPath;
         public static async Task<PipeItem[]> Read(PipeItem[] items) 
         { 
             Console.WriteLine("Reading"); 
             return await Task<PipeItem[]>.Factory.StartNew(() => items.Select(x => new PipeItem(x.path, File.ReadAllText(x.path))).ToArray());
         }
         
         public static async Task<PipeItem[]> Generate(PipeItem[] items) 
         { 
             TestGenerator Generator = new TestGenerator(); 
             Console.WriteLine("Generating"); 
             return await Task<PipeItem[]>.Factory.StartNew(() => items.Select(x => new PipeItem(x.path, Generator.Generate(x.code).Result)).ToArray());
         }
        
        public static void Write(PipeItem[] items) 
        { 
            Console.WriteLine("Writing"); 
            foreach (PipeItem item in items) 
            { 
                string name = Path.GetFileNameWithoutExtension(item.path); 
                name = name.Split('\\').Last(); 
                File.WriteAllTextAsync(resultPath + name + "Result" + Path.GetExtension(item.path), item.code);
            }
        }

        public void Start(List<string> pathes, string resPath)
        {
            List<IEnumerable<PipeItem>> items = new List<IEnumerable<PipeItem>>();
            resultPath = resPath;
            
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