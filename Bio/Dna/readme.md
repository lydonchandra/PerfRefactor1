# Protein compression

## Added CompressSimdHarold

### Edge case scenarios

* Data looks like this `_xjoubzxjoubz-._xjoubz-._xjoubz` repeated up to `kb100` or `kbXXX` dataSize

### Typical scenario

* Data looks like `MTTQAPTFTQPLQSVVVLEGSTATFEAHISGFPVPEVSWFRDGQVISTSTLPGVQ` repeated up to `kb100` or `kbXXX` dataSize

### Results:

| Method               | dataSize       |         Mean |     StdDev | Ratio |     Gen0 |     Gen1 |     Gen2 |   Allocated | Alloc Ratio |
|----------------------|----------------|-------------:|-----------:|------:|---------:|---------:|---------:|------------:|------------:|
| CompressSimd         | kb100          |     96.38 us |   0.047 us |  0.14 |  32.2266 |  32.2266 |  32.2266 |   100.83 KB |        1.00 |
| CompressSimdHarold   | kb100          |    111.72 us |   1.036 us |  0.16 |        - |        - |        - |   302.09 KB |        3.00 |
| CompressSimdNoSwitch | kb100          |    136.61 us |   0.582 us |  0.19 |  32.2266 |  32.2266 |  32.2266 |   100.83 KB |        1.00 |
| Compress             | kb100          |    703.66 us |   0.377 us |  1.00 |  32.2266 |  32.2266 |  32.2266 |   100.83 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb100edgeCase  |    109.41 us |   1.877 us |  0.74 |        - |        - |        - |   293.13 KB |        3.00 |
| CompressSimdNoSwitch | kb100edgeCase  |    118.71 us |   0.042 us |  0.80 |  31.1279 |  31.1279 |  31.1279 |    97.85 KB |        1.00 |
| Compress             | kb100edgeCase  |    147.67 us |   0.971 us |  1.00 |  31.0059 |  31.0059 |  31.0059 |    97.85 KB |        1.00 |
| CompressSimd         | kb100edgeCase  |    238.95 us |   0.230 us |  1.62 |  31.0059 |  31.0059 |  31.0059 |    97.85 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimd         | kb200          |    190.16 us |   0.133 us |  0.14 |  62.2559 |  62.2559 |  62.2559 |   201.54 KB |        1.00 |
| CompressSimdHarold   | kb200          |    213.86 us |   0.595 us |  0.15 |        - |        - |        - |   603.99 KB |        3.00 |
| CompressSimdNoSwitch | kb200          |    268.31 us |   1.992 us |  0.19 |  62.0117 |  62.0117 |  62.0117 |   201.54 KB |        1.00 |
| Compress             | kb200          |  1,394.37 us |   3.090 us |  1.00 |  60.5469 |  60.5469 |  60.5469 |   201.54 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb200edgeCase  |    206.98 us |   0.031 us |  0.70 |        - |        - |        - |   586.07 KB |        3.00 |
| CompressSimdNoSwitch | kb200edgeCase  |    236.94 us |   1.061 us |  0.80 |  62.2559 |  62.2559 |  62.2559 |   195.57 KB |        1.00 |
| Compress             | kb200edgeCase  |    295.67 us |   0.189 us |  1.00 |  62.0117 |  62.0117 |  62.0117 |   195.57 KB |        1.00 |
| CompressSimd         | kb200edgeCase  |    476.61 us |   2.413 us |  1.61 |  62.0117 |  62.0117 |  62.0117 |   195.57 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimd         | kb400          |    385.58 us |   0.879 us |  0.14 | 124.5117 | 124.5117 | 124.5117 |   402.95 KB |        1.00 |
| CompressSimdHarold   | kb400          |    415.25 us |   3.327 us |  0.15 |        - |        - |        - |  1207.78 KB |        3.00 |
| CompressSimdNoSwitch | kb400          |    533.62 us |   1.721 us |  0.19 | 124.0234 | 124.0234 | 124.0234 |   402.95 KB |        1.00 |
| Compress             | kb400          |  2,789.99 us |   3.676 us |  1.00 | 121.0938 | 121.0938 | 121.0938 |   402.95 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb400edgeCase  |    402.68 us |   1.178 us |  0.69 |        - |        - |        - |  1171.94 KB |        3.00 |
| CompressSimdNoSwitch | kb400edgeCase  |    480.21 us |   5.765 us |  0.82 | 124.5117 | 124.5117 | 124.5117 |   391.01 KB |        1.00 |
| Compress             | kb400edgeCase  |    586.46 us |   0.475 us |  1.00 | 124.0234 | 124.0234 | 124.0234 |   391.01 KB |        1.00 |
| CompressSimd         | kb400edgeCase  |    953.84 us |   1.557 us |  1.63 | 124.0234 | 124.0234 | 124.0234 |   391.01 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimd         | kb800          |    798.08 us |   5.157 us |  0.14 | 199.2188 | 199.2188 | 199.2188 |   805.65 KB |        1.00 |
| CompressSimdHarold   | kb800          |    830.61 us |   8.917 us |  0.15 |        - |        - |        - |   2415.4 KB |        3.00 |
| CompressSimdNoSwitch | kb800          |  1,072.92 us |   0.636 us |  0.19 | 197.2656 | 197.2656 | 197.2656 |   805.65 KB |        1.00 |
| Compress             | kb800          |  5,607.25 us |  12.128 us |  1.00 | 195.3125 | 195.3125 | 195.3125 |   805.65 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb800edgeCase  |    802.88 us |   2.192 us |  0.60 |        - |        - |        - |   2343.7 KB |        3.00 |
| CompressSimdNoSwitch | kb800edgeCase  |    990.47 us |   1.716 us |  0.74 | 197.2656 | 197.2656 | 197.2656 |   781.76 KB |        1.00 |
| Compress             | kb800edgeCase  |  1,338.01 us |   2.244 us |  1.00 | 197.2656 | 197.2656 | 197.2656 |   781.76 KB |        1.00 |
| CompressSimd         | kb800edgeCase  |  1,958.71 us |   6.568 us |  1.46 | 195.3125 | 195.3125 | 195.3125 |   781.76 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimd         | kb2000         |  2,066.46 us |  19.052 us |  0.15 | 324.2188 | 324.2188 | 324.2188 |  2013.56 KB |        1.00 |
| CompressSimdHarold   | kb2000         |  2,265.57 us |   1.035 us |  0.16 |        - |        - |        - |  6038.26 KB |        3.00 |
| CompressSimdNoSwitch | kb2000         |  2,819.18 us |  33.320 us |  0.20 | 324.2188 | 324.2188 | 324.2188 |  2013.56 KB |        1.00 |
| Compress             | kb2000         | 14,021.96 us |  52.383 us |  1.00 | 296.8750 | 296.8750 | 296.8750 |  2013.53 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb2000edgeCase |  2,203.49 us |   1.196 us |  0.65 |        - |        - |        - |  5858.98 KB |        3.00 |
| CompressSimdNoSwitch | kb2000edgeCase |  2,533.21 us |   3.103 us |  0.74 | 324.2188 | 324.2188 | 324.2188 |  1953.81 KB |        1.00 |
| Compress             | kb2000edgeCase |  3,405.14 us |   3.187 us |  1.00 | 324.2188 | 324.2188 | 324.2188 |  1953.81 KB |        1.00 |
| CompressSimd         | kb2000edgeCase |  4,956.90 us |  12.558 us |  1.46 | 320.3125 | 320.3125 | 320.3125 |   1953.8 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb4000         |  4,680.39 us |   4.714 us |  0.16 |        - |        - |        - | 12076.35 KB |        3.00 |
| CompressSimd         | kb4000         |  4,739.57 us |  48.412 us |  0.17 | 156.2500 | 156.2500 | 156.2500 |  4025.89 KB |        1.00 |
| CompressSimdNoSwitch | kb4000         |  6,038.64 us |  67.980 us |  0.21 | 156.2500 | 156.2500 | 156.2500 |  4025.89 KB |        1.00 |
| Compress             | kb4000         | 28,418.41 us | 179.401 us |  1.00 | 187.5000 | 187.5000 | 187.5000 |  4026.08 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb4000edgeCase |  4,481.91 us |  23.799 us |  0.61 |        - |        - |        - | 11717.78 KB |        3.00 |
| CompressSimdNoSwitch | kb4000edgeCase |  5,597.94 us |  18.124 us |  0.76 | 156.2500 | 156.2500 | 156.2500 |  3906.37 KB |        1.00 |
| Compress             | kb4000edgeCase |  7,352.41 us |  13.433 us |  1.00 | 156.2500 | 156.2500 | 156.2500 |  3906.37 KB |        1.00 |
| CompressSimd         | kb4000edgeCase | 10,118.96 us | 440.924 us |  1.38 | 296.8750 | 296.8750 | 296.8750 |  3906.71 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimd         | kb5000         |  5,592.89 us |   6.548 us |  0.16 | 156.2500 | 156.2500 | 156.2500 |  5032.24 KB |        1.00 |
| CompressSimdHarold   | kb5000         |  5,798.76 us |  20.272 us |  0.16 |        - |        - |        - | 15095.41 KB |        3.00 |
| CompressSimdNoSwitch | kb5000         |  7,375.83 us |  12.179 us |  0.21 | 156.2500 | 156.2500 | 156.2500 |  5032.24 KB |        1.00 |
| Compress             | kb5000         | 35,149.14 us | 515.925 us |  1.00 | 200.0000 | 200.0000 | 200.0000 |  5032.47 KB |        1.00 |
|                      |                |              |            |       |          |          |          |             |             |
| CompressSimdHarold   | kb5000edgeCase |  5,592.96 us |  26.440 us |  0.66 |        - |        - |        - | 14647.18 KB |        3.00 |
| CompressSimdNoSwitch | kb5000edgeCase |  6,791.18 us |   8.745 us |  0.81 | 156.2500 | 156.2500 | 156.2500 |  4882.84 KB |        1.00 |
| Compress             | kb5000edgeCase |  8,434.37 us |  34.883 us |  1.00 | 296.8750 | 296.8750 | 296.8750 |  4883.18 KB |        1.00 |
| CompressSimd         | kb5000edgeCase | 12,183.72 us |  24.779 us |  1.44 | 296.8750 | 296.8750 | 296.8750 |  4883.18 KB |        1.00 |

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

