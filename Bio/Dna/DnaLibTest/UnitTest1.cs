using DnaLib;
using Shouldly;

namespace DnaLibTest;

public class UnitTest1
{
    [Fact]
    public async Task Test0()
    {
        var dna = "acgtatta";
        var isValid = DnaUtil.ValidateDna(dna);
        isValid.ShouldBe(true);
    }

    [Fact]
    public async Task Test1a()
    {
        var path = "gene.fna";
        var valid = await DnaUtil.ValidateDnaFromFile(path);
        valid.ShouldBe(true);
    }

    [Fact]
    public async Task Test2()
    {
        var path = "gene.fna";
        var valid = await DnaUtil.ValidateDnaFromFileAsByte(path);
        valid.ShouldBe(true);
    }
}