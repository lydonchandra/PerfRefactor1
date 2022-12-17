using Permutation;
using Shouldly;

namespace PermutationTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        List<int> input1 = new() { 1, 2, 3 };
        List<List<int>> output1 = Recursive.GetPermutations(input1);
        List<List<int>> expectedOutput = new()
        {
            new List<int> { 1, 2, 3 },
            new List<int> { 1, 3, 2 },
            new List<int> { 2, 1, 3 },
            new List<int> { 2, 3, 1 },
            new List<int> { 3, 2, 1 },
            new List<int> { 3, 1, 2 }
        };
        output1.ShouldBe(expectedOutput, true);
    }

    [Fact]
    public void TestQuickPerm()
    {
        List<int> input1 = new() { 1, 2, 3 };
        List<List<int>> output1 = QuickPerm.GetPermutations(input1);
        List<List<int>> expectedOutput = new()
        {
            new List<int> { 1, 2, 3 },
            new List<int> { 1, 3, 2 },
            new List<int> { 2, 1, 3 },
            new List<int> { 2, 3, 1 },
            new List<int> { 3, 2, 1 },
            new List<int> { 3, 1, 2 }
        };
        output1.ShouldBe(expectedOutput, true);
    }

    [Fact]
    public void TestQuickPermSwapRef()
    {
        List<int> input1 = new() { 1, 2, 3 };
        List<List<int>> output1 = QuickPerm.GetPermutations(input1);
        List<List<int>> expectedOutput = new()
        {
            new List<int> { 1, 2, 3 },
            new List<int> { 1, 3, 2 },
            new List<int> { 2, 1, 3 },
            new List<int> { 2, 3, 1 },
            new List<int> { 3, 2, 1 },
            new List<int> { 3, 1, 2 }
        };
        output1.ShouldBe(expectedOutput, true);
    }

    [Fact]
    public void TestQuickPerm10Items()
    {
        List<int> input1 = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<List<int>> output1 = QuickPerm.GetPermutations(input1);
        List<List<int>> expectedOutput = new()
        {
            new List<int> { 1, 2, 3 },
            new List<int> { 1, 3, 2 },
            new List<int> { 2, 1, 3 },
            new List<int> { 2, 3, 1 },
            new List<int> { 3, 2, 1 },
            new List<int> { 3, 1, 2 }
        };
        output1.ShouldBe(expectedOutput, true);
    }
}