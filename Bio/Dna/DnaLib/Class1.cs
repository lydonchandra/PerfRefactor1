using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Text;

namespace DnaLib;

public static class bla
{
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public static bool ContainsAnyExcept(this ReadOnlySpan<byte> input, byte[] excepts)
    {
        unsafe
        {
            var dnaLowerCaseBytes = (byte*)excepts.AsMemory().Pin().Pointer;
            var valid = true;

            //todo: investigate why Vector<64> is super slow
            Vector128<byte> vec1A = Vector128.LoadUnsafe(ref *dnaLowerCaseBytes);
            // Vector64.Load(ref excepts);
            // Vector64<byte> vec2A = Vector64.LoadUnsafe(ref *dnaLowerCaseBytes);
            // Vector64<byte> vec3A = Vector64.LoadUnsafe(ref *dnaLowerCaseBytes);
            // Vector64<byte> vec4A = Vector64.LoadUnsafe(ref *dnaLowerCaseBytes);
            // Vector64<byte> vec2A = Vector64.Load(dnaLowerCaseBytes);
            // Vector64<byte> vec3A = Vector64.Load(dnaLowerCaseBytes);
            // Vector64<byte> vec4A = Vector64.Load(dnaLowerCaseBytes);

            // Vector256<byte> vec256ExceptArg = Vector256.Create(
            //     Vector128.Create(vec1A, vec2A),
            //     Vector128.Create(vec3A, vec4A)
            // );

            var len = input.Length;
            Vector128<byte> equals;
            // for (var i = 0; i < input.Length - 4; i += 4)
            for (var i = 0; i < len; i++)
            {
                // var bla = Unsafe.As<byte>(input[i]);
                Vector128<byte> vec1B = Vector128.Create(input[i]);
                equals = Vector128.Equals(vec1A, vec1B);
                if (equals != Vector128<byte>.Zero) continue;

                valid = false;
                break;
                // Vector64<byte> vec2B = Vector64.Create(input[i + 1]);
                // Vector64<byte> vec3B = Vector64.Create(input[i + 2]);
                // Vector64<byte> vec4B = Vector64.Create(input[i + 3]);

                // Vector256<byte> vec256dnaSeq = Vector256.Create(
                //     Vector128.Create(Vector64.Create(input[i]), Vector64.Create(input[i + 1])),
                //     Vector128.Create(Vector64.Create(input[i + 2]), Vector64.Create(input[i + 3]))
                // );

                // Vector256<byte> result = Vector256.Equals(vec256dnaSeq, vec256ExceptArg);
                // Vector128<byte> result128Lower = result.GetLower();
                // Vector128<byte> result128Upper = result.GetUpper();

                // if (
                //     Vector64.Equals(result128Lower.GetLower(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
                //     Vector64.Equals(result128Lower.GetUpper(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
                //     Vector64.Equals(result128Upper.GetLower(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet ||
                //     Vector64.Equals(result128Upper.GetUpper(), Vector64<byte>.Zero) == Vector64<byte>.AllBitsSet)
                // {
                //     valid = false;
                //     break;
                // }
            }

            //todo: handle remainder if % 4 is not zero
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


    public static bool ValidateDna(ReadOnlySpan<char> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCase) < 0;
    }

    public static bool ValidateDnaNaive(ReadOnlySpan<char> dnaSeq)
    {
        for (var i = 0; i < dnaSeq.Length; i++)
            if (!DnaLowerCase.Contains(dnaSeq[i]))
                return false;

        return true;
    }

    public static bool ValidateDnaPad256(ReadOnlySpan<char> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCasePad256) < 0;
    }

    public static bool ValidateDnaVec256(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.ContainsAnyExcept(DnaLowerCaseBytes8);
    }

    public static bool ValidateDna(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCaseBytes) < 0;
    }

    public static bool ValidateDnaPad128(ReadOnlySpan<byte> dnaSeq)
    {
        return dnaSeq.IndexOfAnyExcept(DnaLowerCaseBytesPad128) < 0;
    }

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