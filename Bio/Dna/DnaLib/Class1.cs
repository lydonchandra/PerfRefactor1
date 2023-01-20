﻿using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DnaLib;

public static class bla
{
    private const byte ANY = 20;
    private const byte GAP = 21;

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] CompressSimd(ReadOnlySpan<byte> proteinSeq)
    {
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            //TODO: handle UBZ, lowercase
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte aa2i(byte a)
    {
        switch (a)
        {
            case (byte)'A': return 0;
            case (byte)'R': return 1;
            case (byte)'N': return 2;
            case (byte)'D': return 3;
            case (byte)'C': return 4;
            case (byte)'Q': return 5;
            case (byte)'E': return 6;
            case (byte)'G': return 7;
            case (byte)'H': return 8;
            case (byte)'I': return 9;
            case (byte)'L': return 10;
            case (byte)'K': return 11;
            case (byte)'M': return 12;
            case (byte)'F': return 13;
            case (byte)'P': return 14;
            case (byte)'S': return 15;
            case (byte)'T': return 16;
            case (byte)'W': return 17;
            case (byte)'Y': return 18;
            case (byte)'V': return 19;
            case (byte)'X': return ANY;
            case (byte)'J': return ANY;
            case (byte)'O': return ANY;
            case (byte)'U': return 4; //Selenocystein -> Cystein
            case (byte)'B': return 3; //D (or N)
            case (byte)'Z': return 6; //E (or Q)
            case (byte)'-': return GAP;
            case (byte)'.': return GAP;
            case (byte)'_': return GAP;
            default: return 31;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] Compress(ReadOnlySpan<byte> proteinSeq)
    {
        var results = new byte[proteinSeq.Length];


        for (var index = 0; index < proteinSeq.Length; index++)
        {
            var protAa = proteinSeq[index];
            results[index] = aa2i(protAa);
        }

        return results;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ContainsAnyExcept64(this ReadOnlySpan<byte> input, byte[] except)
    {
        unsafe
        {
            var exceptPtr = (byte*)except.AsMemory().Pin().Pointer;
            Vector64<byte> vecExcept = Vector64.Load(exceptPtr);

            var valid = true;

            Vector64<byte> equals;
            for (var i = 0; i < input.Length; i++)
            {
                Vector64<byte> vecInput = Vector64.Create(input[i]);
                equals = Vector64.Equals(vecExcept, vecInput);
                if (equals != Vector64<byte>.Zero) continue;

                valid = false;
                break;
            }

            return valid;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ContainsAnyExcept(this ReadOnlySpan<byte> input, byte[] except)
    {
        unsafe
        {
            var exceptPtr = (byte*)except.AsMemory().Pin().Pointer;
            //todo: investigate why Vector<64> is super slow
            //todo: try Vector256
            Vector128<byte> vecExcept = Vector128.Load(exceptPtr);

            var valid = true;

            Vector128<byte> equals;
            for (var i = 0; i < input.Length; i++)
            {
                Vector128<byte> vecInput = Vector128.Create(input[i]);
                equals = Vector128.Equals(vecExcept, vecInput);
                if (equals != Vector128<byte>.Zero) continue;

                valid = false;
                break;
            }

            return valid;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ContainsAnyExcept256(this ReadOnlySpan<byte> input, byte[] except)
    {
        unsafe
        {
            var exceptPtr = (byte*)except.AsMemory().Pin().Pointer;
            Vector128<byte> vecExcept = Vector128.Load(exceptPtr);

            var valid = true;

            Vector128<byte> equals0;
            Vector128<byte> equals1;
            for (var i = 0; i < input.Length - 2; i += 2)
            {
                Vector128<byte> vecInput0 = Vector128.Create(input[i]);
                Vector128<byte> vecInput1 = Vector128.Create(input[i + 1]);
                equals0 = Vector128.Equals(vecExcept, vecInput0);
                equals1 = Vector128.Equals(vecExcept, vecInput1);
                if (equals0 != Vector128<byte>.Zero && equals1 != Vector128<byte>.Zero) continue;

                valid = false;
                break;
            }
            //todo handle end

            return valid;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ContainsAnyExcept384(this ReadOnlySpan<byte> input, byte[] except)
    {
        unsafe
        {
            var exceptPtr = (byte*)except.AsMemory().Pin().Pointer;
            Vector128<byte> vecExcept = Vector128.Load(exceptPtr);

            var valid = true;

            Vector128<byte> equals0;
            Vector128<byte> equals1;
            Vector128<byte> equals2;
            Vector128<byte> equals3;
            for (var i = 0; i < input.Length - 4; i += 4)
            {
                Vector128<byte> vecInput0 = Vector128.Create(input[i]);
                Vector128<byte> vecInput1 = Vector128.Create(input[i + 1]);
                Vector128<byte> vecInput2 = Vector128.Create(input[i + 2]);
                Vector128<byte> vecInput3 = Vector128.Create(input[i + 3]);
                equals0 = Vector128.Equals(vecExcept, vecInput0);
                equals1 = Vector128.Equals(vecExcept, vecInput1);
                equals2 = Vector128.Equals(vecExcept, vecInput2);
                equals3 = Vector128.Equals(vecExcept, vecInput3);
                if (equals0 != Vector128<byte>.Zero && equals1 != Vector128<byte>.Zero &&
                    equals2 != Vector128<byte>.Zero && equals3 != Vector128<byte>.Zero) continue;

                valid = false;
                break;
            }

            return valid;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ContainsAnyExcept768(this ReadOnlySpan<byte> input, byte[] except)
    {
        unsafe
        {
            var exceptPtr = (byte*)except.AsMemory().Pin().Pointer;
            Vector128<byte> vecExcept = Vector128.Load(exceptPtr);

            var valid = true;

            Vector128<byte> equals0;
            Vector128<byte> equals1;
            Vector128<byte> equals2;
            Vector128<byte> equals3;
            Vector128<byte> equals4;
            Vector128<byte> equals5;
            for (var i = 0; i < input.Length - 6; i += 6)
            {
                Vector128<byte> vecInput0 = Vector128.Create(input[i]);
                Vector128<byte> vecInput1 = Vector128.Create(input[i + 1]);
                Vector128<byte> vecInput2 = Vector128.Create(input[i + 2]);
                Vector128<byte> vecInput3 = Vector128.Create(input[i + 3]);
                Vector128<byte> vecInput4 = Vector128.Create(input[i + 4]);
                Vector128<byte> vecInput5 = Vector128.Create(input[i + 5]);
                equals0 = Vector128.Equals(vecExcept, vecInput0);
                equals1 = Vector128.Equals(vecExcept, vecInput1);
                equals2 = Vector128.Equals(vecExcept, vecInput2);
                equals3 = Vector128.Equals(vecExcept, vecInput3);
                equals4 = Vector128.Equals(vecExcept, vecInput4);
                equals5 = Vector128.Equals(vecExcept, vecInput5);
                if (equals0 != Vector128<byte>.Zero && equals1 != Vector128<byte>.Zero &&
                    equals2 != Vector128<byte>.Zero && equals3 != Vector128<byte>.Zero &&
                    equals4 != Vector128<byte>.Zero && equals5 != Vector128<byte>.Zero) continue;

                valid = false;
                break;
            }

            return valid;
        }
    }
}

public class DnaUtil
{
    private const int ElementCount = 256 * 1_024;
    private static readonly char[] DnaLowerCase = { 'a', 'c', 'g', 't', 'A', 'C', 'G', 'T', '\n', '>' };

    private static readonly char[] DnaLowerCasePad256 =
        { 'a', 'c', 'g', 't', 'A', 'C', 'G', 'T', '\n', '>', 'a', 'c', 'g', 't', 'A', 'C' };

    private static readonly byte[] DnaLowerCaseBytes = "acgtACGT\n"u8.ToArray();
    private static readonly byte[] DnaLowerCaseBytes8 = "cgtACGT\n"u8.ToArray();
    private static readonly byte[] DnaLowerCaseBytesPad128 = "acgtACGT\nacgtACG"u8.ToArray();


    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDna(ReadOnlySpan<char> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCase) < 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaNaive(ReadOnlySpan<char> dnaSeq)
    {
        for (var i = 0; i < dnaSeq.Length; i++)
            if (!DnaLowerCase.Contains(dnaSeq[i]))
                return false;

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaPad256(ReadOnlySpan<char> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCasePad256) < 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaVec64(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.ContainsAnyExcept64(DnaLowerCaseBytes8);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaVec128(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.ContainsAnyExcept(DnaLowerCaseBytes8);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaVec256(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.ContainsAnyExcept256(DnaLowerCaseBytes8);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaVec384(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.ContainsAnyExcept384(DnaLowerCaseBytes8);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaVec768(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.ContainsAnyExcept768(DnaLowerCaseBytes8);
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDna(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCaseBytes) < 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ValidateDnaPad128(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCaseBytesPad128) < 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static async Task<bool> ValidateDnaFromFileAsChar(string path)
    {
        FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        var buffer = new byte[ElementCount];
        var bufferString = new char[ElementCount];
        // if 512 elem, extra 2 * 512 bytes, uses extra 2*N compared to using byte array in ValidateDnaFromFileAsByte

        int read;
        do
        {
            read = await fs.ReadAsync(buffer);
            Encoding.ASCII.GetChars(buffer.AsSpan()[..read], bufferString.AsSpan());
            var valid = ValidateDnaPad256(bufferString.AsSpan()[..read]);
            if (!valid) return false;
        } while (read > 0);

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static async Task<bool> ValidateDnaFromFileAsByte(string path)
    {
        FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        var buffer = new byte[ElementCount];

        int read;
        do
        {
            read = await fs.ReadAsync(buffer);
            var valid = ValidateDnaPad128(buffer.AsSpan()[..read]);
            if (!valid) return false;
        } while (read > 0);

        return true;
    }

    public static async Task<bool> ValidateDnaFromFileAsByteLargerBuffer(string path)
    {
        FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        var buffer = new byte[ElementCount * 8];

        int read;
        do
        {
            read = await fs.ReadAsync(buffer);
            var valid = ValidateDnaPad128(buffer.AsSpan()[..read]);
            if (!valid) return false;
        } while (read > 0);

        return true;
    }
}