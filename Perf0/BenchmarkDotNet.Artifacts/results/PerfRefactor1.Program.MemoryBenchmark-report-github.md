``` ini

BenchmarkDotNet=v0.13.2, OS=ubuntu 20.04
Intel Core i7-9700K CPU 3.60GHz (Coffee Lake), 1 CPU, 8 logical and 8 physical cores
.NET SDK=6.0.401
  [Host]     : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT AVX2
  Job-FQMFJT : .NET 5.0.17 (5.0.1722.21314), X64 RyuJIT AVX2
  Job-HAENNU : .NET 6.0.9 (6.0.922.41905), X64 RyuJIT AVX2


```
|     Method |  Runtime |     Mean |     Error |    StdDev | Ratio |   Gen0 | Allocated | Alloc Ratio |
|----------- |--------- |---------:|----------:|----------:|------:|-------:|----------:|------------:|
| ReadWrite1 | .NET 5.0 | 4.800 ms | 0.0324 ms | 0.0287 ms |  1.00 | 7.8125 |   72002 B |       1.000 |
| ReadWrite2 | .NET 5.0 | 4.675 ms | 0.0220 ms | 0.0205 ms |  0.97 |      - |       2 B |       0.000 |
| ReadWrite1 | .NET 6.0 | 4.606 ms | 0.0127 ms | 0.0113 ms |  0.96 | 7.8125 |   72006 B |       1.000 |
| ReadWrite2 | .NET 6.0 | 4.512 ms | 0.0139 ms | 0.0130 ms |  0.94 |      - |       6 B |       0.000 |
