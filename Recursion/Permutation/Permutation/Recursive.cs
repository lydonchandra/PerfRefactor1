namespace Permutation;

public class Recursive
{
    /*
    Write a function that takes in an array of unique integers and returns an
    array of all permutations of those integers in no particular order.
    
    If the input array is empty, the function should return an empty array.
    input = [1, 2, 3]
    output = [[1, 2, 3], [1, 3, 2], [2, 1, 3], [2, 3, 1], [3, 1, 2], [3, 2, 1]]
    */
    public static List<List<int>> GetPermutations(List<int> array)
    {
        List<int> current = new();
        List<List<int>> result = new();
        Permute(current, array, result);
        return result;
    }

    private static void Permute(
        List<int> current,
        List<int> remainder,
        List<List<int>> result)
    {
        if (remainder.Count == 0)
        {
            result.Add(current);
            return;
        }

        for (var i = 0; i < remainder.Count; i++)
        {
            int elem = remainder[i];
            List<int> nextRemainder = new(remainder);
            nextRemainder.RemoveAt(i);
            List<int> nextCurrent = new(current) { elem };
            Permute(nextCurrent, nextRemainder, result);
        }
        // todo: performance test with large input
    }
}