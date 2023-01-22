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
        bla.aa2iSimd((byte)'A').ShouldBe((byte)0);
        bla.aa2iSimd((byte)'R').ShouldBe((byte)1);
        bla.aa2iSimd((byte)'N').ShouldBe((byte)2);
        bla.aa2iSimd((byte)'D').ShouldBe((byte)3);
        bla.aa2iSimd((byte)'C').ShouldBe((byte)4);
        bla.aa2iSimd((byte)'Q').ShouldBe((byte)5);
        bla.aa2iSimd((byte)'E').ShouldBe((byte)6);
        bla.aa2iSimd((byte)'G').ShouldBe((byte)7);
        bla.aa2iSimd((byte)'H').ShouldBe((byte)8);
        bla.aa2iSimd((byte)'I').ShouldBe((byte)9);
        bla.aa2iSimd((byte)'L').ShouldBe((byte)10);
        bla.aa2iSimd((byte)'K').ShouldBe((byte)11);
        bla.aa2iSimd((byte)'M').ShouldBe((byte)12);
        bla.aa2iSimd((byte)'F').ShouldBe((byte)13);
        bla.aa2iSimd((byte)'P').ShouldBe((byte)14);
        bla.aa2iSimd((byte)'S').ShouldBe((byte)15);
        bla.aa2iSimd((byte)'T').ShouldBe((byte)16);
        bla.aa2iSimd((byte)'W').ShouldBe((byte)17);
        bla.aa2iSimd((byte)'Y').ShouldBe((byte)18);
        bla.aa2iSimd((byte)'V').ShouldBe((byte)19);
        bla.aa2iSimd((byte)'X').ShouldBe(bla.ANY);
        bla.aa2iSimd((byte)'J').ShouldBe(bla.ANY);
        bla.aa2iSimd((byte)'O').ShouldBe(bla.ANY);
        bla.aa2iSimd((byte)'U').ShouldBe((byte)4);
        bla.aa2iSimd((byte)'B').ShouldBe((byte)3);
        bla.aa2iSimd((byte)'Z').ShouldBe((byte)6);
        bla.aa2iSimd((byte)'-').ShouldBe(bla.GAP);
        bla.aa2iSimd((byte)'.').ShouldBe(bla.GAP);
        bla.aa2iSimd((byte)'_').ShouldBe(bla.GAP);

        bla.aa2iSimd((byte)' ').ShouldBe(unchecked((byte)-1));
        bla.aa2iSimd((byte)'{').ShouldBe(unchecked((byte)-2));
    }

    [Fact]
    public void TestAa2iSimdLowercase()
    {
        bla.aa2iSimd((byte)'a').ShouldBe((byte)0);
        bla.aa2iSimd((byte)'r').ShouldBe((byte)1);
        bla.aa2iSimd((byte)'n').ShouldBe((byte)2);
        bla.aa2iSimd((byte)'d').ShouldBe((byte)3);
        bla.aa2iSimd((byte)'c').ShouldBe((byte)4);
        bla.aa2iSimd((byte)'q').ShouldBe((byte)5);
        bla.aa2iSimd((byte)'e').ShouldBe((byte)6);
        bla.aa2iSimd((byte)'g').ShouldBe((byte)7);
        bla.aa2iSimd((byte)'h').ShouldBe((byte)8);
        bla.aa2iSimd((byte)'i').ShouldBe((byte)9);
        bla.aa2iSimd((byte)'l').ShouldBe((byte)10);
        bla.aa2iSimd((byte)'k').ShouldBe((byte)11);
        bla.aa2iSimd((byte)'m').ShouldBe((byte)12);
        bla.aa2iSimd((byte)'f').ShouldBe((byte)13);
        bla.aa2iSimd((byte)'p').ShouldBe((byte)14);
        bla.aa2iSimd((byte)'s').ShouldBe((byte)15);
        bla.aa2iSimd((byte)'t').ShouldBe((byte)16);
        bla.aa2iSimd((byte)'w').ShouldBe((byte)17);
        bla.aa2iSimd((byte)'y').ShouldBe((byte)18);
        bla.aa2iSimd((byte)'v').ShouldBe((byte)19);
        bla.aa2iSimd((byte)'x').ShouldBe(bla.ANY);
        bla.aa2iSimd((byte)'j').ShouldBe(bla.ANY);
        bla.aa2iSimd((byte)'o').ShouldBe(bla.ANY);
        bla.aa2iSimd((byte)'u').ShouldBe((byte)4);
        bla.aa2iSimd((byte)'b').ShouldBe((byte)3);
        bla.aa2iSimd((byte)'z').ShouldBe((byte)6);
        bla.aa2iSimd((byte)' ').ShouldBe(unchecked((byte)-1));
        bla.aa2iSimd((byte)'{').ShouldBe(unchecked((byte)-2));
    }
}