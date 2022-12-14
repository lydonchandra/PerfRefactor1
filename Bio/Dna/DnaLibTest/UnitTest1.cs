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
}