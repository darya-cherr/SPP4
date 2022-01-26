using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp;
using NUnit.Framework;

public class Tests
{
    string path = "C:\\Users\\Dasha_2\\RiderProjects\\SPP4\\ConsoleApp\\TestPrograms";
    string resultPath = @"C:\Users\Dasha_2\RiderProjects\SPP4\UnitTests\ResultTests\";
    private List<string> pathes;
    
    [SetUp] 
    public void Setup() 
    { 
       pathes = new List<string>(Directory.GetFiles(path));
    }
    
    [Test]
    public void FilesNumberTest()
    {
        Assert.AreEqual(pathes.Count, 3);
    }

    [Test]
    public void LibraryTest()
    {
        new Pipeline().Start(pathes,resultPath);
        var generatedFiles = Directory.GetFiles(resultPath);
        Assert.AreEqual(generatedFiles.Length, 3);
    }
}