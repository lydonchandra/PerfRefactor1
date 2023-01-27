/home/don/RiderProjects/Refactor1/Bio/Dna/DnaConsole/bin/Release/net7.0/DnaConsole
; Assembly listing for method DnaLib.bla:CompressSimdHarold(System.ReadOnlySpan`1[ubyte]):System.Span`1[byte]
; Emitting BLENDED_CODE for X64 CPU with AVX - Unix
; Tier-1 compilation
; optimized code
; rbp based frame
; fully interruptible
; No PGO data
; 4 inlinees with PGO data; 11 single block inlinees; 2 inlinees without PGO data

G_M000_IG01:                ;; offset=0000H
       55                   push     rbp
       4157                 push     r15
       4156                 push     r14
       4155                 push     r13
       4154                 push     r12
       53                   push     rbx
       4883EC48             sub      rsp, 72
       C5F877               vzeroupper
       488D6C2470           lea      rbp, [rsp+70H]
       33C0                 xor      eax, eax
       48894598             mov      qword ptr [rbp-68H], rax
       C4413857C0           vxorps   xmm8, xmm8
       C5797F45A0           vmovdqa  xmmword ptr [rbp-60H], xmm8
       C5797F45B0           vmovdqa  xmmword ptr [rbp-50H], xmm8
       C5797F45C0           vmovdqa  xmmword ptr [rbp-40H], xmm8
       4C8BF7               mov      r14, rdi
       8BDE                 mov      ebx, esi

G_M000_IG02:                ;; offset=0035H
       4863F3               movsxd   rsi, ebx
       48BFB859C85A057F0000 mov      rdi, 0x7F055AC859B8
       E8292D5F7E           call     CORINFO_HELP_NEWARR_1_VC
       4C8BF8               mov      r15, rax
       4533E4               xor      r12d, r12d
       448BEB               mov      r13d, ebx

G_M000_IG03:                ;; offset=0050H
       498BFF               mov      rdi, r15
       8B7708               mov      esi, dword ptr [rdi+08H]
       48897DC0             mov      gword ptr [rbp-40H], rdi
       33FF                 xor      edi, edi
       897DC8               mov      dword ptr [rbp-38H], edi
       8975CC               mov      dword ptr [rbp-34H], esi
       488D7DC0             lea      rdi, bword ptr [rbp-40H]
       488D75A8             lea      rsi, bword ptr [rbp-58H]
       FF15B81B2000         call     [System.Memory`1[byte]:Pin():System.Buffers.MemoryHandle:this]
       488B45B0             mov      rax, qword ptr [rbp-50H]
       488945D0             mov      qword ptr [rbp-30H], rax
       4585ED               test     r13d, r13d
       0F849B010000         je       G_M000_IG14

G_M000_IG04:                ;; offset=0081H
       4963F5               movsxd   rsi, r13d
       48BF9017B95A057F0000 mov      rdi, 0x7F055AB91790
       E8DD2C5F7E           call     CORINFO_HELP_NEWARR_1_VC
       48894590             mov      gword ptr [rbp-70H], rax
       488D7810             lea      rdi, bword ptr [rax+10H]
       418BD5               mov      edx, r13d
       498BF6               mov      rsi, r14
       FF15117F0C00         call     [System.Buffer:Memmove(byref,byref,ulong)]
       488B4590             mov      rax, gword ptr [rbp-70H]

G_M000_IG05:                ;; offset=00ABH
       4885C0               test     rax, rax
       0F8445010000         je       G_M000_IG11

G_M000_IG06:                ;; offset=00B4H
       488BF8               mov      rdi, rax
       8B7708               mov      esi, dword ptr [rdi+08H]

G_M000_IG07:                ;; offset=00BAH
       48897D98             mov      gword ptr [rbp-68H], rdi
       33FF                 xor      edi, edi
       897DA0               mov      dword ptr [rbp-60H], edi
       8975A4               mov      dword ptr [rbp-5CH], esi
       488D7D98             lea      rdi, bword ptr [rbp-68H]
       488D75A8             lea      rsi, bword ptr [rbp-58H]
       FF153C1E2000         call     [System.Memory`1[ubyte]:Pin():System.Buffers.MemoryHandle:this]
       488B7DB0             mov      rdi, qword ptr [rbp-50H]
       418D44241F           lea      eax, [r12+1FH]
       3BC3                 cmp      eax, ebx
       0F8D01010000         jge      G_M000_IG09
       C5FD100573010000     vmovupd  ymm0, ymmword ptr[reloc @RWD00] ;; 0x5F 95
       C5FD100D8B010000     vmovupd  ymm1, ymmword ptr[reloc @RWD32] ;; 0x5E 94
       C5FD1015A3010000     vmovupd  ymm2, ymmword ptr[reloc @RWD64] ;; 0x40 64
       C5FD101DBB010000     vmovupd  ymm3, ymmword ptr[reloc @RWD96] ;; 0x51 81
                            align    [0 bytes for IG08]

G_M000_IG08:                ;; offset=0105H
       4963C4               movsxd   rax, r12d
       C5FE6F2407           vmovdqu  ymm4, ymmword ptr[rdi+rax] ;; ymm4: data
       C5DDFCE8             vpaddb   ymm5, ymm4, ymm0           ;; data + 95
       C5D564E9             vpcmpgtb ymm5, ymm5, ymm1           ;; ymm5: is_above_ws
       C5DDFCF2             vpaddb   ymm6, ymm4, ymm2           ;; data + 64
       C5CD64F1             vpcmpgtb ymm6, ymm6, ymm1           ;; ymm6: is_control
       C5DDFCFB             vpaddb   ymm7, ymm4, ymm3           ;; data + 71
       C5C5643DB7010000     vpcmpgtb ymm7, ymm7, ymmword ptr[reloc @RWD128]
       C55DF805CF010000     vpsubb   ymm8, ymm4, ymmword ptr[reloc @RWD160]
       C53D6405E7010000     vpcmpgtb ymm8, ymm8, ymmword ptr[reloc @RWD192]
       C53DDF05FF010000     vpandn   ymm8, ymm8, ymmword ptr[reloc @RWD224]
       C4415DEFC0           vpxor    ymm8, ymm4, ymm8
       C53DF8CA             vpsubb   ymm9, ymm8, ymm2
       C57D10150E020000     vmovupd  ymm10, ymmword ptr[reloc @RWD256]
       C44135DCCA           vpaddusb ymm9, ymm9, ymm10
       C57D101D21020000     vmovupd  ymm11, ymmword ptr[reloc @RWD288]
       C4422500C9           vpshufb  ymm9, ymm11, ymm9
       C53DF80534020000     vpsubb   ymm8, ymm8, ymmword ptr[reloc @RWD320]
       C4413DDCC2           vpaddusb ymm8, ymm8, ymm10
       C57D101547020000     vmovupd  ymm10, ymmword ptr[reloc @RWD352]
       C4422D00C0           vpshufb  ymm8, ymm10, ymm8
       C44135EBC0           vpor     ymm8, ymm9, ymm8
       C57D100D55020000     vmovupd  ymm9, ymmword ptr[reloc @RWD384]
       C5DDFC256D020000     vpaddb   ymm4, ymm4, ymmword ptr[reloc @RWD416]
       C4C33D4CE140         vpblendvb ymm4, ymm8, ymm9, ymm4
       C4C14DDBF1           vpand    ymm6, ymm6, ymm9
       C5D5EBEE             vpor     ymm5, ymm5, ymm6
       C5DDEBE5             vpor     ymm4, ymm4, ymm5
       C4E35D4C257002000070 vpblendvb ymm4, ymm4, [reloc @RWD448], ymm7
       488B75D0             mov      rsi, qword ptr [rbp-30H]
       C5FE7F2406           vmovdqu  ymmword ptr[rsi+rax], ymm4         ;; res.Store(outPtr+i)
       4183C420             add      r12d, 32
       418D44241F           lea      eax, [r12+1FH]
       3BC3                 cmp      eax, ebx
       488975D0             mov      qword ptr [rbp-30H], rsi
       0F8C37FFFFFF         jl       G_M000_IG08

G_M000_IG09:                ;; offset=01CEH
       443BE3               cmp      r12d, ebx
       7D17                 jge      SHORT G_M000_IG12

G_M000_IG10:                ;; offset=01D3H
       83FB20               cmp      ebx, 32
       7C12                 jl       SHORT G_M000_IG12
       448D63E0             lea      r12d, [rbx-20H]
       E96FFEFFFF           jmp      G_M000_IG03

G_M000_IG11:                ;; offset=01E1H
       33FF                 xor      rdi, rdi
       33F6                 xor      esi, esi
       E9D0FEFFFF           jmp      G_M000_IG07

G_M000_IG12:                ;; offset=01EAH
       498D4710             lea      rax, bword ptr [r15+10H]
       418B5708             mov      edx, dword ptr [r15+08H]

G_M000_IG13:                ;; offset=01F2H
       C5F877               vzeroupper
       4883C448             add      rsp, 72
       5B                   pop      rbx
       415C                 pop      r12
       415D                 pop      r13
       415E                 pop      r14
       415F                 pop      r15
       5D                   pop      rbp
       C3                   ret

G_M000_IG14:                ;; offset=0204H
       48BFB8489E5A057F0000 mov      rdi, 0x7F055A9E48B8
       BE18000000           mov      esi, 24
       E888FD5E7E           call     CORINFO_HELP_CLASSINIT_SHARED_DYNAMICCLASS
       48BE801F004CC57E0000 mov      rsi, 0x7EC54C001F80
       488B06               mov      rax, gword ptr [rsi]
       E981FEFFFF           jmp      G_M000_IG05

RWD00   dq      5F5F5F5F5F5F5F5Fh, 5F5F5F5F5F5F5F5Fh, 5F5F5F5F5F5F5F5Fh, 5F5F5F5F5F5F5F5Fh
RWD32   dq      5E5E5E5E5E5E5E5Eh, 5E5E5E5E5E5E5E5Eh, 5E5E5E5E5E5E5E5Eh, 5E5E5E5E5E5E5E5Eh
RWD64   dq      4040404040404040h, 4040404040404040h, 4040404040404040h, 4040404040404040h
RWD96   dq      5151515151515151h, 5151515151515151h, 5151515151515151h, 5151515151515151h
RWD128  dq      7D7D7D7D7D7D7D7Dh, 7D7D7D7D7D7D7D7Dh, 7D7D7D7D7D7D7D7Dh, 7D7D7D7D7D7D7D7Dh
RWD160  dq      E0E0E0E0E0E0E0E0h, E0E0E0E0E0E0E0E0h, E0E0E0E0E0E0E0E0h, E0E0E0E0E0E0E0E0h
RWD192  dq      1A1A1A1A1A1A1A1Ah, 1A1A1A1A1A1A1A1Ah, 1A1A1A1A1A1A1A1Ah, 1A1A1A1A1A1A1A1Ah
RWD224  dq      2020202020202020h, 2020202020202020h, 2020202020202020h, 2020202020202020h
RWD256  dq      7070707070707070h, 7070707070707070h, 7070707070707070h, 7070707070707070h
RWD288  dq      070D0603040300FEh, 14020C0A0B140908h, 070D0603040300FEh, 14020C0A0B140908h
RWD320  dq      5050505050505050h, 5050505050505050h, 5050505050505050h, 5050505050505050h
RWD352  dq      111304100F01050Eh, 15FEFEFEFE061214h, 111304100F01050Eh, 15FEFEFEFE061214h
RWD384  dq      FEFEFEFEFEFEFEFEh, FEFEFEFEFEFEFEFEh, FEFEFEFEFEFEFEFEh, FEFEFEFEFEFEFEFEh
RWD416  dq      0101010101010101h, 0101010101010101h, 0101010101010101h, 0101010101010101h
RWD448  dq      1515151515151515h, 1515151515151515h, 1515151515151515h, 1515151515151515h

; Total bytes of code 554


Process finished with exit code 0.

