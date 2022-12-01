using System;

namespace Perf0.Coding.SmallestDifference;

public class SmallestDifferenceMain
{
    public static int[] SmallestDifference(int[] arrayOne, int[] arrayTwo)
    {
        if (arrayOne.Length == 0 || arrayTwo.Length == 0)
            throw new ArgumentException("Check your input, one of them is empty");
        
        int smallestDiff = Int32.MaxValue;
        int arrayOneSmallestIdx = -1;
        int arrayTwoSmallestIdx = -1;
        
        for (int oneIdx = 0; oneIdx < arrayOne.Length; oneIdx++)
        {
            int oneElem = arrayOne[oneIdx];
            for (int twoIdx = 0; twoIdx < arrayTwo.Length; twoIdx++)
            {
                int twoElem = arrayTwo[twoIdx];
                int diffAbs = Math.Abs(oneElem - twoElem);
                if (diffAbs < smallestDiff)
                {
                    smallestDiff = diffAbs;
                    arrayOneSmallestIdx = oneIdx;
                    arrayTwoSmallestIdx = twoIdx;
                }
            }
        }
        
        return new [] { arrayOne[arrayOneSmallestIdx], arrayTwo[arrayTwoSmallestIdx] };
    }
    
    public static void Main(string[] args)
    {
    }
}