// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DnaLib;

var summary = BenchmarkRunner.Run<DnaBenchmark1>();

[MemoryDiagnoser(false)]
[HideColumns("Error", "RatioSD")]
[SimpleJob(2, 2, 3)]
public class DnaBenchmark1
{
    private readonly string _data;

    public DnaBenchmark1()
    {
        _data = "gene.fna";
    }

    [Benchmark]
    public async Task<bool> ValidateDnaFromFile()
    {
        return await DnaUtil.ValidateDnaFromFile(_data);
    }

    [Benchmark]
    public async Task<bool> ValidateDnaFromFileAsByte()
    {
        return await DnaUtil.ValidateDnaFromFileAsByte(_data);
    }
}