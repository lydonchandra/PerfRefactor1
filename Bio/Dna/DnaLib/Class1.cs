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

    private const int x = 32;

    public static unsafe void* proteinLutUpper =
        "ARNDCQEGHILKMFPSTWYV;;;;;;;;;;;;"u8.ToArray().AsMemory().Pin().Pointer;

    public static unsafe void* proteinLutLower =
        "arndcqeghilkmfpstwyv;;;;;;;;;;;;"u8.ToArray().AsMemory().Pin().Pointer;

    public static readonly unsafe Vector256<byte> vecInput0 = Vector256.Load((byte*)proteinLutUpper);

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] CompressSimdInlined(ReadOnlySpan<byte> proteinSeq)
    {
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            Vector256<byte> vecInput0Inner = Vector256.Load((byte*)proteinLutUpper);
            Vector256<byte> eq = new();
            Vector256<byte> vecCompare0 = new();
            const byte x = 32;

            for (var index = 0; index < proteinSeq.Length; index++)
            {
                var protAa = proteinSeq[index];
                if (protAa >= (byte)'a') protAa = (byte)(protAa & ~x);
                vecCompare0 = Vector256.Create(protAa);

                // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
                eq = Avx2.CompareEqual(vecInput0Inner, vecCompare0);

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

                byte result;
                switch (protAa)
                {
                    case (byte)'X':
                    case (byte)'J':
                    case (byte)'O':
                        result = ANY;
                        break;
                    case (byte)'U':
                        result = 4; //Selenocystein -> Cystein
                        break;
                    case (byte)'B':
                        result = 3; //D (or N)
                        break;
                    case (byte)'Z':
                        result = 6; //E (or Q)
                        break;
                    case (byte)'-':
                    case (byte)'.':
                    case (byte)'_':
                        result = GAP;
                        break;
                    default:
                        if (protAa is >= 0 and <= 32)
                            result = unchecked((byte)-1);
                        else
                            result = unchecked((byte)-2);
                        break;
                }

                results[index] = result;
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
            Vector256<byte> vecInput0Inner = Vector256.Load((byte*)proteinLutUpper);
            for (var index = 0; index < proteinSeq.Length; index++)
                results[index] = aa2iSimd(proteinSeq[index], vecInput0Inner);

            return results;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static byte[] CompressSimdNoIf(ReadOnlySpan<byte> proteinSeq)
    {
        // ReSharper disable once RedundantUnsafeContext
        unsafe
        {
            var results = new byte[proteinSeq.Length];
            Vector256<byte> vecInput0Upper = Vector256.Load((byte*)proteinLutUpper);
            Vector256<byte> vecInput0Lower = Vector256.Load((byte*)proteinLutLower);
            for (var index = 0; index < proteinSeq.Length; index++)
                results[index] = aa2iSimdNoIf(proteinSeq[index], vecInput0Upper, vecInput0Lower);

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

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte aa2iSimd(byte protAa, Vector256<byte> vecInput1)
    {
        // if (protAa is >= (byte)'a' and <= (byte)'z') protAa = (byte)(protAa + (byte)'A' - (byte)'a');
        if (protAa >= (byte)'a') protAa = (byte)(protAa & ~x);

        Vector256<byte> vecProtAa = Vector256.Create(protAa);

        // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
        Vector256<byte> eq = Avx2.CompareEqual(vecInput1, vecProtAa);
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
            default:
                if (protAa is >= 0 and <= 32)
                    firstEqualIndex = unchecked((byte)-1);
                else
                    firstEqualIndex = unchecked((byte)-2);
                break;
        }

        return (byte)firstEqualIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte aa2iSimdNoIf(byte protAa, Vector256<byte> vecUpper, Vector256<byte> vecLower)
    {
        Vector256<byte> vecProtAa = Vector256.Create(protAa);

        // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
        Vector256<byte> eqUpper = Avx2.CompareEqual(vecUpper, vecProtAa);
        Vector256<byte> eqLower = Avx2.CompareEqual(vecLower, vecProtAa);
        Vector256<byte> eq = Vector256.BitwiseOr(eqUpper, eqLower);
        // Move comparison results into a bitmap of 32 bits

        var bmp = unchecked((uint)Avx2.MoveMask(eq));
        // Find index of the first byte in the vectors which compared equal
        // The method will return 32 if none of the bytes compared equal
        var firstEqualIndex = BitOperations.TrailingZeroCount(bmp);
        if (firstEqualIndex != 32) return (byte)firstEqualIndex;

        switch (protAa)
        {
            case (byte)'X':
            case (byte)'x':
            case (byte)'J':
            case (byte)'j':
            case (byte)'O':
            case (byte)'o':
                firstEqualIndex = ANY;
                break;
            case (byte)'U':
            case (byte)'u':
                firstEqualIndex = 4; //Selenocystein -> Cystein
                break;
            case (byte)'B':
            case (byte)'b':
                firstEqualIndex = 3; //D (or N)
                break;
            case (byte)'Z':
            case (byte)'z':
                firstEqualIndex = 6; //E (or Q)
                break;
            case (byte)'-':
            case (byte)'.':
            case (byte)'_':
                firstEqualIndex = GAP;
                break;
            default:
                if (protAa is >= 0 and <= 32)
                    firstEqualIndex = unchecked((byte)-1);
                else
                    firstEqualIndex = unchecked((byte)-2);
                break;
        }

        return (byte)firstEqualIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static Span<sbyte> CompressSimdHarold(ReadOnlySpan<byte> protAaSeq)
    {
        // ReadOnlySpan<sbyte> a = MemoryMarshal.Cast<byte, sbyte>(protAaSeq);
        var outRes = new sbyte[protAaSeq.Length];

        var i = 0;
        var N = protAaSeq.Length;

        // sbyte dash = 95;
        // sbyte caret = 94;
        // sbyte atSymbol = 64;
        // sbyte backtick = 96;
        // // sbyte dash = 81;
        // sbyte dot = 125;
        do
        {
            unsafe
            {
                Vector256<sbyte> LUT1 = Vector256.Create(
                    //@ A  B  C  D  E   F  G  H  I    J         K   L   M   N    O
                    -2, 0, 3, 4, 3, 6, 13, 7, 8, 9, (sbyte)ANY, 11, 10, 12, 2, (sbyte)ANY,
                    -2, 0, 3, 4, 3, 6, 13, 7, 8, 9, (sbyte)ANY, 11, 10, 12, 2, (sbyte)ANY);

                Vector256<sbyte> LUT2 = Vector256.Create(
                    //P Q  R   S   T  U   V   W    X         Y   Z   [   \   ]   ^    _
                    14, 5, 1, 15, 16, 4, 19, 17, (sbyte)ANY, 18, 6, -2, -2, -2, -2, (sbyte)GAP,
                    14, 5, 1, 15, 16, 4, 19, 17, (sbyte)ANY, 18, 6, -2, -2, -2, -2, (sbyte)GAP);

                var outPtr = (sbyte*)outRes.AsMemory().Pin().Pointer;
                var protPtr = (sbyte*)protAaSeq.ToArray().AsMemory().Pin().Pointer;

                for (; i + 31 < N; i += 32)
                {
                    Vector256<sbyte> data = Vector256.Load(protPtr + i);
                    Vector256<sbyte> is_above_ws = Vector256.GreaterThan(
                        Vector256.Add(data,
                            Vector256.Create((sbyte)95)), //0x5F
                        Vector256.Create((sbyte)94)
                    );

                    Vector256<sbyte> is_control = Vector256.GreaterThan(
                        Vector256.Add(data,
                            Vector256.Create((sbyte)64)),
                        Vector256.Create((sbyte)94));

                    Vector256<sbyte> is_dash_or_dot = Vector256.GreaterThan(
                        Vector256.Add(data,
                            Vector256.Create((sbyte)81)),
                        Vector256.Create((sbyte)125));

                    Vector256<sbyte> is_not_a_to_z_lower = Vector256.GreaterThan(
                        Vector256.Subtract(data,
                            Vector256.Create((sbyte)((byte)'`' - 128))),
                        Vector256.Create((sbyte)26));

                    Vector256<sbyte> lowercase =
                        Vector256.Xor(data,
                            Avx2.AndNot(is_not_a_to_z_lower, Vector256.Create((sbyte)0x20)));

                    Vector256<sbyte> rangeA = Vector256.Subtract(lowercase, Vector256.Create((sbyte)'@'));
                    Vector256<sbyte> partA = Avx2.Shuffle(
                        LUT1,
                        Avx2.AddSaturate(rangeA.AsByte(), Vector256.Create((byte)0x70)).AsSByte());

                    Vector256<sbyte> rangeB = Avx2.Subtract(lowercase, Vector256.Create((sbyte)'P'));
                    Vector256<sbyte> partB = Avx2.Shuffle(
                        LUT2,
                        Avx2.AddSaturate(rangeB.AsByte(), Vector256.Create((byte)0x70)).AsSByte());


                    Vector256<sbyte> res = Vector256.BitwiseOr(partA, partB);
                    res = Avx2.BlendVariable(
                        res,
                        Vector256.Create((sbyte)-2),
                        Vector256.Add(data, Vector256.Create((sbyte)1)));

                    res = Vector256.BitwiseOr(
                        res,
                        Vector256.BitwiseOr(
                            is_above_ws,
                            Vector256.BitwiseAnd(is_control, Vector256.Create((sbyte)-2))));

                    res = Avx2.BlendVariable(res, Vector256.Create((sbyte)GAP), is_dash_or_dot);
                    res.Store(outPtr + i);
                }

                if (i < N && N >= 32)
                    i = N - 32;
                else
                    break;
            }
        } while (true);

        return outRes;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static byte aa2iSimdLut(byte protAa, Vector256<byte> lut)
    {
        // if (protAa is >= (byte)'a' and <= (byte)'z') protAa = (byte)(protAa + (byte)'A' - (byte)'a');
        Vector256<byte> vecProtAa = Vector256.Create(protAa);

        // Compare bytes for equality, equal = 0xFF = 255, not equal = 0
        Vector256<byte> eq = Avx2.CompareEqual(lut, vecProtAa);
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
            default:
                if (protAa is >= 0 and <= 32)
                    firstEqualIndex = unchecked((byte)-1);
                else
                    firstEqualIndex = unchecked((byte)-2);
                break;
        }

        return (byte)firstEqualIndex;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static byte aa2i(byte a)
    {
        if (a is >= (byte)'a' and <= (byte)'z') a = (byte)(a + (byte)'A' - (byte)'a');

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
            default:
                if (a is >= 0 and <= 32)
                    return unchecked((byte)-1);
                return unchecked((byte)-2);
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