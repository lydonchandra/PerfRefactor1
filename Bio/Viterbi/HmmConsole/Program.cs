using System.Text.Json.Serialization;
using CommonUtil;
using static HmmConsole.TextObservation;
using static HmmConsole.TextState;

// ReSharper disable ForCanBeConvertedToForeach

namespace HmmConsole;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TextState
{
    Q0,
    Q1,
    Q2,
    Q3,
    Q4
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TextObservation
{
    The,
    Her,
    My,
    Minas,
    Dog,
    Cat,
    Lizard,
    Unicorn,
    Laughed,
    Ate,
    Slept
}

public class HmmDon
{
    public static void Main()
    {
        TestHmm();
    }

    public static void TestHmm()
    {
        Dictionary<TextState, Dictionary<TextState, double>> transitionProb = new()
        {
            {
                Q0, new Dictionary<TextState, double>
                {
                    { Q1, .7 }, { Q2, .1 }, { Q3, .2 }
                }
            },
            {
                Q1, new Dictionary<TextState, double>
                {
                    { Q1, .1 }, { Q2, .4 }, { Q3, .2 }, { Q4, .3 }
                }
            },
            {
                Q2, new Dictionary<TextState, double>
                {
                    { Q1, .2 }, { Q3, .7 }, { Q4, .1 }
                }
            },
            {
                Q3, new Dictionary<TextState, double>
                {
                    { Q1, .2 }, { Q2, .1 }, { Q3, .1 }, { Q4, .6 }
                }
            }
            // { Q4, new Dictionary<TextState, double>() }
        };

        Dictionary<TextState, Dictionary<TextState, double>> transitionProb1 = new()
        {
            {
                Q0, new Dictionary<TextState, double>
                {
                    { Q1, 1 }, { Q2, .0 }, { Q3, .0 }
                }
            },
            {
                Q1, new Dictionary<TextState, double>
                {
                    { Q1, .0 }, { Q2, 1 }, { Q3, .0 }, { Q4, .0 }
                }
            },
            {
                Q2, new Dictionary<TextState, double>
                {
                    { Q1, .0 }, { Q3, 1 }, { Q4, .0 }
                }
            },
            {
                Q3, new Dictionary<TextState, double>
                {
                    { Q1, .0 }, { Q2, .0 }, { Q3, .0 }, { Q4, 1 }
                }
            }
            // { Q4, new Dictionary<TextState, double>() }
        };

        Dictionary<TextState, Dictionary<TextObservation, double>> emissionProb = new()
        {
            {
                Q1, new Dictionary<TextObservation, double>
                {
                    { The, .3 }, { Her, .1 }, { My, .3 }, { Minas, .3 }
                }
            },
            {
                Q2, new Dictionary<TextObservation, double>
                {
                    { Dog, .2 }, { Cat, .3 }, { Lizard, .1 }, { Unicorn, .4 }
                }
            },
            {
                Q3, new Dictionary<TextObservation, double>
                {
                    { Laughed, .5 }, { Ate, .2 }, { Slept, .3 }
                }
            }
        };

        TextObservation[] texts = GenerateText(transitionProb1, emissionProb);
        texts.Dump();
    }

    public static TextObservation[] GenerateText(
        Dictionary<TextState, Dictionary<TextState, double>> transitionProb,
        Dictionary<TextState, Dictionary<TextObservation, double>> emissionProb
    )
    {
        List<TextObservation> results = new();
        TextState[] states = Enum.GetValues<TextState>();
        var startState = Q0;
        TextState? state = GetNextState(transitionProb, startState);
        while (state.HasValue)
        {
            TextObservation? text = GetText(emissionProb, state.Value);
            if (text.HasValue) results.Add(text.Value);
            state = GetNextState(transitionProb, state.Value);
        }

        return results.ToArray();
    }

    public static TextState? GetNextState
    (Dictionary<TextState, Dictionary<TextState, double>> transitionProb,
        TextState state)
    {
        if (!transitionProb.ContainsKey(state)) return null;
        var random = new Random().NextDouble();
        Dictionary<TextState, double> transition = transitionProb[state];
        TextState[] transitionKeys = transition.Keys.ToArray();
        double totalProb = 0;
        for (var i = 0; i < transitionKeys.Length; i++)
        {
            var key = transitionKeys[i];
            if (!transition.ContainsKey(key)) continue;
            totalProb += transition[key];
            if (random <= totalProb)
            {
                Console.WriteLine("state: " + key);
                return key;
            }
        }

        throw new ApplicationException("Unexpected GetNextState");
    }

    public static TextObservation? GetText(Dictionary<TextState, Dictionary<TextObservation, double>> emissionProb,
        TextState state)
    {
        if (!emissionProb.ContainsKey(state)) return null;
        var random = new Random().NextDouble();
        Dictionary<TextObservation, double> emission = emissionProb[state];
        double totalProb = 0;
        TextObservation[] emissionKeys = emission.Keys.ToArray();
        for (var i = 0; i < emissionKeys.Length; i++)
        {
            var key = emissionKeys[i];
            totalProb += emission[key];
            if (random <= totalProb) return key;
        }

        throw new ApplicationException("Unexpected TextObservation");
    }
}