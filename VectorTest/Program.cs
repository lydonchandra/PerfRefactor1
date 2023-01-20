using System.Numerics;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text.Json;

const int start = 20;
const int length = 32;
var arr1 = Enumerable.Range(start, start + length).ToArray();

// Util.TestShuffle(arr1);
// void* dnaLowerCaseBytes = "ARNDCQEGHILKMFPSTWYXJOUBZ-._;;;;"u8.ToArray().AsMemory().Pin().Pointer;
//
// Vector256<byte> vecInput0 = Vector256.Load((byte*)dnaLowerCaseBytes);
// var value = (byte)'M';
// Vector256<byte> vecCompare0 = Vector256.Create(value);
// Vector256<byte> result = Vector256.Equals(vecInput0, vecCompare0);
// Vector128<byte> lower = result.GetLower();
//
// // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
// Vector256<byte> eq = Avx2.CompareEqual(vecInput0, vecCompare0);
// // Move comparison results into a bitmap of 32 bits
// var bmp = unchecked((uint)Avx2.MoveMask(eq));
// // Find index of the first byte in the vectors which compared equal
// // The method will return 32 if none of the bytes compared equal
// var firstEqualIndex = BitOperations.TrailingZeroCount(bmp);

ReadOnlySpan<byte> proteinSeq = "MLDPTGTYRRPRDTQDSRQKRRQDCLDPTGQY"u8;
var compressedProteinSeq = Util.Compress(proteinSeq);
compressedProteinSeq.ToString();

public static class Util
{
    public static byte[] Compress(ReadOnlySpan<byte> proteinSeq)
    {
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            void* proteinLut = "ARNDCQEGHILKMFPSTWYXJOUBZ-._;;;;"u8.ToArray().AsMemory().Pin().Pointer;

            Vector256<byte> vecInput0 = Vector256.Load((byte*)proteinLut);

            for (var index = 0; index < proteinSeq.Length; index++)
            {
                var protAa = proteinSeq[index];
                // var value = (byte)'M';
                var value = protAa;
                Vector256<byte> vecCompare0 = Vector256.Create(value);
                Vector256<byte> result = Vector256.Equals(vecInput0, vecCompare0);

                // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
                Vector256<byte> eq = Avx2.CompareEqual(vecInput0, vecCompare0);
                // Move comparison results into a bitmap of 32 bits
                var bmp = unchecked((uint)Avx2.MoveMask(eq));
                // Find index of the first byte in the vectors which compared equal
                // The method will return 32 if none of the bytes compared equal
                var firstEqualIndex = BitOperations.TrailingZeroCount(bmp);
                results[index] = (byte)firstEqualIndex;
            }

            return results;
        }
    }

    public static bool TestShuffle(int[] arr1)
    {
        unsafe
        {
            var arr1LeftPtr = (int*)arr1.AsMemory().Pin().Pointer;
            // var arr1RightPtr = arr1LeftPtr + arr1.Length - Vector128<int>.Count;
            Vector128<int> left = Sse2.LoadVector128(arr1LeftPtr);
            // Vector128<int> right = Sse2.LoadVector128(arr1RightPtr);

            // 0x1b == 27 == 00 01 10 11 == 0 1 2 3
            // 0x1e == 30 == 00 01 11 10 == 0 1 3 2
            Vector128<int> reversedLeft = Sse2.Shuffle(left, 0b00_01_10_11);
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