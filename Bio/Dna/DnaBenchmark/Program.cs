// See https://aka.ms/new-console-template for more information

using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Running;
using DnaLib;

var config = DefaultConfig.Instance.With(ConfigOptions.DisableOptimizationsValidator);
config.AddColumn(new TagColumn("FileSize", s => "20MB"));

var summary = BenchmarkRunner.Run<DnaBenchmark1>(config);
// var summary2 = BenchmarkRunner.Run<DnaBenchmarkReadFile>(config);

public enum DataSize
{
    sm,
    md,
    lg,
    xl
}

[MemoryDiagnoser]
[HideColumns("Error", "RatioSD")]
[SimpleJob(1, 1, 2)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class DnaBenchmark1
{
    public Dictionary<string, string> data = new();
    public Dictionary<string, byte[]> dataBytes = new();

    [Params(DataSize.sm, DataSize.lg, DataSize.xl)]
    public DataSize dataSize;

    private string _path => "Data/gene-" + dataSize + ".fna";

    [GlobalSetup]
    public void SetupData()
    {
        DataSize[] dataSizes = { DataSize.sm, DataSize.lg, DataSize.xl };
        foreach (var size in dataSizes)
        {
            var path = "Data/gene-" + size + ".fna";
            using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);
            var content = streamReader.ReadToEnd();
            data.Add(path, content);
            dataBytes.Add(path, Encoding.UTF8.GetBytes(content));
        }
    }

    // [Benchmark(Baseline = true)]
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

    // [Benchmark(Baseline = true)]
    // public bool ValidateDnaChar()
    // {
    //     return DnaUtil.ValidateDna(data[_path]);
    // }
    //
    // [Benchmark]
    // public bool ValidateDnaCharNaive()
    // {
    //     return DnaUtil.ValidateDnaNaive(data[_path]);
    // }
    //
    // [Benchmark]
    // public bool ValidateDnaCharPad256()
    // {
    //     return DnaUtil.ValidateDnaPad256(data[_path]);
    // }
    //
    // [Benchmark]
    // public bool ValidateDnaByte()
    // {
    //     return DnaUtil.ValidateDna(dataBytes[_path]);
    // }

    [Benchmark]
    public bool ValidateDnaBytePad128()
    {
        return DnaUtil.ValidateDnaPad128(dataBytes[_path]);
    }

    [Benchmark]
    public bool ValidateDnaContainsAnyExcept()
    {
        return DnaUtil.ValidateDnaVec256(dataBytes[_path]);
    }
}

[MemoryDiagnoser]
[HideColumns("Error", "RatioSD")]
[SimpleJob(1, 1, 3)]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class DnaBenchmarkReadFile
{
    [Params(DataSize.sm, DataSize.lg, DataSize.xl)]
    public DataSize dataSize;

    private string _path => "Data/gene-" + dataSize + ".fna";

    [GlobalSetup]
    public void SetupData()
    {
    }

    [Benchmark(Baseline = true)]
    public async Task<bool> ValidateDnaFromFileAsChar()
    {
        return await DnaUtil.ValidateDnaFromFileAsChar(_path);
    }

    [Benchmark]
    public async Task<bool> ValidateDnaFromFileAsByte()
    {
        return await DnaUtil.ValidateDnaFromFileAsByte(_path);
    }
}