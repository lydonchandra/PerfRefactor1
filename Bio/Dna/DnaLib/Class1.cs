using System.Text;

namespace DnaLib;

public class DnaUtil
{
    private const int BufferSize = 256 * 1_024;
    private static readonly char[] DnaLowerCase = { 'a', 'c', 'g', 't', 'A', 'C', 'G', 'T', '\n', '>' };
    private static readonly byte[] DnaLowerCaseBytes = "acgtACGT\n>"u8.ToArray();

    public static bool ValidateDna(ReadOnlySpan<char> dnaSeq)
    {
        var isValid = dnaSeq.IndexOfAnyExcept(DnaLowerCase) < 0;
        return isValid;
    }

    public static bool ValidateDna(ReadOnlySpan<byte> dnaSeq)
    {
        var isValid = dnaSeq.IndexOfAnyExcept(DnaLowerCaseBytes) < 0;
        return isValid;
    }

    public static async Task<bool> ValidateDnaFromFileAsChar(string path)
    {
        FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        var buffer = new byte[BufferSize];
        var bufferString = new char[BufferSize];

        int read;
        do
        {
            read = await fs.ReadAsync(buffer);
            Encoding.ASCII.GetChars(buffer.AsSpan()[..read], bufferString.AsSpan());
            var valid = ValidateDna(bufferString.AsSpan()[..read]);
            if (!valid) return false;
        } while (read > 0);

        return true;
    }

    public static async Task<bool> ValidateDnaFromFileAsByte(string path)
    {
        FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        var buffer = new byte[BufferSize];

        int read;
        do
        {
            read = await fs.ReadAsync(buffer);
            var valid = ValidateDna(buffer.AsSpan()[..read]);
            if (!valid) return false;
        } while (read > 0);

        return true;
    }
}