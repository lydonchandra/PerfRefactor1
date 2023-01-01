namespace ViterbiLib;

public class ViterbiWiki
{
    //Weather states
    public static readonly string HEALTHY = "Healthy";

    public static readonly string FEVER = "Fever";

    //Dependable actions (observations)
    public static readonly string DIZZY = "dizzy";
    public static readonly string COLD = "cold";
    public static readonly string NORMAL = "normal";

    public static object[] TestForwardViterbi(string[] observations)
    {
        //initialize our arrays of states and observations
        string[] states = { HEALTHY, FEVER };
        // string[] observations = { DIZZY, COLD, NORMAL, DIZZY };

        Dictionary<string, float> start_probability = new();
        start_probability.Add(HEALTHY, 0.6f);
        start_probability.Add(FEVER, 0.4f);
        //Transition probability
        Dictionary<string, Dictionary<string, float>> transition_probability = new();
        Dictionary<string, float> t1 = new();
        t1.Add(HEALTHY, 0.7f);
        t1.Add(FEVER, 0.3f);
        Dictionary<string, float> t2 = new();
        t2.Add(HEALTHY, 0.4f);
        t2.Add(FEVER, 0.6f);
        transition_probability.Add(HEALTHY, t1);
        transition_probability.Add(FEVER, t2);

        //emission_probability
        Dictionary<string, Dictionary<string, float>> emission_probability = new();
        Dictionary<string, float> e1 = new();
        e1.Add(DIZZY, 0.1f);
        e1.Add(COLD, 0.4f);
        e1.Add(NORMAL, 0.5f);

        Dictionary<string, float> e2 = new();
        e2.Add(DIZZY, 0.6f);
        e2.Add(COLD, 0.3f);
        e2.Add(NORMAL, 0.1f);

        emission_probability.Add(HEALTHY, e1);
        emission_probability.Add(FEVER, e2);

        var ret = ForwardViterbi(observations, states, start_probability, transition_probability,
            emission_probability);
        return ret;
        // Console.WriteLine((float)ret[0]);
        // Console.WriteLine((string)ret[1]);
        // Console.WriteLine((float)ret[2]);
        // Console.ReadLine();
    }

    public static object[] ForwardViterbi(string[] obs, string[] states, Dictionary<string, float> start_p,
        Dictionary<string, Dictionary<string, float>> trans_p, Dictionary<string, Dictionary<string, float>> emit_p)
    {
        Dictionary<string, object[]> T = new();
        foreach (var state in states) T.Add(state, new object[] { start_p[state], state, start_p[state] });

        foreach (var observation in obs)
        {
            Dictionary<string, object[]> U = new();

            foreach (var nextState in states)
            {
                float total = 0;
                var argmax = "";
                float valmax = 0;

                float prob = 1;
                var vPath = "";
                float vProb = 1;

                foreach (var source_state in states)
                {
                    var objs = T[source_state];
                    prob = (float)objs[0];
                    vPath = (string)objs[1];
                    vProb = (float)objs[2];

                    var p = emit_p[source_state][observation] * trans_p[source_state][nextState];
                    prob *= p;
                    vProb *= p;
                    total += prob;

                    if (vProb > valmax)
                    {
                        argmax = vPath + "," + nextState;
                        valmax = vProb;
                    }
                }

                U.Add(nextState, new object[] { total, argmax, valmax });
            }

            T = U;
        }

        float xtotal = 0;
        var xargmax = "";
        float xvalmax = 0;

        float xprob;
        string xv_path;
        float xv_prob;

        foreach (var state in states)
        {
            var objs = T[state];
            xprob = (float)objs[0];
            xv_path = (string)objs[1];
            xv_prob = (float)objs[2];

            xtotal += xprob;
            if (xv_prob > xvalmax)
            {
                xargmax = xv_path;
                xvalmax = xv_prob;
            }
        }

        return new object[] { xtotal, xargmax, xvalmax };
    }
}