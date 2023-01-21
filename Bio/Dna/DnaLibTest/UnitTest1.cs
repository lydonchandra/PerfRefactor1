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
    public void TestAa2i()
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
    }

    [Fact]
    public void TestAa2iSimd1()
    {
        bla.aa2iSimd1((byte)'A').ShouldBe((byte)0);
        bla.aa2iSimd1((byte)'R').ShouldBe((byte)1);
        bla.aa2iSimd1((byte)'N').ShouldBe((byte)2);
        bla.aa2iSimd1((byte)'D').ShouldBe((byte)3);
        bla.aa2iSimd1((byte)'C').ShouldBe((byte)4);
        bla.aa2iSimd1((byte)'Q').ShouldBe((byte)5);
        bla.aa2iSimd1((byte)'E').ShouldBe((byte)6);
        bla.aa2iSimd1((byte)'G').ShouldBe((byte)7);
        bla.aa2iSimd1((byte)'H').ShouldBe((byte)8);
        bla.aa2iSimd1((byte)'I').ShouldBe((byte)9);
        bla.aa2iSimd1((byte)'L').ShouldBe((byte)10);
        bla.aa2iSimd1((byte)'K').ShouldBe((byte)11);
        bla.aa2iSimd1((byte)'M').ShouldBe((byte)12);
        bla.aa2iSimd1((byte)'F').ShouldBe((byte)13);
        bla.aa2iSimd1((byte)'P').ShouldBe((byte)14);
        bla.aa2iSimd1((byte)'S').ShouldBe((byte)15);
        bla.aa2iSimd1((byte)'T').ShouldBe((byte)16);
        bla.aa2iSimd1((byte)'W').ShouldBe((byte)17);
        bla.aa2iSimd1((byte)'Y').ShouldBe((byte)18);
        bla.aa2iSimd1((byte)'V').ShouldBe((byte)19);

        bla.aa2iSimd1((byte)'X').ShouldBe(bla.ANY);
        bla.aa2iSimd1((byte)'J').ShouldBe(bla.ANY);
        bla.aa2iSimd1((byte)'O').ShouldBe(bla.ANY);
        bla.aa2iSimd1((byte)'U').ShouldBe((byte)4);
        bla.aa2iSimd1((byte)'B').ShouldBe((byte)3);
        bla.aa2iSimd1((byte)'Z').ShouldBe((byte)6);
        bla.aa2iSimd1((byte)'-').ShouldBe(bla.GAP);
        bla.aa2iSimd1((byte)'.').ShouldBe(bla.GAP);
        bla.aa2iSimd1((byte)'_').ShouldBe(bla.GAP);
    }
}