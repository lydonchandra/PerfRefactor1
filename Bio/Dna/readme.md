# Note: DOTNET_JitDisasm=ValidateDnaVec256, no quotes

Doing the following loop unrolling is ~20% faster,
and as expected adding another Vector128<byte>, making it 3, slow things down as it does not fit into CPU registers

```
[MethodImpl(MethodImplOptions.AggressiveOptimization)]
public static bool ContainsAnyExcept256(this ReadOnlySpan<byte> input, byte[] except)
{
unsafe
{
var exceptPtr = (byte*)except.AsMemory().Pin().Pointer;
Vector128<byte> vecExcept = Vector128.Load(exceptPtr);

            var valid = true;

            Vector128<byte> equals;
            Vector128<byte> equals2;
            for (var i = 0; i < input.Length - 1; i += 2)
            {
                Vector128<byte> vecInput0 = Vector128.Create(input[i]);
                Vector128<byte> vecInput1 = Vector128.Create(input[i + 1]);
                // Vector256<byte> vecInput = Vector256.Create(Vector128.Create(input[i]), Vector128.Create(input[i + 1]));
                equals = Vector128.Equals(vecExcept, vecInput0);
                equals2 = Vector128.Equals(vecExcept, vecInput1);
                if (equals != Vector128<byte>.Zero && equals2 != Vector128<byte>.Zero) continue;

                valid = false;
                break;
            }

            return valid;
        }
    }
```    

| Method                          | dataSize |         Mean |    StdDev | Allocated |
|---------------------------------|----------|-------------:|----------:|----------:|
| ValidateDnaContainsAnyExcept256 | sm       |     16.96 us |  0.067 us |      80 B |
| ValidateDnaContainsAnyExcept128 | sm       |     19.90 us |  0.082 us |      80 B |
| ValidateDnaBytePad128           | sm       |    117.31 us |  0.320 us |      80 B |
| ValidateDnaContainsAnyExcept256 | lg       |    502.64 us |  0.294 us |      81 B |
| ValidateDnaContainsAnyExcept128 | lg       |    590.56 us |  1.575 us |      81 B |
| ValidateDnaBytePad128           | lg       |  3,503.59 us | 16.048 us |      84 B |
| ValidateDnaContainsAnyExcept256 | xl       | 10,450.72 us | 10.798 us |      98 B |
| ValidateDnaContainsAnyExcept128 | xl       | 12,142.86 us | 63.175 us |      98 B |
| ValidateDnaBytePad128           | xl       | 70,178.76 us | 94.641 us |     245 B |

## Linux-Arm64 RaspberryPi4B

| Method                |     Mean |   StdDev | Ratio | Allocated | Alloc Ratio |
|-----------------------|---------:|---------:|------:|----------:|------------:|
| ValidateDnaChar       | 12.08 ms | 0.042 ms |  1.00 |      11 B |        1.00 |
| ValidateDnaCharPad256 | 12.10 ms | 0.034 ms |  1.00 |      11 B |        1.00 |
| ValidateDnaCharNaive  | 30.58 ms | 0.034 ms |  2.53 |      22 B |        2.00 |

## Linux-x64

BenchmarkDotNet=v0.13.2, OS=ubuntu 20.04
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.100
[Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
Job-TUVSKM : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

IterationCount=3 LaunchCount=2 WarmupCount=2

| Method                |     Mean |    StdDev | Ratio | Allocated | Alloc Ratio |
|-----------------------|---------:|----------:|------:|----------:|------------:|
| ValidateDnaBytePad128 | 3.499 ms | 0.0170 ms |  0.93 |       3 B |        1.00 |
| ValidateDnaCharPad256 | 3.513 ms | 0.0071 ms |  0.93 |       3 B |        1.00 |
| ValidateDnaChar       | 3.783 ms | 0.0401 ms |  1.00 |       3 B |        1.00 |
| ValidateDnaCharNaive  | 7.421 ms | 0.0247 ms |  1.96 |       6 B |        2.00 |
| ValidateDnaByte       | 8.617 ms | 0.0470 ms |  2.28 |      11 B |        3.67 |

| Method                    | dataSize |        Mean |   StdDev | Ratio |     Gen0 |     Gen1 |     Gen2 | Allocated | Alloc Ratio |
|---------------------------|----------|------------:|---------:|------:|---------:|---------:|---------:|----------:|------------:|
| ValidateDnaFromFileAsByte | sm       |    181.0 us |  0.13 us |  0.74 |  83.2520 |  83.2520 |  83.2520 | 257.57 KB |        0.33 |
| ValidateDnaFromFileAsChar | sm       |    246.2 us |  3.05 us |  1.00 | 249.5117 | 249.5117 | 249.5117 | 769.82 KB |        1.00 |
|                           |          |             |          |       |          |          |          |           |             |
| ValidateDnaFromFileAsByte | lg       |  3,765.7 us |  0.93 us |  0.93 |  82.0313 |  82.0313 |  82.0313 | 257.57 KB |        0.33 |
| ValidateDnaFromFileAsChar | lg       |  4,067.0 us | 12.04 us |  1.00 | 242.1875 | 242.1875 | 242.1875 | 769.83 KB |        1.00 |
|                           |          |             |          |       |          |          |          |           |             |
| ValidateDnaFromFileAsByte | xl       | 72,831.4 us | 34.52 us |  0.97 |        - |        - |        - | 257.67 KB |        0.33 |
| ValidateDnaFromFileAsChar | xl       | 74,754.7 us | 67.88 us |  1.00 | 142.8571 | 142.8571 | 142.8571 | 769.89 KB |        1.00 |

## Notes

* adding `[MethodImpl(MethodImplOptions.AggressiveInlining)]` into `ValidateDnaPad256` has no measurable effect.
* Making the buffer larger, to 2MB, slow things down, for very small input, as expected


