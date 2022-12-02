using System.Diagnostics;
using Perf0.Coding.SmallestDifference;
using Shouldly;
using Xunit.Abstractions;

namespace Perf0Test.SmallestDifference;

public class UnitTest1
{
    private readonly ITestOutputHelper _testOutputHelper;

    [Fact]
    public void SmallestDifferencePerfTest()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        var data = new SmallestDifferenceTestData();
        int[] result = SmallestDifferenceMain.SmallestDifference(data.arrayOne, data.arrayTwo);
        stopwatch.Stop();
        _testOutputHelper.WriteLine(stopwatch.ElapsedMilliseconds.ToString());
        result.ShouldNotBeEmpty();
    }


    public UnitTest1(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
}



