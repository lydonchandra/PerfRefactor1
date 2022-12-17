18/12/2022

Using `int[]` instead of `List<int>` by about `~30%` and reduce memory allocation by `~20%`

| Method                   |     Mean |   StdDev | Allocated |
|--------------------------|---------:|---------:|----------:|
| QuickPermutation         | 589.6 ms | 96.05 ms | 396.23 MB |
| QuickPermutationIntArray | 391.3 ms | 41.51 ms | 285.49 MB |

17/12/2022
input: new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

| Method               |       Mean |    StdDev |  Allocated |
|----------------------|-----------:|----------:|-----------:|
| RecursivePermutation | 2,181.8 ms | 113.66 ms | 2364.44 MB |
| QuickPermutation     |   605.1 ms |  82.83 ms |  396.23 MB |
