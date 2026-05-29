using System.Collections.Generic;
using UnityEngine;

public static class ProbabilityManager
{
    public static int GetRandomIndex(IList<float> weights)
    {
        if(weights == null || weights.Count == 0) return -1;

        float totalweight = 0f;
        for(int i = 0; i < weights.Count; i++)
        {
            totalweight += weights[i];
        }

        float randomRoll = Random.Range(0f, totalweight);
        float cumulativeWeight = 0f;

        for(int i = 0;i < weights.Count; i++)
        {
            cumulativeWeight += weights[i];
            if(randomRoll <= cumulativeWeight)
            {
                return i;
            }
        }

        return weights.Count -1;

    }

}
