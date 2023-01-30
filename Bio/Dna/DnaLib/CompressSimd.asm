/home/don/RiderProjects/Refactor1/Bio/Dna/DnaConsole/bin/Release/net7.0/DnaConsole
; Assembly listing for method DnaLib.bla:CompressSimd(System.ReadOnlySpan`1[ubyte]):<unnamed>
; Emitting BLENDED_CODE for X64 CPU with AVX - Unix
; Tier-1 compilation
; optimized code
; rbp based frame
; fully interruptible
; No PGO data
; 1 inlinees with PGO data; 2 single block inlinees; 1 inlinees without PGO data

G_M000_IG01:                ;; offset=0000H
       55                   push     rbp
       4156                 push     r14
       53                   push     rbx
       C5F877               vzeroupper
       488D6C2410           lea      rbp, [rsp+10H]
       4C8BF7               mov      r14, rdi
       8BDE                 mov      ebx, esi

G_M000_IG02:                ;; offset=0011H
       4863F3               movsxd   rsi, ebx
       48BF8817F5594E7F0000 mov      rdi, 0x7F4E59F51788
       E8AD0A5E7E           call     CORINFO_HELP_NEWARR_1_VC
       488B3D1E091F00       mov      rdi, qword ptr [(reloc 0x7f4e5a044ba8)]
       C5FE6F07             vmovdqu  ymm0, ymmword ptr[rdi]
       33FF                 xor      edi, edi
       85DB                 test     ebx, ebx
       0F8E95010000         jle      G_M000_IG36
       85DB                 test     ebx, ebx
       0F8CCA000000         jl       G_M000_IG20
       395808               cmp      dword ptr [rax+08H], ebx
       0F8CC1000000         jl
                            align    [0 bytes for IG03]

G_M000_IG03:                ;; offset=0049H
       8BF7                 mov      esi, edi
       410FB61436           movzx    rdx, byte  ptr [r14+rsi]
       83FA61               cmp      edx, 97
       7C06                 jl       SHORT G_M000_IG05

G_M000_IG04:                ;; offset=0055H
       83E2DF               and      edx, -33
       0FB6D2               movzx    rdx, dl

G_M000_IG05:                ;; offset=005BH
       C5F96ECA             vmovd    xmm1, edx
       C4E27D78C9           vpbroadcastb ymm1, ymm1
       C5FD74C9             vpcmpeqb ymm1, ymm0, ymm1
       C5FDD7C9             vpmovmskb ecx, ymm1
       F30FBCC9             tzcnt    ecx, ecx
       83F920               cmp      ecx, 32
       7406                 je       SHORT G_M000_IG07

G_M000_IG06:                ;; offset=0075H
       440FB6C1             movzx    r8, cl
       EB78                 jmp      SHORT G_M000_IG18

G_M000_IG07:                ;; offset=007BH
       83FA4F               cmp      edx, 79
       7725                 ja       SHORT G_M000_IG09
       83FA42               cmp      edx, 66
       7714                 ja       SHORT G_M000_IG08
       8D4AD3               lea      ecx, [rdx-2DH]
       83F901               cmp      ecx, 1
       764A                 jbe      SHORT G_M000_IG14
       83FA42               cmp      edx, 66
       754C                 jne      SHORT G_M000_IG15
       B903000000           mov      ecx, 3
       EB56                 jmp      SHORT G_M000_IG17

G_M000_IG08:                ;; offset=0099H
       83FA4A               cmp      edx, 74
       7424                 je       SHORT G_M000_IG11
       83FA4F               cmp      edx, 79
       741F                 je       SHORT G_M000_IG11
       EB39                 jmp      SHORT G_M000_IG15

G_M000_IG09:                ;; offset=00A5H
       83FA58               cmp      edx, 88
       770C                 ja       SHORT G_M000_IG10
       83FA55               cmp      edx, 85
       741A                 je       SHORT G_M000_IG12
       83FA58               cmp      edx, 88
       740E                 je       SHORT G_M000_IG11
       EB28                 jmp      SHORT G_M000_IG15

G_M000_IG10:                ;; offset=00B6H
       83FA5A               cmp      edx, 90
       7415                 je       SHORT G_M000_IG13
       83FA5F               cmp      edx, 95
       7417                 je       SHORT G_M000_IG14
       EB1C                 jmp      SHORT G_M000_IG15

G_M000_IG11:                ;; offset=00C2H
       B914000000           mov      ecx, 20
       EB26                 jmp      SHORT G_M000_IG17

G_M000_IG12:                ;; offset=00C9H
       B904000000           mov      ecx, 4
       EB1F                 jmp      SHORT G_M000_IG17

G_M000_IG13:                ;; offset=00D0H
       B906000000           mov      ecx, 6
       EB18                 jmp      SHORT G_M000_IG17

G_M000_IG14:                ;; offset=00D7H
       B915000000           mov      ecx, 21
       EB11                 jmp      SHORT G_M000_IG17

G_M000_IG15:                ;; offset=00DEH
       83FA20               cmp      edx, 32
       7F07                 jg       SHORT G_M000_IG16
       B9FF000000           mov      ecx, 255
       EB05                 jmp      SHORT G_M000_IG17

G_M000_IG16:                ;; offset=00EAH
       B9FE000000           mov      ecx, 254

G_M000_IG17:                ;; offset=00EFH
       440FB6C1             movzx    r8, cl

G_M000_IG18:                ;; offset=00F3H
       4488443010           mov      byte  ptr [rax+rsi+10H], r8b
       FFC7                 inc      edi
       3BFB                 cmp      edi, ebx
       0F8C47FFFFFF         jl       G_M000_IG03

G_M000_IG19:                ;; offset=0102H
       E9C3000000           jmp      G_M000_IG36

G_M000_IG20:                ;; offset=0107H
       8BD7                 mov      edx, edi
       410FB61416           movzx    rdx, byte  ptr [r14+rdx]
       83FA61               cmp      edx, 97
       7C06                 jl       SHORT G_M000_IG22

G_M000_IG21:                ;; offset=0113H
       83E2DF               and      edx, -33
       0FB6D2               movzx    rdx, dl

G_M000_IG22:                ;; offset=0119H
       C5F96ECA             vmovd    xmm1, edx
       C4E27D78C9           vpbroadcastb ymm1, ymm1
       C5FD74C9             vpcmpeqb ymm1, ymm0, ymm1
       C5FDD7C9             vpmovmskb ecx, ymm1
       F30FBCC9             tzcnt    ecx, ecx
       83F920               cmp      ecx, 32
       7406                 je       SHORT G_M000_IG24

G_M000_IG23:                ;; offset=0133H
       440FB6C1             movzx    r8, cl
       EB78                 jmp      SHORT G_M000_IG35

G_M000_IG24:                ;; offset=0139H
       83FA4F               cmp      edx, 79
       7725                 ja       SHORT G_M000_IG26
       83FA42               cmp      edx, 66
       7714                 ja       SHORT G_M000_IG25
       8D4AD3               lea      ecx, [rdx-2DH]
       83F901               cmp      ecx, 1
       764A                 jbe      SHORT G_M000_IG31
       83FA42               cmp      edx, 66
       754C                 jne      SHORT G_M000_IG32
       B903000000           mov      ecx, 3
       EB56                 jmp      SHORT G_M000_IG34

G_M000_IG25:                ;; offset=0157H
       83FA4A               cmp      edx, 74
       7424                 je       SHORT G_M000_IG28
       83FA4F               cmp      edx, 79
       741F                 je       SHORT G_M000_IG28
       EB39                 jmp      SHORT G_M000_IG32

G_M000_IG26:                ;; offset=0163H
       83FA58               cmp      edx, 88
       770C                 ja       SHORT G_M000_IG27
       83FA55               cmp      edx, 85
       741A                 je       SHORT G_M000_IG29
       83FA58               cmp      edx, 88
       740E                 je       SHORT G_M000_IG28
       EB28                 jmp      SHORT G_M000_IG32

G_M000_IG27:                ;; offset=0174H
       83FA5A               cmp      edx, 90
       7415                 je       SHORT G_M000_IG30
       83FA5F               cmp      edx, 95
       7417                 je       SHORT G_M000_IG31
       EB1C                 jmp      SHORT G_M000_IG32

G_M000_IG28:                ;; offset=0180H
       B914000000           mov      ecx, 20
       EB26                 jmp      SHORT G_M000_IG34

G_M000_IG29:                ;; offset=0187H
       B904000000           mov      ecx, 4
       EB1F                 jmp      SHORT G_M000_IG34

G_M000_IG30:                ;; offset=018EH
       B906000000           mov      ecx, 6
       EB18                 jmp      SHORT G_M000_IG34

G_M000_IG31:                ;; offset=0195H
       B915000000           mov      ecx, 21
       EB11                 jmp      SHORT G_M000_IG34

G_M000_IG32:                ;; offset=019CH
       83FA20               cmp      edx, 32
       7F07                 jg       SHORT G_M000_IG33
       B9FF000000           mov      ecx, 255
       EB05                 jmp      SHORT G_M000_IG34

G_M000_IG33:                ;; offset=01A8H
       B9FE000000           mov      ecx, 254

G_M000_IG34:                ;; offset=01ADH
       440FB6C1             movzx    r8, cl

G_M000_IG35:                ;; offset=01B1H
       3B7808               cmp      edi, dword ptr [rax+08H]
       7319                 jae      SHORT G_M000_IG37
       8BF7                 mov      esi, edi
       4488443010           mov      byte  ptr [rax+rsi+10H], r8b
       FFC7                 inc      edi
       3BFB                 cmp      edi, ebx
       0F8C40FFFFFF         jl       G_M000_IG20

G_M000_IG36:                ;; offset=01C7H
       C5F877               vzeroupper
       5B                   pop      rbx
       415E                 pop      r14
       5D                   pop      rbp
       C3                   ret

G_M000_IG37:                ;; offset=01CFH
       E83C6B5E7E           call     CORINFO_HELP_RNGCHKFAIL
       CC                   int3

; Total bytes of code 469


Process finished with exit code 0.

