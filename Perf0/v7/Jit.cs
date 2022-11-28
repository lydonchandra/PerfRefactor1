using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

[DisassemblyDiagnoser]
public class Jit
{
    private int _a = 42, _b = 84;

    [Benchmark]
    public int Min() => Math.Min(_a, _b);
}