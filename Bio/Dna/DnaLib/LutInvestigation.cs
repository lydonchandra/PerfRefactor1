using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace DnaLib;

public class C
{
    public const byte ANY = 20;
    public const byte GAP = 21;
    public readonly Vector256<byte> vecInput0; // = Vector256.Load((byte*)proteinLut);
    public unsafe void* proteinLut = "ARNDCQEGHILKMFPSTWYV;;;;;;;;;;;;"u8.ToArray().AsMemory().Pin().Pointer;
    public unsafe void* proteinLut2 = "XJOUBZ-._;;;;;;;;;;;;;;;;;;;l;;;"u8.ToArray().AsMemory().Pin().Pointer;

    public C()
    {
        unsafe
        {
            vecInput0 = Vector256.Load((byte*)proteinLut);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public byte[] CompressSimd(ReadOnlySpan<byte> proteinSeq)
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

    // [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    // public byte[] CompressSimdLutStatic(ReadOnlySpan<byte> proteinSeq)
    // {
    //     // ReSharper disable once RedundantUnsafeContext
    //     unsafe
    //     {
    //         var results = new byte[proteinSeq.Length];
    //         for (var index = 0; index < proteinSeq.Length; index++)
    //         {
    //             var protAa = proteinSeq[index];
    //             results[index] = aa2iSimdLut(protAa, vecInput0);
    //         }
    //
    //         return results;
    //     }
    // }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public byte aa2iSimdLut(byte protAa, Vector256<byte> lut)
    {
        // if (protAa is >= (byte)'a' and <= (byte)'z') protAa = (byte)(protAa + (byte)'A' - (byte)'a');

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
            // default:
            //     if (protAa is >= 0 and <= 32)
            //         firstEqualIndex = unchecked((byte)-1);
            //     else
            //         firstEqualIndex = unchecked((byte)-2);
            //     break;
        }

        return (byte)firstEqualIndex;
    }
}