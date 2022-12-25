// See https://aka.ms/new-console-template for more information

using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DnaLib;

var summary = BenchmarkRunner.Run<DnaBenchmark1>();

[MemoryDiagnoser(false)]
[HideColumns("Error", "RatioSD")]
[SimpleJob(1, 1, 2)]
public class DnaBenchmark1
{
    private readonly string _data = string.Empty;
    private readonly byte[] _dataBytes = { };
    private readonly string _path;

    public DnaBenchmark1()
    {
        _path = "gene-large.fna";

        using FileStream fileStream = new(_path, FileMode.Open, FileAccess.Read);
        using var streamReader = new StreamReader(fileStream);
        _data = streamReader.ReadToEnd();
        _dataBytes = Encoding.UTF8.GetBytes(_data);
    }

    // [Benchmark]
    // public async Task<bool> ValidateDnaFromFileAsChar()
    // {
    //     return await DnaUtil.ValidateDnaFromFileAsChar(_path);
    // }
    //
    // [Benchmark]
    // public async Task<bool> ValidateDnaFromFileAsByte()
    // {
    //     return await DnaUtil.ValidateDnaFromFileAsByte(_path);
    // }

    [Benchmark]
    public bool ValidateDnaChar()
    {
        return DnaUtil.ValidateDna(_data);
    }

    [Benchmark]
    public bool ValidateDnaByte()
    {
        return DnaUtil.ValidateDna(_dataBytes);
    }
}