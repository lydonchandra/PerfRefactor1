# Protein compression

## Added CompressSimdHarold

 ### Slow Data Scenario

Data looks like this `_xjoubzxjoubz-._xjoubz-._xjoubz` repeated up to `kb100` or `kbXXX` dataSize

| Method             | dataSize |        Mean |    StdDev | Ratio |     Gen0 |     Gen1 |     Gen2 |   Allocated | Alloc Ratio |
|--------------------|----------|------------:|----------:|------:|---------:|---------:|---------:|------------:|------------:|
| CompressSimdHarold | kb100    |    111.5 us |   1.71 us |  0.42 |        - |        - |        - |   293.13 KB |        3.00 |
| CompressSimd       | kb100    |    268.1 us |   5.37 us |  1.00 |  30.7617 |  30.7617 |  30.7617 |    97.83 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb200    |    212.1 us |   1.41 us |  0.43 |        - |        - |        - |   586.07 KB |        3.00 |
| CompressSimd       | kb200    |    492.2 us |   0.83 us |  1.00 |  61.5234 |  61.5234 |  61.5234 |   195.52 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb400    |    416.0 us |   0.51 us |  0.42 |        - |        - |        - |  1171.94 KB |        3.00 |
| CompressSimd       | kb400    |    983.3 us |   1.25 us |  1.00 | 123.0469 | 123.0469 | 123.0469 |   390.91 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb800    |    830.8 us |   0.05 us |  0.41 |        - |        - |        - |   2343.7 KB |        3.00 |
| CompressSimd       | kb800    |  2,016.7 us |  31.32 us |  1.00 | 195.3125 | 195.3125 | 195.3125 |    781.6 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb2000   |  2,317.6 us |   8.57 us |  0.46 |        - |        - |        - |  5858.97 KB |        3.00 |
| CompressSimd       | kb2000   |  5,075.2 us |   8.84 us |  1.00 | 320.3125 | 320.3125 | 320.3125 |  1953.54 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb4000   |  4,632.4 us |  44.61 us |  0.45 |        - |        - |        - | 11717.77 KB |        3.00 |
| CompressSimd       | kb4000   | 10,376.7 us | 397.22 us |  1.00 | 296.8750 | 296.8750 | 296.8750 |  3906.46 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb5000   |  5,826.3 us |   8.34 us |  0.46 |        - |        - |        - | 14647.18 KB |        3.00 |
| CompressSimd       | kb5000   | 12,640.1 us | 219.85 us |  1.00 | 296.8750 | 296.8750 | 296.8750 |  4882.93 KB |        1.00 |

### Typical Data Scenario

Data looks like `MTTQAPTFTQPLQSVVVLEGSTATFEAHISGFPVPEVSWFRDGQVISTSTLPGVQ` repeated up to `kb100` or `kbXXX` dataSize

| Method             | dataSize |        Mean |    StdDev | Ratio |     Gen0 |     Gen1 |     Gen2 |   Allocated | Alloc Ratio |
|--------------------|----------|------------:|----------:|------:|---------:|---------:|---------:|------------:|------------:|
| CompressSimd       | kb100    |    95.25 us |  0.181 us |  1.00 |  32.2266 |  32.2266 |  32.2266 |   100.81 KB |        1.00 |
| CompressSimdHarold | kb100    |   111.73 us |  1.153 us |  1.17 |        - |        - |        - |   302.09 KB |        3.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimd       | kb200    |   189.16 us |  0.080 us |  1.00 |  62.2559 |  62.2559 |  62.2559 |   201.49 KB |        1.00 |
| CompressSimdHarold | kb200    |   210.98 us |  1.342 us |  1.12 |        - |        - |        - |   603.98 KB |        3.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimd       | kb400    |   382.53 us |  1.319 us |  1.00 | 124.5117 | 124.5117 | 124.5117 |   402.84 KB |        1.00 |
| CompressSimdHarold | kb400    |   412.29 us |  0.749 us |  1.08 |        - |        - |        - |  1207.78 KB |        3.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimd       | kb800    |   795.75 us |  1.572 us |  1.00 | 199.2188 | 199.2188 | 199.2188 |   805.49 KB |        1.00 |
| CompressSimdHarold | kb800    |   812.47 us |  0.119 us |  1.02 |        - |        - |        - |   2415.4 KB |        3.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimd       | kb2000   | 2,047.14 us |  6.058 us |  1.00 | 324.2188 | 324.2188 | 324.2188 |   2013.3 KB |        1.00 |
| CompressSimdHarold | kb2000   | 2,242.26 us |  9.284 us |  1.10 |        - |        - |        - |  6038.26 KB |        3.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimdHarold | kb4000   | 4,536.82 us |  5.494 us |  0.98 |        - |        - |        - | 12076.35 KB |        3.00 |
| CompressSimd       | kb4000   | 4,640.09 us | 23.321 us |  1.00 | 156.2500 | 156.2500 | 156.2500 |  4025.75 KB |        1.00 |
|                    |          |             |           |       |          |          |          |             |             |
| CompressSimd       | kb5000   | 5,536.27 us | 74.628 us |  1.00 | 156.2500 | 156.2500 | 156.2500 |  5032.11 KB |        1.00 |
| CompressSimdHarold | kb5000   | 5,677.87 us | 14.282 us |  1.03 |        - |        - |        - |  15095.4 KB |        3.00 |

## Regular vs SIMD

IterationCount=2 LaunchCount=1 WarmupCount=1  
Error=NA

| Method        | dataSize |       Mean |     StdDev | Ratio |    Gen0 |    Gen1 |    Gen2 | Allocated | Alloc Ratio |
|---------------|----------|-----------:|-----------:|------:|--------:|--------:|--------:|----------:|------------:|
| CompressSimd1 | sm       |   4.899 us |  0.0311 us |  0.60 |  1.1292 |  0.6714 |       - |   5.13 KB |        1.01 |
| Compress      | sm       |   8.229 us |  0.0035 us |  1.00 |  0.8240 |  0.0153 |       - |   5.08 KB |        1.00 |
|               |          |            |            |       |         |         |         |           |             |
| CompressSimd1 | md       | 210.127 us | 34.1023 us |  0.63 | 62.2559 | 62.2559 | 62.2559 | 203.95 KB |        1.00 |
| Compress      | md       | 333.931 us |  0.4218 us |  1.00 | 62.0117 | 62.0117 | 62.0117 |  203.9 KB |        1.00 |

# TLDR arm64

arm64 has Vector64, so `ValidateDnaContainsAnyExcept64` runs on par with the other vectorized methods.

| Method                          | dataSize |         Mean |    StdDev | Ratio | Allocated | Alloc Ratio |
|---------------------------------|----------|-------------:|----------:|------:|----------:|------------:|
| ValidateDnaContainsAnyExcept384 | sm       |     102.9 us |   0.07 us |  0.92 |      80 B |        1.00 |
| ValidateDnaContainsAnyExcept256 | sm       |     112.1 us |   0.11 us |  1.00 |      80 B |        1.00 |
| ValidateDnaContainsAnyExcept64  | sm       |     129.5 us |   0.49 us |  1.16 |      80 B |        1.00 |
| ValidateDnaContainsAnyExcept128 | sm       |     133.1 us |   0.18 us |  1.19 |      80 B |        1.00 |
| ValidateDnaBytePad128           | sm       |     306.1 us |  13.17 us |  2.73 |      81 B |        1.01 |
|                                 |          |              |           |       |           |             |
| ValidateDnaContainsAnyExcept384 | lg       |   3,060.7 us |   0.74 us |  0.91 |      84 B |        1.00 |
| ValidateDnaContainsAnyExcept256 | lg       |   3,378.7 us |   2.13 us |  1.00 |      84 B |        1.00 |
| ValidateDnaContainsAnyExcept64  | lg       |   3,746.1 us |  10.65 us |  1.11 |      84 B |        1.00 |
| ValidateDnaContainsAnyExcept128 | lg       |   3,906.8 us |  96.38 us |  1.16 |      89 B |        1.06 |
| ValidateDnaBytePad128           | lg       |   8,679.4 us |  10.30 us |  2.57 |      98 B |        1.17 |
|                                 |          |              |           |       |           |             |
| ValidateDnaContainsAnyExcept384 | xl       |  61,017.6 us |  16.25 us |  0.90 |     208 B |        0.93 |
| ValidateDnaContainsAnyExcept256 | xl       |  67,790.2 us | 642.52 us |  1.00 |     224 B |        1.00 |
| ValidateDnaContainsAnyExcept64  | xl       |  74,751.6 us |  60.58 us |  1.10 |     245 B |        1.09 |
| ValidateDnaContainsAnyExcept128 | xl       |  76,254.2 us | 299.19 us |  1.12 |     245 B |        1.09 |
| ValidateDnaBytePad128           | xl       | 173,407.6 us |  91.71 us |  2.56 |     464 B |        2.07 |

# TLDR x64

BenchmarkDotNet=v0.13.2, OS=ubuntu 20.04
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.100
[Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
Job-CLMXJB : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

IterationCount=2 LaunchCount=1 WarmupCount=1  
Error=NA

| Method                          | dataSize |          Mean |       StdDev | Ratio | Allocated | Alloc Ratio |
|---------------------------------|----------|--------------:|-------------:|------:|----------:|------------:|
| ValidateDnaContainsAnyExcept384 | sm       |      17.00 us |     0.041 us |  0.99 |      80 B |        1.00 |
| ValidateDnaContainsAnyExcept256 | sm       |      17.15 us |     0.024 us |  1.00 |      80 B |        1.00 |
| ValidateDnaContainsAnyExcept128 | sm       |      20.28 us |     0.053 us |  1.18 |      80 B |        1.00 |
| ValidateDnaBytePad128           | sm       |     117.94 us |     0.262 us |  6.88 |      80 B |        1.00 |
| ValidateDnaContainsAnyExcept64  | sm       |     862.45 us |     1.142 us | 50.29 |      81 B |        1.01 |
|                                 |          |               |              |       |           |             |
| ValidateDnaContainsAnyExcept384 | lg       |     507.39 us |     1.331 us |  1.00 |      81 B |        1.00 |
| ValidateDnaContainsAnyExcept256 | lg       |     509.05 us |     1.601 us |  1.00 |      81 B |        1.00 |
| ValidateDnaContainsAnyExcept128 | lg       |     598.10 us |     0.975 us |  1.17 |      81 B |        1.00 |
| ValidateDnaBytePad128           | lg       |   3,532.07 us |    12.810 us |  6.94 |      84 B |        1.04 |
| ValidateDnaContainsAnyExcept64  | lg       |  25,890.63 us |    19.680 us | 50.86 |     116 B |        1.43 |
|                                 |          |               |              |       |           |             |
| ValidateDnaContainsAnyExcept256 | xl       |  10,517.01 us |    56.332 us |  1.00 |      98 B |        1.00 |
| ValidateDnaContainsAnyExcept384 | xl       |  10,524.03 us |     9.299 us |  1.00 |      98 B |        1.00 |
| ValidateDnaContainsAnyExcept128 | xl       |  12,368.53 us |    18.423 us |  1.18 |      98 B |        1.00 |
| ValidateDnaBytePad128           | xl       |  71,250.97 us |   381.928 us |  6.78 |     245 B |        2.50 |
| ValidateDnaContainsAnyExcept64  | xl       | 526,335.84 us | 1,820.424 us | 50.05 |    1232 B |       12.57 |

## There is no SIMD instruction for Vector64, it seems, which is why it is very slow.

eg `calling System.Runtime.Intrinsics.Vector64:Equals[ubyte]`

```
G_M000_IG05:                ;; offset=0073H
418BF5               mov      esi, r13d
410FB63436           movzx    rsi, byte  ptr [r14+rsi]
488D7DA8             lea      rdi, bword ptr [rbp-58H]
FF15C3153700         call     [System.Runtime.Intrinsics.Vector64:Create(ubyte):System.Runtime.Intrinsics.Vector64`1[ubyte]]
4C893C24             mov      qword ptr [rsp], r15
488B7DA8             mov      rdi, qword ptr [rbp-58H]
48897C2408           mov      qword ptr [rsp+08H], rdi
488D7DA0             lea      rdi, bword ptr [rbp-60H]
FF1594353700         call     [System.Runtime.Intrinsics.Vector64:Equals[ubyte](System.Runtime.Intrinsics.Vector64`1[ubyte],System.Runtime.Intrinsics.Vector64`1[ubyte]):System.Runtime.Intrinsics.Vector64`1[ubyte]]

```

# Arm neon arrangement (128bits) specifier: can be one of 8B, 16B, 4H, 8H, 2S, 4S or 2D.

# Note: DOTNET_JitDisasm=ValidateDnaVec256, no quotes

Doing the following loop unrolling is ~20% faster,
and adding another Vector128<byte>, making it 3 (or even adding another one), does not increase performance further

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

## Shuffle

* Control mask is the position of elements easier to see when expressed in binary format:
    * given input [20, 21, 22, 23]
    * `0b11_10_01_00` == `0xE4` => No shuffling, element stays the same
    * `0b00_01_10_11` => reverse elements to [23, 22, 21, 20]

```
const int start = 20;
const int length = 32;
var arr1 = Enumerable.Range(start, start + length).ToArray();
var arr1LeftPtr = (int*)arr1.AsMemory().Pin().Pointer;

Vector128<int> left = Sse2.LoadVector128(arr1LeftPtr);  // left: 20, 21, 22, 23

Vector128<int> reversedLeft = Sse2.Shuffle(left, 0b00_01_10_11);  // left: 23, 22, 21, 20
Vector128<int> reversedLeft2 = Sse2.Shuffle(left, 0b11_10_01_00); // left: 20, 21, 22 , 23
Vector128<int> reversedRight = Sse2.Shuffle(left, 0b00_01_00_01); // left: 21, 20, 21, 20
```

## Intel Assembly

