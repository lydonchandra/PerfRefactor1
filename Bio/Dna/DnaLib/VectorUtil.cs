using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace DnaLib;

public class VectorUtil
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool IsSorted(int[] array)
    {
        if (array.Length < 2) return true;

        for (var i = 0; i < array.Length - 1; i++)
            if (array[i] > array[i + 1])
                return false;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool IsSorted_Sse41(int[] array)
    {
        unsafe
        {
            fixed (int* ptr = &array[0])
            {
                for (var i = 0; i < array.Length - 4; i += 4)
                {
                    Vector128<int> curr = Sse2.LoadVector128(ptr + i);
                    Vector128<int> next = Sse2.LoadVector128(ptr + i + 1);
                    Vector128<int> mask = Sse2.CompareGreaterThan(curr, next);
                    if (!Sse41.TestZ(mask, mask))
                        return false;
                }
            }

            return true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool IsSorted_Vector256(int[] array)
    {
        unsafe
        {
            fixed (int* ptr = &array[0])
            {
                for (var i = 0; i < array.Length - 8; i += 8)
                {
                    Vector256<int> curr = Vector256.Load(ptr + i);
                    Vector256<int> next = Vector256.Load(ptr + i + 1);
                    Vector256<int> mask = Avx2.CompareGreaterThan(curr, next);
                    if (!Avx.TestZ(mask, mask))
                        return false;
                }
            }

            return true;
        }
    }
}