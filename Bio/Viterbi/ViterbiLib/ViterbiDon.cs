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

        Dictionary<HealthState, Dictionary<HealthState, double>> transitionProbability = new();
        Dictionary<HealthState, double> transition1 = new()
        {
            [HealthState.Healthy] = 0.7,
            [HealthState.Fever] = 0.3
        };
        Dictionary<HealthState, double> transition2 = new()
        {
            [HealthState.Healthy] = 0.4,
            [HealthState.Fever] = 0.6
        };
        transitionProbability[HealthState.Healthy] = transition1;
        transitionProbability[HealthState.Fever] = transition2;

        Dictionary<HealthState, Dictionary<Observation, double>> emissionProbability = new();
        Dictionary<Observation, double> emission1 = new()
        {
            [Observation.Dizzy] = 0.1,
            [Observation.Cold] = 0.4,
            [Observation.Normal] = 0.5
        };
        Dictionary<Observation, double> emission2 = new()
        {
            [Observation.Dizzy] = 0.6,
            [Observation.Cold] = 0.3,
            [Observation.Normal] = 0.1
        };
        emissionProbability[HealthState.Healthy] = emission1;
        emissionProbability[HealthState.Fever] = emission2;

        var output = ForwardViterbi(observations, startProbability, transitionProbability, emissionProbability);
        return output;
    }

    public static object[] ForwardViterbi(
        Observation[] observations,
        Dictionary<HealthState, double> startProbability,
        Dictionary<HealthState, Dictionary<HealthState, double>> transitionProbability,
        Dictionary<HealthState, Dictionary<Observation, double>> emissionProbability)
    {
        Dictionary<HealthState, object[]> t = new();
        foreach (var state in Enum.GetValues<HealthState>())
            t.Add(state, new object[] { startProbability[state], new[] { state }, startProbability[state] });

        foreach (var observation in observations)
        {
            Dictionary<HealthState, object[]> u = new();
            foreach (var nextState in Enum.GetValues<HealthState>())
            {
                double total = 0;
                HealthState[] argmax = Array.Empty<HealthState>();
                double valmax = 0;

                double prob = 1;
                HealthState[] vPath = Array.Empty<HealthState>();
                double vProb = 1;

                foreach (var sourceState in Enum.GetValues<HealthState>())
                {
                    var objs = t[sourceState];
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

                u.Add(nextState, new object[] { total, argmax, valmax });
            }

            t = u;
        }

        double xTotal = 0;
        HealthState[] xArgMax = Array.Empty<HealthState>();
        double xValMax = 0;

        double xProb;
        HealthState[] xvPath = Array.Empty<HealthState>();
        double xvProb;

        foreach (var healthState in Enum.GetValues<HealthState>())
        {
            var objs = t[healthState];
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