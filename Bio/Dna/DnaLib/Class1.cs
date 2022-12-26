using System.Text;

namespace DnaLib;

public class DnaUtil
{
    private const int ElementCount = 256 * 1_024;
    private static readonly char[] DnaLowerCase = { 'a', 'c', 'g', 't', 'A', 'C', 'G', 'T', '\n', '>' };

    private static readonly char[] DnaLowerCasePad256 =
        { 'a', 'c', 'g', 't', 'A', 'C', 'G', 'T', '\n', '>', 'a', 'c', 'g', 't', 'A', 'C' };

    private static readonly byte[] DnaLowerCaseBytes = "acgtACGT\n"u8.ToArray();
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
}