using Permutation;
using Shouldly;

namespace PermutationTest;

public class UnitTest1
{
    private readonly List<List<int>> expectedOutputFor1234 = new()
    {
        new List<int> { 1, 2, 3, 4 },
        new List<int> { 1, 2, 4, 3 },
        new List<int> { 1, 3, 2, 4 },
        new List<int> { 1, 3, 4, 2 },
        new List<int> { 1, 4, 2, 3 },
        new List<int> { 1, 4, 3, 2 },
        new List<int> { 2, 1, 3, 4 },
        new List<int> { 2, 1, 4, 3 },
        new List<int> { 2, 3, 1, 4 },
        new List<int> { 2, 3, 4, 1 },
        new List<int> { 2, 4, 1, 3 },
        new List<int> { 2, 4, 3, 1 },
        new List<int> { 3, 1, 2, 4 },
        new List<int> { 3, 1, 4, 2 },
        new List<int> { 3, 2, 1, 4 },
        new List<int> { 3, 2, 4, 1 },
        new List<int> { 3, 4, 1, 2 },
        new List<int> { 3, 4, 2, 1 },
        new List<int> { 4, 1, 2, 3 },
        new List<int> { 4, 1, 3, 2 },
        new List<int> { 4, 2, 1, 3 },
        new List<int> { 4, 2, 3, 1 },
        new List<int> { 4, 3, 1, 2 },
        new List<int> { 4, 3, 2, 1 }
    };

    private readonly List<int[]> expectedOutputFor1234Array = new()
    {
        new[] { 1, 2, 3, 4 },
        new[] { 1, 2, 4, 3 },
        new[] { 1, 3, 2, 4 },
        new[] { 1, 3, 4, 2 },
        new[] { 1, 4, 2, 3 },
        new[] { 1, 4, 3, 2 },
        new[] { 2, 1, 3, 4 },
        new[] { 2, 1, 4, 3 },
        new[] { 2, 3, 1, 4 },
        new[] { 2, 3, 4, 1 },
        new[] { 2, 4, 1, 3 },
        new[] { 2, 4, 3, 1 },
        new[] { 3, 1, 2, 4 },
        new[] { 3, 1, 4, 2 },
        new[] { 3, 2, 1, 4 },
        new[] { 3, 2, 4, 1 },
        new[] { 3, 4, 1, 2 },
        new[] { 3, 4, 2, 1 },
        new[] { 4, 1, 2, 3 },
        new[] { 4, 1, 3, 2 },
        new[] { 4, 2, 1, 3 },
        new[] { 4, 2, 3, 1 },
        new[] { 4, 3, 1, 2 },
        new[] { 4, 3, 2, 1 }
    };

    private List<List<int>> expectedOutputFor123 = new()
    {
        new List<int> { 1, 2, 3 },
        new List<int> { 1, 3, 2 },
        new List<int> { 2, 1, 3 },
        new List<int> { 2, 3, 1 },
        new List<int> { 3, 2, 1 },
        new List<int> { 3, 1, 2 }
    };


    [Fact]
    public void Test1()
    {
        List<int> input1 = new() { 1, 2, 3, 4 };
        List<List<int>> output1 = Recursive.GetPermutations(input1);
        output1.ShouldBe(expectedOutputFor1234, true);
    }

    [Fact]
    public void TestQuickPerm()
    {
        List<int> input1 = new() { 1, 2, 3, 4 };
        List<List<int>> output1 = QuickPerm.GetPermutations(input1);
        output1.ShouldBe(expectedOutputFor1234, true);
    }

    [Fact]
    public void TestQuickPermMultiCores()
    {
        List<int> input1 = new() { 1, 2, 3, 4 };
        List<int[]> output1 = QuickPerm.GetPermutationsIntArrayMultiCores(input1.ToArray());
        output1.ShouldBe(expectedOutputFor1234Array, true);
    }

    [Fact]
    public void TestCompareQuickPermWithMultiCoresOdd()
    {
        List<int> input1 = new() { 1, 2, 3, 4, 5, 6, 7 };
        List<int[]> outputMultiCores = QuickPerm.GetPermutationsIntArrayMultiCores(input1.ToArray());
        List<int[]> outputSingleCore = QuickPerm.GetPermutationsIntArray(input1.ToArray());
        outputSingleCore.ShouldBe(outputMultiCores, true);
    }

    [Fact]
    public void TestCompareQuickPermWithMultiCoresEven()
    {
        List<int> input1 = new() { 1, 2, 3, 4, 5, 6 };
        List<int[]> outputMultiCores = QuickPerm.GetPermutationsIntArrayMultiCores(input1.ToArray());
        List<int[]> outputSingleCore = QuickPerm.GetPermutationsIntArray(input1.ToArray());
        outputSingleCore.ShouldBe(outputMultiCores, true);
    }
}