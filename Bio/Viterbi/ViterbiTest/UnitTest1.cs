using Shouldly;
using ViterbiLib;

namespace ViterbiTest;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var viterbiDon = ViterbiDon.TestForwardViterbi(new[]
            { Observation.Dizzy, Observation.Cold, Observation.Normal, Observation.Dizzy });
        viterbiDon[1].ShouldBe(new[]
            { HealthState.Fever, HealthState.Healthy, HealthState.Healthy, HealthState.Fever, HealthState.Fever });

        var viterbiWiki = ViterbiWiki.TestForwardViterbi(new[]
            { ViterbiWiki.DIZZY, ViterbiWiki.COLD, ViterbiWiki.NORMAL, ViterbiWiki.DIZZY });
        viterbiWiki[1].ShouldBe(
            string.Join(",",
                ViterbiWiki.FEVER, ViterbiWiki.HEALTHY, ViterbiWiki.HEALTHY, ViterbiWiki.FEVER, ViterbiWiki.FEVER));

        viterbiDon = ViterbiDon.TestForwardViterbi(new[]
            { Observation.Dizzy });
        viterbiDon[1].ShouldBe(new[]
            { HealthState.Fever, HealthState.Fever });

        viterbiWiki = ViterbiWiki.TestForwardViterbi(new[]
            { ViterbiWiki.DIZZY });
        viterbiWiki[1].ShouldBe(
            string.Join(",",
                ViterbiWiki.FEVER, ViterbiWiki.FEVER));
    }
}