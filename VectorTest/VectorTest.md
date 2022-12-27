```
public static bool TestShuffle(int[] arr1)
    {
        unsafe
        {
            var arr1LeftPtr = (int*)arr1.AsMemory().Pin().Pointer;
            Vector128<int> left = Sse2.LoadVector128(arr1LeftPtr);

            // 0x1b == 27 == 00 01 10 11 == 0 1 2 3
            // 0x1e == 30 == 00 01 11 10 == 0 1 3 2
            Vector128<int> reversedLeft = Sse2.Shuffle(left, 0b00_01_10_11);
            Sse2.Store(arr1LeftPtr, reversedLeft);
            return true;
        }
    }

/xxx/Refactor1/VectorTest/bin/Release/net7.0/VectorTest
; Assembly listing for method Util:TestShuffle(<unnamed>):bool
; Emitting BLENDED_CODE for X64 CPU with AVX - Unix
; Tier-0 compilation
; MinOpts code
; instrumented for collecting profile data
; rbp based frame
; partially interruptible

G_M000_IG01:                ;; offset=0000H
       55                   push     rbp
       4883EC60             sub      rsp, 96
       C5F877               vzeroupper
       488D6C2460           lea      rbp, [rsp+60H]
       33C0                 xor      eax, eax
       488945A8             mov      qword ptr [rbp-58H], rax
       C4413857C0           vxorps   xmm8, xmm8
       C5797F45B0           vmovdqa  xmmword ptr [rbp-50H], xmm8
       C5797F45C0           vmovdqa  xmmword ptr [rbp-40H], xmm8
       C5797F45D0           vmovdqa  xmmword ptr [rbp-30H], xmm8
       C5797F45E0           vmovdqa  xmmword ptr [rbp-20H], xmm8
       488945F0             mov      qword ptr [rbp-10H], rax
       48897DF8             mov      gword ptr [rbp-08H], rdi

G_M000_IG02:                ;; offset=0034H
       488B7DF8             mov      rdi, gword ptr [rbp-08H]
       FF154A933500         call     [System.MemoryExtensions:AsMemory[int](<unnamed>):System.Memory`1[int]]
       488945A8             mov      gword ptr [rbp-58H], rax
       488955B0             mov      qword ptr [rbp-50H], rdx

G_M000_IG03:                ;; offset=0046H
       C5FA6F45A8           vmovdqu  xmm0, xmmword ptr [rbp-58H]
       C5FA7F45D0           vmovdqu  xmmword ptr [rbp-30H], xmm0

G_M000_IG04:                ;; offset=0050H
       488D7DD0             lea      rdi, [rbp-30H]
       488D75B8             lea      rsi, [rbp-48H]
       FF15525B3500         call     [System.Memory`1[int]:Pin():System.Buffers.MemoryHandle:this]
       488D7DB8             lea      rdi, [rbp-48H]
       FF15205C3500         call     [System.Buffers.MemoryHandle:get_Pointer():ulong:this]
       488945F0             mov      qword ptr [rbp-10H], rax       ;; store pointer to reversedLeft 
       488B45F0             mov      rax, qword ptr [rbp-10H]
       C5F970001B           vpshufd  xmm0, xmmword ptr [rax], 27    ;; shuffle 
       C5F92945E0           vmovapd  xmmword ptr [rbp-20H], xmm0    ;; store reversedLeft in memory, free-up xmm0
       488B45F0             mov      rax, qword ptr [rbp-10H]       ;; load address arr1LeftPtr 
       C5F92845E0           vmovapd  xmm0, xmmword ptr [rbp-20H]    ;; load reversedLeft in xmm0
       C5FA7F00             vmovdqu  xmmword ptr [rax], xmm0        ;; store reversedLeft into arr1LeftPtr
       B801000000           mov      eax, 1                         ;; return true

G_M000_IG05:                ;; offset=008CH
       4883C460             add      rsp, 96
       5D                   pop      rbp
       C3                   ret

; Total bytes of code 146


Process finished with exit code 0.

```