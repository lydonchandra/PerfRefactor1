// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Permutation;

var summary = BenchmarkRunner.Run<Md5VsSha256>();

[MemoryDiagnoser(false)]
[HideColumns("Error", "RatioSD")]
[SimpleJob(1, 1, 3)]
public class Md5VsSha256
{
    private readonly int[] _data;

    public Md5VsSha256()
    {
        _data = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
    }

    // [Benchmark]
    // public List<List<int>> RecursivePermutation()
    // {
    //     return Recursive.GetPermutations(_data.ToList());
    // }

    // [Benchmark]
    // public List<List<int>> QuickPermutation()
    // {
    //     return QuickPerm.GetPermutations(_data.ToList());
    // }

    [Benchmark]
    public List<int[]> QuickPermutationIntArray()
    {
        return QuickPerm.GetPermutationsIntArray(_data);
    }

    // [Benchmark]
    // public List<int[]> QuickPermutationArray2Batches()
    // {
    //     return QuickPerm.GetPermutationsIntArray2Batches(_data);
    // }

    [Benchmark]
    public List<int[]> QuickPermutationMultiCores()
    {
        return QuickPerm.GetPermutationsIntArrayMultiCores(_data);
    }
}