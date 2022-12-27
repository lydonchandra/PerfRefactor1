using System.Diagnostics;
using System.Runtime.Intrinsics;

bool ContainsAnyExcept(ReadOnlySpan<byte> input, byte[] excepts)
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

unsafe
{
    void* dnaLowerCaseBytes = "acgtACGT"u8.ToArray().AsMemory().Pin().Pointer;
    var dnaSeq = "acgtacgtacgtacgtacgtacgtacgtacgt"u8.ToArray();
    var valid = true;

    Vector64<byte> vec1A = Vector64.Load((byte*)dnaLowerCaseBytes);
    Vector64<byte> vec2A = Vector64.Load((byte*)dnaLowerCaseBytes);
    Vector64<byte> vec3A = Vector64.Load((byte*)dnaLowerCaseBytes);
    Vector64<byte> vec4A = Vector64.Load((byte*)dnaLowerCaseBytes);

    Vector256<byte> vec256ExceptArg = Vector256.Create(
        Vector128.Create(vec1A, vec2A),
        Vector128.Create(vec3A, vec4A)
    );

    for (var i = 0; i < dnaSeq.Length; i += 4)
    {
        Vector64<byte> vec1B = Vector64.Create(dnaSeq[i]);
        Vector64<byte> vec2B = Vector64.Create(dnaSeq[i + 1]);
        Vector64<byte> vec3B = Vector64.Create(dnaSeq[i + 2]);
        Vector64<byte> vec4B = Vector64.Create(dnaSeq[i + 3]);

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

    Debug.Assert(valid);
}