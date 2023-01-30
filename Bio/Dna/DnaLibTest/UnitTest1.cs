using System.Runtime.Intrinsics;
using System.Text;
using DnaLib;
using Shouldly;
using Xunit.Abstractions;

// ReSharper disable StringLiteralTypo

namespace DnaLibTest;

public class UnitTest1
{
    private readonly ITestOutputHelper _outputHelper;

    public UnitTest1(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void TestValidateChar()
    {
        var dna = "ACGTATTAACGTATTAACGTATTAACGTATTAACGTATTAACGTATTAACGTATTA";
        var isValid = DnaUtil.ValidateDna(dna);
        isValid.ShouldBe(true);
    }

    [Fact]
    public void TestValidateCharNaive()
    {
        var dna = "ACGTATTAACGTATTAACGTATTAACGTATTAACGTATTAACGTATTAACGTATTA";
        var isValid = DnaUtil.ValidateDnaNaive(dna);
        isValid.ShouldBe(true);
    }

    [Fact]
    public void TestValidateCharNaiveNotValid()
    {
        var dna = "acgtattaacgtattaacgtattaacgtattaacgtattaacgtattaacgtattazz";
        var isValid = DnaUtil.ValidateDnaNaive(dna);
        isValid.ShouldBe(false);
    }

    [Fact]
    public void TestValidateByte()
    {
        var dna = "ACGTATTAACGTATTAACGTATTAACGTATTAACGTATTAACGTATTAACGTATTACACACACA";
        var dnaBytes = Encoding.UTF8.GetBytes(dna);
        var isValid = DnaUtil.ValidateDna(dnaBytes);
        isValid.ShouldBe(true);

        isValid = DnaUtil.ValidateDnaVec128(dnaBytes);
        isValid.ShouldBe(true);
    }

    [Fact]
    public async Task TestValidateDnaFromFileAsChar()
    {
        _outputHelper.WriteLine("Vector256.IsHardwareAccelerated: " + Vector256.IsHardwareAccelerated);
        var path = "Data/gene-lg.fna";
        var valid = await DnaUtil.ValidateDnaFromFileAsChar(path);
        valid.ShouldBe(true);
    }

    [Fact]
    public async Task TestValidateDnaFromFileAsByte()
    {
        var path = "Data/gene-lg.fna";
        var valid = await DnaUtil.ValidateDnaFromFileAsByte(path);
        valid.ShouldBe(true);
    }

    [Fact]
    public async Task TestReadString()
    {
        var path = "Data/gene-xl.fna";

        using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
        using var streamReader = new StreamReader(fileStream);
        var data = await streamReader.ReadToEndAsync();
        var dataBytes = Encoding.UTF8.GetBytes(data);

        var valid = DnaUtil.ValidateDna(data);
        valid.ShouldBe(true);

        valid = DnaUtil.ValidateDnaVec64(dataBytes);
        valid.ShouldBe(true);

        valid = DnaUtil.ValidateDnaVec128(dataBytes.AsSpan());
        valid.ShouldBe(true);

        valid = DnaUtil.ValidateDnaVec256(dataBytes.AsSpan());
        valid.ShouldBe(true);

        valid = DnaUtil.ValidateDnaVec384(dataBytes.AsSpan());
        valid.ShouldBe(true);

        valid = DnaUtil.ValidateDnaVec768(dataBytes.AsSpan());
        valid.ShouldBe(true);
    }

    [Fact]
    public void TestAa2iUppercase()
    {
        bla.aa2i((byte)'A').ShouldBe((byte)0);
        bla.aa2i((byte)'R').ShouldBe((byte)1);
        bla.aa2i((byte)'N').ShouldBe((byte)2);
        bla.aa2i((byte)'D').ShouldBe((byte)3);
        bla.aa2i((byte)'C').ShouldBe((byte)4);
        bla.aa2i((byte)'Q').ShouldBe((byte)5);
        bla.aa2i((byte)'E').ShouldBe((byte)6);
        bla.aa2i((byte)'G').ShouldBe((byte)7);
        bla.aa2i((byte)'H').ShouldBe((byte)8);
        bla.aa2i((byte)'I').ShouldBe((byte)9);
        bla.aa2i((byte)'L').ShouldBe((byte)10);
        bla.aa2i((byte)'K').ShouldBe((byte)11);
        bla.aa2i((byte)'M').ShouldBe((byte)12);
        bla.aa2i((byte)'F').ShouldBe((byte)13);
        bla.aa2i((byte)'P').ShouldBe((byte)14);
        bla.aa2i((byte)'S').ShouldBe((byte)15);
        bla.aa2i((byte)'T').ShouldBe((byte)16);
        bla.aa2i((byte)'W').ShouldBe((byte)17);
        bla.aa2i((byte)'Y').ShouldBe((byte)18);
        bla.aa2i((byte)'V').ShouldBe((byte)19);

        bla.aa2i((byte)'X').ShouldBe(bla.ANY);
        bla.aa2i((byte)'J').ShouldBe(bla.ANY);
        bla.aa2i((byte)'O').ShouldBe(bla.ANY);
        bla.aa2i((byte)'U').ShouldBe((byte)4);
        bla.aa2i((byte)'B').ShouldBe((byte)3);
        bla.aa2i((byte)'Z').ShouldBe((byte)6);
        bla.aa2i((byte)'-').ShouldBe(bla.GAP);
        bla.aa2i((byte)'.').ShouldBe(bla.GAP);
        bla.aa2i((byte)'_').ShouldBe(bla.GAP);

        bla.aa2i((byte)' ').ShouldBe(unchecked((byte)-1));
        bla.aa2i((byte)'{').ShouldBe(unchecked((byte)-2));
    }

    [Fact]
    public void TestAa2iLowercase()
    {
        bla.aa2i((byte)'a').ShouldBe((byte)0);
        bla.aa2i((byte)'r').ShouldBe((byte)1);
        bla.aa2i((byte)'n').ShouldBe((byte)2);
        bla.aa2i((byte)'d').ShouldBe((byte)3);
        bla.aa2i((byte)'c').ShouldBe((byte)4);
        bla.aa2i((byte)'q').ShouldBe((byte)5);
        bla.aa2i((byte)'e').ShouldBe((byte)6);
        bla.aa2i((byte)'g').ShouldBe((byte)7);
        bla.aa2i((byte)'h').ShouldBe((byte)8);
        bla.aa2i((byte)'i').ShouldBe((byte)9);
        bla.aa2i((byte)'l').ShouldBe((byte)10);
        bla.aa2i((byte)'k').ShouldBe((byte)11);
        bla.aa2i((byte)'m').ShouldBe((byte)12);
        bla.aa2i((byte)'f').ShouldBe((byte)13);
        bla.aa2i((byte)'p').ShouldBe((byte)14);
        bla.aa2i((byte)'s').ShouldBe((byte)15);
        bla.aa2i((byte)'t').ShouldBe((byte)16);
        bla.aa2i((byte)'w').ShouldBe((byte)17);
        bla.aa2i((byte)'y').ShouldBe((byte)18);
        bla.aa2i((byte)'v').ShouldBe((byte)19);

        bla.aa2i((byte)'x').ShouldBe(bla.ANY);
        bla.aa2i((byte)'j').ShouldBe(bla.ANY);
        bla.aa2i((byte)'o').ShouldBe(bla.ANY);
        bla.aa2i((byte)'u').ShouldBe((byte)4);
        bla.aa2i((byte)'b').ShouldBe((byte)3);
        bla.aa2i((byte)'z').ShouldBe((byte)6);

        bla.aa2i((byte)' ').ShouldBe(unchecked((byte)-1));
        bla.aa2i((byte)'{').ShouldBe(unchecked((byte)-2));
    }

    [Fact]
    public void TestAa2iSimd()
    {
        unsafe
        {
            Vector256<byte> vecInput0Upper = Vector256.Load((byte*)bla.proteinLutUpper);
            bla.aa2iSimd((byte)'A', vecInput0Upper).ShouldBe((byte)0);
            bla.aa2iSimd((byte)'R', vecInput0Upper).ShouldBe((byte)1);
            bla.aa2iSimd((byte)'N', vecInput0Upper).ShouldBe((byte)2);
            bla.aa2iSimd((byte)'D', vecInput0Upper).ShouldBe((byte)3);
            bla.aa2iSimd((byte)'C', vecInput0Upper).ShouldBe((byte)4);
            bla.aa2iSimd((byte)'Q', vecInput0Upper).ShouldBe((byte)5);
            bla.aa2iSimd((byte)'E', vecInput0Upper).ShouldBe((byte)6);
            bla.aa2iSimd((byte)'G', vecInput0Upper).ShouldBe((byte)7);
            bla.aa2iSimd((byte)'H', vecInput0Upper).ShouldBe((byte)8);
            bla.aa2iSimd((byte)'I', vecInput0Upper).ShouldBe((byte)9);
            bla.aa2iSimd((byte)'L', vecInput0Upper).ShouldBe((byte)10);
            bla.aa2iSimd((byte)'K', vecInput0Upper).ShouldBe((byte)11);
            bla.aa2iSimd((byte)'M', vecInput0Upper).ShouldBe((byte)12);
            bla.aa2iSimd((byte)'F', vecInput0Upper).ShouldBe((byte)13);
            bla.aa2iSimd((byte)'P', vecInput0Upper).ShouldBe((byte)14);
            bla.aa2iSimd((byte)'S', vecInput0Upper).ShouldBe((byte)15);
            bla.aa2iSimd((byte)'T', vecInput0Upper).ShouldBe((byte)16);
            bla.aa2iSimd((byte)'W', vecInput0Upper).ShouldBe((byte)17);
            bla.aa2iSimd((byte)'Y', vecInput0Upper).ShouldBe((byte)18);
            bla.aa2iSimd((byte)'V', vecInput0Upper).ShouldBe((byte)19);
            bla.aa2iSimd((byte)'X', vecInput0Upper).ShouldBe(bla.ANY);
            bla.aa2iSimd((byte)'J', vecInput0Upper).ShouldBe(bla.ANY);
            bla.aa2iSimd((byte)'O', vecInput0Upper).ShouldBe(bla.ANY);
            bla.aa2iSimd((byte)'U', vecInput0Upper).ShouldBe((byte)4);
            bla.aa2iSimd((byte)'B', vecInput0Upper).ShouldBe((byte)3);
            bla.aa2iSimd((byte)'Z', vecInput0Upper).ShouldBe((byte)6);
            bla.aa2iSimd((byte)'-', vecInput0Upper).ShouldBe(bla.GAP);
            bla.aa2iSimd((byte)'.', vecInput0Upper).ShouldBe(bla.GAP);
            bla.aa2iSimd((byte)'_', vecInput0Upper).ShouldBe(bla.GAP);

            bla.aa2iSimd((byte)' ', vecInput0Upper).ShouldBe(unchecked((byte)-1));
            bla.aa2iSimd((byte)'{', vecInput0Upper).ShouldBe(unchecked((byte)-2));
        }
    }

    [Fact]
    public void TestAa2iSimdLowercase()
    {
        unsafe
        {
            Vector256<byte> vecInput0Upper = Vector256.Load((byte*)bla.proteinLutUpper);

            bla.aa2iSimd((byte)'a', vecInput0Upper).ShouldBe((byte)0);
            bla.aa2iSimd((byte)'r', vecInput0Upper).ShouldBe((byte)1);
            bla.aa2iSimd((byte)'n', vecInput0Upper).ShouldBe((byte)2);
            bla.aa2iSimd((byte)'d', vecInput0Upper).ShouldBe((byte)3);
            bla.aa2iSimd((byte)'c', vecInput0Upper).ShouldBe((byte)4);
            bla.aa2iSimd((byte)'q', vecInput0Upper).ShouldBe((byte)5);
            bla.aa2iSimd((byte)'e', vecInput0Upper).ShouldBe((byte)6);
            bla.aa2iSimd((byte)'g', vecInput0Upper).ShouldBe((byte)7);
            bla.aa2iSimd((byte)'h', vecInput0Upper).ShouldBe((byte)8);
            bla.aa2iSimd((byte)'i', vecInput0Upper).ShouldBe((byte)9);
            bla.aa2iSimd((byte)'l', vecInput0Upper).ShouldBe((byte)10);
            bla.aa2iSimd((byte)'k', vecInput0Upper).ShouldBe((byte)11);
            bla.aa2iSimd((byte)'m', vecInput0Upper).ShouldBe((byte)12);
            bla.aa2iSimd((byte)'f', vecInput0Upper).ShouldBe((byte)13);
            bla.aa2iSimd((byte)'p', vecInput0Upper).ShouldBe((byte)14);
            bla.aa2iSimd((byte)'s', vecInput0Upper).ShouldBe((byte)15);
            bla.aa2iSimd((byte)'t', vecInput0Upper).ShouldBe((byte)16);
            bla.aa2iSimd((byte)'w', vecInput0Upper).ShouldBe((byte)17);
            bla.aa2iSimd((byte)'y', vecInput0Upper).ShouldBe((byte)18);
            bla.aa2iSimd((byte)'v', vecInput0Upper).ShouldBe((byte)19);
            bla.aa2iSimd((byte)'x', vecInput0Upper).ShouldBe(bla.ANY);
            bla.aa2iSimd((byte)'j', vecInput0Upper).ShouldBe(bla.ANY);
            bla.aa2iSimd((byte)'o', vecInput0Upper).ShouldBe(bla.ANY);
            bla.aa2iSimd((byte)'u', vecInput0Upper).ShouldBe((byte)4);
            bla.aa2iSimd((byte)'b', vecInput0Upper).ShouldBe((byte)3);
            bla.aa2iSimd((byte)'z', vecInput0Upper).ShouldBe((byte)6);
            bla.aa2iSimd((byte)' ', vecInput0Upper).ShouldBe(unchecked((byte)-1));
            bla.aa2iSimd((byte)'{', vecInput0Upper).ShouldBe(unchecked((byte)-2));
        }
    }

    [Fact]
    public void TestAa2iSimdNoSwitchLowercase()
    {
        unsafe
        {
            Vector256<byte> proteinLutUpper2 = Vector256.Load((byte*)bla.proteinLutUpper2);
            var proteinMap = bla.proteinMap;
            bla.aa2iSimdNoSwitch((byte)'a', proteinLutUpper2, proteinMap).ShouldBe((byte)0);
            bla.aa2iSimdNoSwitch((byte)'r', proteinLutUpper2, proteinMap).ShouldBe((byte)1);
            bla.aa2iSimdNoSwitch((byte)'n', proteinLutUpper2, proteinMap).ShouldBe((byte)2);
            bla.aa2iSimdNoSwitch((byte)'d', proteinLutUpper2, proteinMap).ShouldBe((byte)3);
            bla.aa2iSimdNoSwitch((byte)'c', proteinLutUpper2, proteinMap).ShouldBe((byte)4);
            bla.aa2iSimdNoSwitch((byte)'q', proteinLutUpper2, proteinMap).ShouldBe((byte)5);
            bla.aa2iSimdNoSwitch((byte)'e', proteinLutUpper2, proteinMap).ShouldBe((byte)6);
            bla.aa2iSimdNoSwitch((byte)'g', proteinLutUpper2, proteinMap).ShouldBe((byte)7);
            bla.aa2iSimdNoSwitch((byte)'h', proteinLutUpper2, proteinMap).ShouldBe((byte)8);
            bla.aa2iSimdNoSwitch((byte)'i', proteinLutUpper2, proteinMap).ShouldBe((byte)9);
            bla.aa2iSimdNoSwitch((byte)'l', proteinLutUpper2, proteinMap).ShouldBe((byte)10);
            bla.aa2iSimdNoSwitch((byte)'k', proteinLutUpper2, proteinMap).ShouldBe((byte)11);
            bla.aa2iSimdNoSwitch((byte)'m', proteinLutUpper2, proteinMap).ShouldBe((byte)12);
            bla.aa2iSimdNoSwitch((byte)'f', proteinLutUpper2, proteinMap).ShouldBe((byte)13);
            bla.aa2iSimdNoSwitch((byte)'p', proteinLutUpper2, proteinMap).ShouldBe((byte)14);
            bla.aa2iSimdNoSwitch((byte)'s', proteinLutUpper2, proteinMap).ShouldBe((byte)15);
            bla.aa2iSimdNoSwitch((byte)'t', proteinLutUpper2, proteinMap).ShouldBe((byte)16);
            bla.aa2iSimdNoSwitch((byte)'w', proteinLutUpper2, proteinMap).ShouldBe((byte)17);
            bla.aa2iSimdNoSwitch((byte)'y', proteinLutUpper2, proteinMap).ShouldBe((byte)18);
            bla.aa2iSimdNoSwitch((byte)'v', proteinLutUpper2, proteinMap).ShouldBe((byte)19);
            bla.aa2iSimdNoSwitch((byte)'x', proteinLutUpper2, proteinMap).ShouldBe(bla.ANY);
            bla.aa2iSimdNoSwitch((byte)'j', proteinLutUpper2, proteinMap).ShouldBe(bla.ANY);
            bla.aa2iSimdNoSwitch((byte)'o', proteinLutUpper2, proteinMap).ShouldBe(bla.ANY);
            bla.aa2iSimdNoSwitch((byte)'u', proteinLutUpper2, proteinMap).ShouldBe((byte)4);
            bla.aa2iSimdNoSwitch((byte)'b', proteinLutUpper2, proteinMap).ShouldBe((byte)3);
            bla.aa2iSimdNoSwitch((byte)'z', proteinLutUpper2, proteinMap).ShouldBe((byte)6);
            bla.aa2iSimdNoSwitch((byte)' ', proteinLutUpper2, proteinMap).ShouldBe(unchecked((byte)-1));
            bla.aa2iSimdNoSwitch((byte)'{', proteinLutUpper2, proteinMap).ShouldBe(unchecked((byte)-2));
        }
    }

    [Fact]
    public void TestAa2iSimdNoIf()
    {
        unsafe
        {
            Vector256<byte> vecInput0Upper = Vector256.Load((byte*)bla.proteinLutUpper);
            Vector256<byte> vecInput0Lower = Vector256.Load((byte*)bla.proteinLutLower);

            bla.aa2iSimdNoIf((byte)'A', vecInput0Upper, vecInput0Lower).ShouldBe((byte)0);
            bla.aa2iSimdNoIf((byte)'R', vecInput0Upper, vecInput0Lower).ShouldBe((byte)1);
            bla.aa2iSimdNoIf((byte)'N', vecInput0Upper, vecInput0Lower).ShouldBe((byte)2);
            bla.aa2iSimdNoIf((byte)'D', vecInput0Upper, vecInput0Lower).ShouldBe((byte)3);
            bla.aa2iSimdNoIf((byte)'C', vecInput0Upper, vecInput0Lower).ShouldBe((byte)4);
            bla.aa2iSimdNoIf((byte)'Q', vecInput0Upper, vecInput0Lower).ShouldBe((byte)5);
            bla.aa2iSimdNoIf((byte)'E', vecInput0Upper, vecInput0Lower).ShouldBe((byte)6);
            bla.aa2iSimdNoIf((byte)'G', vecInput0Upper, vecInput0Lower).ShouldBe((byte)7);
            bla.aa2iSimdNoIf((byte)'H', vecInput0Upper, vecInput0Lower).ShouldBe((byte)8);
            bla.aa2iSimdNoIf((byte)'I', vecInput0Upper, vecInput0Lower).ShouldBe((byte)9);
            bla.aa2iSimdNoIf((byte)'L', vecInput0Upper, vecInput0Lower).ShouldBe((byte)10);
            bla.aa2iSimdNoIf((byte)'K', vecInput0Upper, vecInput0Lower).ShouldBe((byte)11);
            bla.aa2iSimdNoIf((byte)'M', vecInput0Upper, vecInput0Lower).ShouldBe((byte)12);
            bla.aa2iSimdNoIf((byte)'F', vecInput0Upper, vecInput0Lower).ShouldBe((byte)13);
            bla.aa2iSimdNoIf((byte)'P', vecInput0Upper, vecInput0Lower).ShouldBe((byte)14);
            bla.aa2iSimdNoIf((byte)'S', vecInput0Upper, vecInput0Lower).ShouldBe((byte)15);
            bla.aa2iSimdNoIf((byte)'T', vecInput0Upper, vecInput0Lower).ShouldBe((byte)16);
            bla.aa2iSimdNoIf((byte)'W', vecInput0Upper, vecInput0Lower).ShouldBe((byte)17);
            bla.aa2iSimdNoIf((byte)'Y', vecInput0Upper, vecInput0Lower).ShouldBe((byte)18);
            bla.aa2iSimdNoIf((byte)'V', vecInput0Upper, vecInput0Lower).ShouldBe((byte)19);
            bla.aa2iSimdNoIf((byte)'X', vecInput0Upper, vecInput0Lower).ShouldBe(bla.ANY);
            bla.aa2iSimdNoIf((byte)'J', vecInput0Upper, vecInput0Lower).ShouldBe(bla.ANY);
            bla.aa2iSimdNoIf((byte)'O', vecInput0Upper, vecInput0Lower).ShouldBe(bla.ANY);
            bla.aa2iSimdNoIf((byte)'U', vecInput0Upper, vecInput0Lower).ShouldBe((byte)4);
            bla.aa2iSimdNoIf((byte)'B', vecInput0Upper, vecInput0Lower).ShouldBe((byte)3);
            bla.aa2iSimdNoIf((byte)'Z', vecInput0Upper, vecInput0Lower).ShouldBe((byte)6);
            bla.aa2iSimdNoIf((byte)' ', vecInput0Upper, vecInput0Lower).ShouldBe(unchecked((byte)-1));
            bla.aa2iSimdNoIf((byte)'{', vecInput0Upper, vecInput0Lower).ShouldBe(unchecked((byte)-2));

            bla.aa2iSimdNoIf((byte)'a', vecInput0Upper, vecInput0Lower).ShouldBe((byte)0);
            bla.aa2iSimdNoIf((byte)'r', vecInput0Upper, vecInput0Lower).ShouldBe((byte)1);
            bla.aa2iSimdNoIf((byte)'n', vecInput0Upper, vecInput0Lower).ShouldBe((byte)2);
            bla.aa2iSimdNoIf((byte)'d', vecInput0Upper, vecInput0Lower).ShouldBe((byte)3);
            bla.aa2iSimdNoIf((byte)'c', vecInput0Upper, vecInput0Lower).ShouldBe((byte)4);
            bla.aa2iSimdNoIf((byte)'q', vecInput0Upper, vecInput0Lower).ShouldBe((byte)5);
            bla.aa2iSimdNoIf((byte)'e', vecInput0Upper, vecInput0Lower).ShouldBe((byte)6);
            bla.aa2iSimdNoIf((byte)'g', vecInput0Upper, vecInput0Lower).ShouldBe((byte)7);
            bla.aa2iSimdNoIf((byte)'h', vecInput0Upper, vecInput0Lower).ShouldBe((byte)8);
            bla.aa2iSimdNoIf((byte)'i', vecInput0Upper, vecInput0Lower).ShouldBe((byte)9);
            bla.aa2iSimdNoIf((byte)'l', vecInput0Upper, vecInput0Lower).ShouldBe((byte)10);
            bla.aa2iSimdNoIf((byte)'k', vecInput0Upper, vecInput0Lower).ShouldBe((byte)11);
            bla.aa2iSimdNoIf((byte)'m', vecInput0Upper, vecInput0Lower).ShouldBe((byte)12);
            bla.aa2iSimdNoIf((byte)'f', vecInput0Upper, vecInput0Lower).ShouldBe((byte)13);
            bla.aa2iSimdNoIf((byte)'p', vecInput0Upper, vecInput0Lower).ShouldBe((byte)14);
            bla.aa2iSimdNoIf((byte)'s', vecInput0Upper, vecInput0Lower).ShouldBe((byte)15);
            bla.aa2iSimdNoIf((byte)'t', vecInput0Upper, vecInput0Lower).ShouldBe((byte)16);
            bla.aa2iSimdNoIf((byte)'w', vecInput0Upper, vecInput0Lower).ShouldBe((byte)17);
            bla.aa2iSimdNoIf((byte)'y', vecInput0Upper, vecInput0Lower).ShouldBe((byte)18);
            bla.aa2iSimdNoIf((byte)'v', vecInput0Upper, vecInput0Lower).ShouldBe((byte)19);
            bla.aa2iSimdNoIf((byte)'x', vecInput0Upper, vecInput0Lower).ShouldBe(bla.ANY);
            bla.aa2iSimdNoIf((byte)'j', vecInput0Upper, vecInput0Lower).ShouldBe(bla.ANY);
            bla.aa2iSimdNoIf((byte)'o', vecInput0Upper, vecInput0Lower).ShouldBe(bla.ANY);
            bla.aa2iSimdNoIf((byte)'u', vecInput0Upper, vecInput0Lower).ShouldBe((byte)4);
            bla.aa2iSimdNoIf((byte)'b', vecInput0Upper, vecInput0Lower).ShouldBe((byte)3);
            bla.aa2iSimdNoIf((byte)'z', vecInput0Upper, vecInput0Lower).ShouldBe((byte)6);
            bla.aa2iSimdNoIf((byte)' ', vecInput0Upper, vecInput0Lower).ShouldBe(unchecked((byte)-1));
            bla.aa2iSimdNoIf((byte)'{', vecInput0Upper, vecInput0Lower).ShouldBe(unchecked((byte)-2));
        }
    }

    [Fact]
    public void TestCompressSimdHarold()
    {
        var result = bla.CompressSimdHarold(
                "ARNDCQEGHILKMFPSTWYVXJOUBZ-._ {ARNDCQEGHILKMFPSTWYVXJOUBZ-._ {"u8)
            .ToArray();
        var expected = new sbyte[]
        {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 20, 20, 4, 3, 6, -1, -2, 0, 1, 2,
            3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 20, 20, 4, 3, 6, -1, -2
        };
        expected.ShouldBe(expected);
    }
}