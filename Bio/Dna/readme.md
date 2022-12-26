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
