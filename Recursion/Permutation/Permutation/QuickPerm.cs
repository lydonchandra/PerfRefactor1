namespace Permutation;

public class QuickPerm
{
    public static List<List<int>> GetPermutations(List<int> elementsToPermute)
    {
        List<List<int>> result = new();

        int n = elementsToPermute.Count;
        var p = new int[n + 1];
        for (var j = 0; j < p.Length; j++) p[j] = j;

        result.Add(new List<int>(elementsToPermute));

        var i = 1;
        while (i < n)
        {
            p[i] -= 1;
            var j = 0;
            if (i % 2 == 1) j = p[i];

            Swap(elementsToPermute, i, j);
            result.Add(new List<int>(elementsToPermute));
            i = 1;
            while (p[i] == 0)
            {
                p[i] = i;
                i += 1;
            }
        }

        return result;
    }

    public static List<int[]> GetPermutationsIntArray(int[] elementsToPermute)
    {
        List<int[]> result = new();

        int n = elementsToPermute.Length;
        var p = new int[n + 1];
        for (var j = 0; j < p.Length; j++) p[j] = j;

        result.Add(elementsToPermute.ToArray());

        var i = 1;
        while (i < n)
        {
            p[i] -= 1;
            var j = 0;
            if (i % 2 == 1) j = p[i];

            SwapIntArray(elementsToPermute, i, j);
            result.Add(elementsToPermute.ToArray());
            i = 1;
            while (p[i] == 0)
            {
                p[i] = i;
                i += 1;
            }
        }

        return result;
    }


    public static void Swap(List<int> array, int i, int j)
    {
        (array[i], array[j]) = (array[j], array[i]);
    }

    public static void SwapIntArray(int[] array, int i, int j)
    {
        (array[i], array[j]) = (array[j], array[i]);
    }
}