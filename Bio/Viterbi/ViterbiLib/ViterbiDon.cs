using System.Text.Json.Serialization;

namespace ViterbiLib;

// ReSharper disable IdentifierTypo
// ReSharper disable TooWideLocalVariableScope
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HealthState
{
    Healthy,
    Fever
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Observation
{
    Dizzy,
    Cold,
    Normal
}

// // input:
// string[] states = { HEALTHY, FEVER };
// string[] observations = { DIZZY, COLD, NORMAL, DIZZY };
// // output:
// 0.009088801
// Fever,Healthy,Healthy,Fever,Fever
// 0.0014515202
public class ViterbiDon
{
    public static object[] TestForwardViterbi(Observation[] observations)
    {
        // Observation[] observations1 =
        //     { Observation.Dizzy, Observation.Cold, Observation.Normal, Observation.Dizzy };
        Dictionary<HealthState, double> startProbability = new()
        {
            [HealthState.Healthy] = 0.6,
            [HealthState.Fever] = 0.4
        };

        Dictionary<HealthState, Dictionary<HealthState, double>> transitionProbability = new()
        {
            {
                HealthState.Healthy, new Dictionary<HealthState, double>
                {
                    [HealthState.Healthy] = 0.7,
                    [HealthState.Fever] = 0.3
                }
            },
            {
                HealthState.Fever, new Dictionary<HealthState, double>
                {
                    [HealthState.Healthy] = 0.4,
                    [HealthState.Fever] = 0.6
                }
            }
        };

        Dictionary<HealthState, Dictionary<Observation, double>> emissionProbability = new()
        {
            {
                HealthState.Healthy, new Dictionary<Observation, double>
                {
                    [Observation.Dizzy] = 0.1,
                    [Observation.Cold] = 0.4,
                    [Observation.Normal] = 0.5
                }
            },
            {
                HealthState.Fever, new Dictionary<Observation, double>
                {
                    [Observation.Dizzy] = 0.6,
                    [Observation.Cold] = 0.3,
                    [Observation.Normal] = 0.1
                }
            }
        };

        var output = ForwardViterbi(observations, startProbability, transitionProbability, emissionProbability);
        return output;
    }

    public static object[] ForwardViterbi(
        Observation[] observations,
        Dictionary<HealthState, double> startProbability,
        Dictionary<HealthState, Dictionary<HealthState, double>> transitionProbability,
        Dictionary<HealthState, Dictionary<Observation, double>> emissionProbability)
    {
        HealthState[] states = Enum.GetValues<HealthState>();
        Dictionary<HealthState, object[]> viterbi = new();
        foreach (var state in states)
            viterbi.Add(state, new object[] { startProbability[state], new[] { state }, startProbability[state] });

        foreach (var observation in observations)
        {
            Dictionary<HealthState, object[]> viterbiInner = new();
            foreach (var nextState in states)
            {
                double total = 0;
                HealthState[] argmax = Array.Empty<HealthState>();
                double valmax = 0;

                double prob;
                HealthState[] vPath = Array.Empty<HealthState>();
                double vProb = 1;

                foreach (var sourceState in states)
                {
                    var objs = viterbi[sourceState];
                    prob = (double)objs[0];
                    vPath = (HealthState[])objs[1];
                    vProb = (double)objs[2];

                    var p = emissionProbability[sourceState][observation] *
                            transitionProbability[sourceState][nextState];
                    prob *= p;
                    vProb *= p;
                    total += prob;

                    if (vProb > valmax)
                    {
                        argmax = vPath.ToList().Append(nextState).ToArray();
                        valmax = vProb;
                    }
                }

                viterbiInner.Add(nextState, new object[] { total, argmax, valmax });
            }

            viterbi = viterbiInner;
        }

        double xTotal = 0;
        HealthState[] xArgMax = Array.Empty<HealthState>();
        double xValMax = 0;

        double xProb;
        HealthState[] xvPath = Array.Empty<HealthState>();
        double xvProb;

        foreach (var healthState in states)
        {
            var objs = viterbi[healthState];
            xProb = (double)objs[0];
            xvPath = (HealthState[])objs[1];
            xvProb = (double)objs[2];

            xTotal += xProb;
            if (xvProb > xValMax)
            {
                xArgMax = xvPath;
                xValMax = xvProb;
            }
        }

        return new object[] { xTotal, xArgMax, xValMax };
    }
}