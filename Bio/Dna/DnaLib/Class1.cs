using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace DnaLib;

public static class bla
{
    public const byte ANY = 20;
    public const byte GAP = 21;
    public static unsafe void* proteinLut = "ARNDCQEGHILKMFPSTWYV;;;;;;;;;;;;"u8.ToArray().AsMemory().Pin().Pointer;
    public static unsafe void* proteinLut2 = "XJOUBZ-._;;;;;;;;;;;;;;;;;;;l;;;"u8.ToArray().AsMemory().Pin().Pointer;
    public static readonly unsafe Vector256<byte> vecInput0 = Vector256.Load((byte*)proteinLut);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] CompressSimdInlined(ReadOnlySpan<byte> proteinSeq)
    {
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            // void* proteinLutInner = "ARNDCQEGHILKMFPSTWYV;;;;;;;;;;;;"u8.ToArray().AsMemory().Pin().Pointer;
            Vector256<byte> vecInput0Inner = Vector256.Load((byte*)proteinLut);

            for (var index = 0; index < proteinSeq.Length; index++)
            {
                var protAa = proteinSeq[index];

                Vector256<byte> vecCompare0 = Vector256.Create(protAa);

                // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
                Vector256<byte> eq = Avx2.CompareEqual(vecInput0Inner, vecCompare0);

                // Move comparison results into a bitmap of 32 bits
                var bmp = unchecked((uint)Avx2.MoveMask(eq));
                // Find index of the first byte in the vectors which compared equal
                // The method will return 32 if none of the bytes compared equal
                var firstEqualIndex = BitOperations.TrailingZeroCount(bmp);
                if (firstEqualIndex != 32)
                {
                    results[index] = (byte)firstEqualIndex;
                    continue;
                }

                switch (protAa)
                {
                    case (byte)'X':
                    case (byte)'J':
                    case (byte)'O':
                        firstEqualIndex = ANY;
                        break;
                    case (byte)'U':
                        firstEqualIndex = 4; //Selenocystein -> Cystein
                        break;
                    case (byte)'B':
                        firstEqualIndex = 3; //D (or N)
                        break;
                    case (byte)'Z':
                        firstEqualIndex = 6; //E (or Q)
                        break;
                    case (byte)'-':
                    case (byte)'.':
                    case (byte)'_':
                        firstEqualIndex = GAP;
                        break;
                }

                results[index] = (byte)firstEqualIndex;
            }

            return results;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] CompressSimd(ReadOnlySpan<byte> proteinSeq)
    {
        // ReSharper disable once RedundantUnsafeContext
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            Vector256<byte> vecInput0Inner = Vector256.Load((byte*)proteinLut);

            for (var index = 0; index < proteinSeq.Length; index++)
                results[index] = aa2iSimdLut(proteinSeq[index], vecInput0Inner);

            return results;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] CompressSimdLutStatic(ReadOnlySpan<byte> proteinSeq)
    {
        // ReSharper disable once RedundantUnsafeContext
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            // this temp vecInput0Inner is here so Benchmark.Net gives accurate result, otherwise getting
            // CORINFO_HELP_GETSHARED_NONGCSTATIC_BASE check within inner loop that slows thing down a lot
            Vector256<byte> vecInput0Inner = vecInput0;
            for (var index = 0; index < proteinSeq.Length; index++)
                results[index] = aa2iSimdLut(proteinSeq[index], vecInput0Inner);

            return results;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte aa2iSimd1(byte protAa /*, Vector256<byte> lut*/)
    {
        Vector256<byte> vecCompare0 = Vector256.Create(protAa);

        // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
        // Vector256<byte> eq = Avx2.CompareEqual(lut, vecCompare0);
        Vector256<byte> eq = Avx2.CompareEqual(vecInput0, vecCompare0);
        // Move comparison results into a bitmap of 32 bits

        var bmp = unchecked((uint)Avx2.MoveMask(eq));
        // Find index of the first byte in the vectors which compared equal
        // The method will return 32 if none of the bytes compared equal
        var firstEqualIndex = BitOperations.TrailingZeroCount(bmp);
        if (firstEqualIndex != 32) return (byte)firstEqualIndex;

        switch (protAa)
        {
            case (byte)'X':
            case (byte)'J':
            case (byte)'O':
                firstEqualIndex = ANY;
                break;
            case (byte)'U':
                firstEqualIndex = 4; //Selenocystein -> Cystein
                break;
            case (byte)'B':
                firstEqualIndex = 3; //D (or N)
                break;
            case (byte)'Z':
                firstEqualIndex = 6; //E (or Q)
                break;
            case (byte)'-':
            case (byte)'.':
            case (byte)'_':
                firstEqualIndex = GAP;
                break;
        }

        return (byte)firstEqualIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte aa2iSimdLut(byte protAa, Vector256<byte> lut)
    {
        Vector256<byte> vecCompare0 = Vector256.Create(protAa);

        // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
        Vector256<byte> eq = Avx2.CompareEqual(lut, vecCompare0);
        // Move comparison results into a bitmap of 32 bits

        var bmp = unchecked((uint)Avx2.MoveMask(eq));
        // Find index of the first byte in the vectors which compared equal
        // The method will return 32 if none of the bytes compared equal
        var firstEqualIndex = BitOperations.TrailingZeroCount(bmp);
        if (firstEqualIndex != 32) return (byte)firstEqualIndex;

        switch (protAa)
        {
            case (byte)'X':
            case (byte)'J':
            case (byte)'O':
                firstEqualIndex = ANY;
                break;
            case (byte)'U':
                firstEqualIndex = 4; //Selenocystein -> Cystein
                break;
            case (byte)'B':
                firstEqualIndex = 3; //D (or N)
                break;
            case (byte)'Z':
                firstEqualIndex = 6; //E (or Q)
                break;
            case (byte)'-':
            case (byte)'.':
            case (byte)'_':
                firstEqualIndex = GAP;
                break;
        }

        return (byte)firstEqualIndex;
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

    public static byte[] CompressInlined(ReadOnlySpan<byte> proteinSeq)
    {
        var results = new byte[proteinSeq.Length];

        for (var index = 0; index < proteinSeq.Length; index++)
        {
            var protAa = proteinSeq[index];
            switch (protAa)
            {
                case (byte)'A':
                    results[index] = 0;
                    break;
                case (byte)'R':
                    results[index] = 1;
                    break;
                case (byte)'N':
                    results[index] = 2;
                    break;
                case (byte)'D':
                    results[index] = 3;
                    break;
                case (byte)'C':
                    results[index] = 4;
                    break;
                case (byte)'Q':
                    results[index] = 5;
                    break;
                case (byte)'E':
                    results[index] = 6;
                    break;
                case (byte)'G':
                    results[index] = 7;
                    break;
                case (byte)'H':
                    results[index] = 8;
                    break;
                case (byte)'I':
                    results[index] = 9;
                    break;
                case (byte)'L':
                    results[index] = 10;
                    break;
                case (byte)'K':
                    results[index] = 11;
                    break;
                case (byte)'M':
                    results[index] = 12;
                    break;
                case (byte)'F':
                    results[index] = 13;
                    break;
                case (byte)'P':
                    results[index] = 14;
                    break;
                case (byte)'S':
                    results[index] = 15;
                    break;
                case (byte)'T':
                    results[index] = 16;
                    break;
                case (byte)'W':
                    results[index] = 17;
                    break;
                case (byte)'Y':
                    results[index] = 18;
                    break;
                case (byte)'V':
                    results[index] = 19;
                    break;
                case (byte)'X':
                    results[index] = ANY;
                    break;
                case (byte)'J':
                    results[index] = ANY;
                    break;
                case (byte)'O':
                    results[index] = ANY;
                    break;
                case (byte)'U':
                    results[index] = 4; //Selenocystein -> Cystein
                    break;
                case (byte)'B':
                    results[index] = 3; //D (or N)
                    break;
                case (byte)'Z':
                    results[index] = 6; //E (or Q)
                    break;
                case (byte)'-':
                    results[index] = GAP;
                    break;
                case (byte)'.':
                    results[index] = GAP;
                    break;
                case (byte)'_':
                    results[index] = GAP;
                    break;
                default:
                    results[index] = 31;
                    break;
            }
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