using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Navigator", menuName = "NPCs/Navigator")]
public class NavigatorSO : AlliesSO
{
    public List<float> probabilities;
    public override string Description =>
        $"Aumenta la probabilidad de encontrar mejores islas";
}
