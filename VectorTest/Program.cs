using System.Dynamic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

var start = (int)DateTime.Now.Ticks % 100;
var length = start;
var arr2 = Enumerable.Range(0, 10_000_000).ToArray();
var obj = new ExpandoObject();
obj.TryAdd("value", arr2);
using var sw = new StreamWriter(File.OpenWrite("/home/don/bla1.json"));
sw.Write(JsonSerializer.Serialize(obj));
sw.Flush();
sw.Close();

char[] arr1 = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
// Util.TestUint1(arr1);
Util.IsSorted(new[] { 5, 6, 4, 3, 4, 7 });
Util.IsSorted_Sse41(new[] { 5, 6, 4, 3, 4, 7 });
Console.WriteLine(Vector<byte>.Count + " " + Vector256<byte>.Count);
// Util.TestShuffle(arr1);


public static class Util
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool IsSorted(int[] array)
    {
        if (array.Length < 2) return true;

        for (var i = 0; i < array.Length; i++)
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
                    if (Avx.TestZ(mask, mask))
                        return false;
                }
            }

            return true;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static void TestUint1(char[] array)
    {
        // if ((uint)array.Length > 5)
        // {
        //     array[0] = 'F';
        //     array[1] = 'a';
        //     array[2] = 'l';
        //     array[3] = 's';
        //     array[4] = 'e';
        //     array[5] = '.';
        // }

        if (array.Length > 5)
        {
            array[0] = 'F';
            array[1] = 'a';
            array[2] = 'l';
            array[3] = 's';
            array[4] = 'e';
            array[5] = '.';
        }
    }

    public static void TestForLoopSignedInt(char[] array)
    {
        for (var i = 0; i < array.Length - 2; i += 2)
        {
            array[i] = 'F';
            array[i + 1] = 'a';
        }
    }

    public static void TestForLoopUnsignedInt(char[] array)
    {
        if ((uint)array.Length < 8) return;
        for (uint i = 0; i < (uint)array.Length - 2; i += 2)
        {
            array[i] = 'F';
            array[i + 1] = 'a';
        }
    }

    public static bool TestShuffle(int[] arr1)
    {
        unsafe
        {
            var arr1LeftPtr = (int*)arr1.AsMemory().Pin().Pointer;
            // var arr1RightPtr = arr1LeftPtr + arr1.Length - Vector128<int>.Count;
            Vector128<int> left = Sse2.LoadVector128(arr1LeftPtr); // left:  23, 22, 21, 20
            // Vector128<int> right = Sse2.LoadVector128(arr1RightPtr);

            // 0x1b == 27 == 00 01 10 11 == 0 1 2 3
            // 0x1e == 30 == 00 01 11 10 == 0 1 3 2
            Vector128<int> reversedLeft = Sse2.Shuffle(left, 0b00_01_10_11); // reversed: 20, 21, 22, 23
            Sse2.Store(arr1LeftPtr, reversedLeft);
            return true;
            // Vector128<int> reversedRight = Sse2.Shuffle(right, 0b00_01_10_11);

            // Sse2.Store(arr1RightPtr, reversedLeft);
            // Sse2.Store(arr1LeftPtr, reversedRight);
        }
    }


    public static bool ContainsAnyExcept(ReadOnlySpan<byte> input, byte[] excepts)
    {
        unsafe
        {
            void* dnaLowerCaseBytes = excepts.AsMemory().Pin().Pointer;
            var valid = true;

            Vector64<byte> vec1A = Vector64.Load((byte*)dnaLowerCaseBytes);
            Vector64<byte> vec2A = Vector64.Load((byte*)dnaLowerCaseBytes);
            Vector64<byte> vec3A = Vector64.Load((byte*)dnaLowerCaseBytes);
            Vector64<byte> vec4A = Vector64.Load((byte*)dnaLowerCaseBytes);

            Vector256<byte> vec256ExceptArg = Vector256.Create(
                Vector128.Create(vec1A, vec2A),
                Vector128.Create(vec3A, vec4A)
            );

            for (var i = 0; i < input.Length; i += 4)
            {
                Vector64<byte> vec1B = Vector64.Create(input[i]);
                Vector64<byte> vec2B = Vector64.Create(input[i + 1]);
                Vector64<byte> vec3B = Vector64.Create(input[i + 2]);
                Vector64<byte> vec4B = Vector64.Create(input[i + 3]);

                Vector256<byte> vec256dnaSeq = Vector256.Create(
                    Vector128.Create(vec1B, vec2B),
                    Vector128.Create(vec3B, vec4B)
                );

                Vector256<byte> result = Vector256.Equals(vec256dnaSeq, vec256ExceptArg);
                Vector128<byte> result128Lower = result.GetLower();
                Vector128<byte> result128Upper = result.GetUpper();

                if (
                    Vector64.Equals(result128Lower.GetLower(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
                    Vector64.Equals(result128Lower.GetUpper(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
                    Vector64.Equals(result128Upper.GetLower(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
                    Vector64.Equals(result128Upper.GetUpper(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet)
                {
                    valid = false;
                    break;
                }
            }

            //todo: handle remainder if % 4 is not zero
            return valid;
        }
    }
}

public static class Dumper
{
    public static string ToPrettyString(this object value)
    {
        return JsonSerializer.Serialize(value, new JsonSerializerOptions { WriteIndented = false });
    }

    public static T Dump<T>(this T value)
    {
        Console.WriteLine(value.ToPrettyString());
        return value;
    }
}


// unsafe
// {
//     void* dnaLowerCaseBytes = "acgtACGT"u8.ToArray().AsMemory().Pin().Pointer;
//     var dnaSeq = "acgtacgtacgtacgtacgtacgtacgtacgt"u8.ToArray();
//     var valid = true;
//
//     Vector64<byte> vec1A = Vector64.Load((byte*)dnaLowerCaseBytes);
//     Vector64<byte> vec2A = Vector64.Load((byte*)dnaLowerCaseBytes);
//     Vector64<byte> vec3A = Vector64.Load((byte*)dnaLowerCaseBytes);
//     Vector64<byte> vec4A = Vector64.Load((byte*)dnaLowerCaseBytes);
//
//     Vector256<byte> vec256ExceptArg = Vector256.Create(
//         Vector128.Create(vec1A, vec2A),
//         Vector128.Create(vec3A, vec4A)
//     );
//
//     for (var i = 0; i < dnaSeq.Length; i += 4)
//     {
//         Vector64<byte> vec1B = Vector64.Create(dnaSeq[i]);
//         Vector64<byte> vec2B = Vector64.Create(dnaSeq[i + 1]);
//         Vector64<byte> vec3B = Vector64.Create(dnaSeq[i + 2]);
//         Vector64<byte> vec4B = Vector64.Create(dnaSeq[i + 3]);
//
//         Vector256<byte> vec256dnaSeq = Vector256.Create(
//             Vector128.Create(vec1B, vec2B),
//             Vector128.Create(vec3B, vec4B)
//         );
//
//         Vector256<byte> result = Vector256.Equals(vec256dnaSeq, vec256ExceptArg);
//         Vector128<byte> result128Lower = result.GetLower();
//         Vector128<byte> result128Upper = result.GetUpper();
//
//         if (
//             Vector64.Equals(result128Lower.GetLower(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
//             Vector64.Equals(result128Lower.GetUpper(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
//             Vector64.Equals(result128Upper.GetLower(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
//             Vector64.Equals(result128Upper.GetUpper(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet)
//         {
//             valid = false;
//             break;
//         }
//     }
//
//     Debug.Assert(valid);
// }