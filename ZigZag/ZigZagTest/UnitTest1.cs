using Shouldly;

namespace ZigZagTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        string data1 = "PAYPALISHIRING";
        string result = new ZigZag.ZigZag().Convert(data1, numRows: 4);
        result.ShouldBe("PINALSIGYAHRPI");
        
        result = new ZigZag.ZigZag().Convert(data1, numRows: 3);
        result.ShouldBe("PAHNAPLSIIGYIR");
        
        data1 = "PAYPALISHIRINGNOWINTHEWORLD";
        result = new ZigZag.ZigZag().Convert(data1, numRows: 5);
        result.ShouldBe("PHWRASIOIOLYIRNNWDPLIGTEANH");
//new Solution().Convert(data2, numRows: 5);
//new Solution().Convert(data2, numRows: 6);
    }
}