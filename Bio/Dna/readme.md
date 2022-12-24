BenchmarkDotNet=v0.13.2, OS=ubuntu 20.04
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.100
[Host]     : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
Job-OPKXZZ : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2

IterationCount=3 LaunchCount=2 WarmupCount=2

| Method                    |     Mean |  StdDev | Allocated |
|---------------------------|---------:|--------:|----------:|
| ValidateDnaFromFile       | 218.8 us | 2.81 us | 193.43 KB |
| ValidateDnaFromFileAsByte | 359.2 us | 0.80 us |  65.36 KB |
