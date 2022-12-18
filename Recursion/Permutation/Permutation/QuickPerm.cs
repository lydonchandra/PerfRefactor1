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

    private static List<int[]> QuickPermBatch(int[] elementsToPermute, int batchSize, int batchNo)
    {
        List<int[]> result = new();
        int n = elementsToPermute.Length;
        int nBatchStart = batchNo * batchSize;
        int nBatchEnd = nBatchStart + batchSize;

        //todo: defect with odd elements
        var pBatch = new int[n + 1];
        for (var j = 0; j < pBatch.Length; j++)
            pBatch[j] = j;

        int iBatch = batchNo == 0 ? 1 : nBatchStart;
        while (iBatch < nBatchEnd)
        {
            pBatch[iBatch] -= 1;
            var j = 0;
            if (iBatch % 2 == 1) j = pBatch[iBatch];
            SwapIntArray(elementsToPermute, iBatch, j);
            result.Add(elementsToPermute.ToArray());
            iBatch = 1;
            while (pBatch[iBatch] == 0)
            {
                pBatch[iBatch] = iBatch;
                iBatch += 1;
            }
        }

        return result;
    }

    public static List<int[]> GetPermutationsIntArrayMultiCores(int[] elementsToPermute)
    {
        var numCores = 2;
        List<int[]> result = new();
        object resultLock = new();

        int n = elementsToPermute.Length;
        int nBatch = n / numCores;

        var p = new int[n + 1];
        for (var j = 0; j < p.Length; j++)
            p[j] = j;

        result.Add(elementsToPermute.ToArray());

        Parallel.For(0, numCores, (i, loop) =>
            // for (var i = 0; i < numCores; i++)
        {
            //todo: batchSize is incorrect, if numCores == 2, then for 7 element, might be 5 & 2 
            List<int[]> bla = QuickPermBatch(elementsToPermute.ToArray(), nBatch, i);
            lock (resultLock)
            {
                result.AddRange(bla);
            }
        });

        return result;
    }

    public static List<int[]> GetPermutationsIntArray2Batches(int[] elementsToPermute)
    {
        var numCores = 2;
        List<int[]> result = new();

        int n = elementsToPermute.Length;
        int nBatch = n / numCores;

        var p = new int[n + 1];
        for (var j = 0; j < p.Length; j++)
            p[j] = j;

        result.Add(elementsToPermute.ToArray());

        var i = 1;

        while (i < nBatch) // i = 1
        {
            p[i] -= 1; // p[1] = 1 - 1 = 0 
            var j = 0;
            if (i % 2 == 1) j = p[i]; // j = p[1] = 0

            SwapIntArray(elementsToPermute, i, j); // swap(arr, 1, 0)
            result.Add(elementsToPermute.ToArray());
            i = 1; // i = 1
            while (p[i] == 0) // p[1] = 0
            {
                p[i] = i; // p[1] = 1
                i += 1; // i = 2
            }
        }

        var pBatch = new int[n + 1];
        for (var j = 0; j < p.Length; j++)
            pBatch[j] = j;

        int iBatch = nBatch;
        while (iBatch < n)
        {
            pBatch[iBatch] -= 1;
            var j = 0;
            if (iBatch % 2 == 1) j = pBatch[iBatch];
            SwapIntArray(elementsToPermute, iBatch, j);
            result.Add(elementsToPermute.ToArray());
            iBatch = 1;
            while (pBatch[iBatch] == 0)
            {
                pBatch[iBatch] = iBatch;
                iBatch += 1;
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