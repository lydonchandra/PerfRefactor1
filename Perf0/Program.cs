using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace PerfRefactor1;

public class Program
{
    // static void Main(string[] args)
    // {
    //     BenchmarkSwitcher.FromAssemblies(new[] { typeof(Program).Assembly }).Run(args);
    // }
    static void Main2()
    {
        IPrinter printer = new Printer();
        for (int i = 0; ; i++)
        {
            DoWork(printer, i);
        }
    }

    static void DoWork(IPrinter printer, int i)
    {
        printer.PrintIfTrue(i == int.MaxValue);
    }

    interface IPrinter
    {
        void PrintIfTrue(bool condition);
    }

    class Printer : IPrinter
    {
        public void PrintIfTrue(bool condition)
        {
            if (condition) Console.WriteLine("Print!");
        }
    }
}