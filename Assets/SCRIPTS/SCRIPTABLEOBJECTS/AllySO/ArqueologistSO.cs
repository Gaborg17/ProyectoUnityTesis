using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Arqueologist", menuName = "NPCs/Arqueologist")]
public class ArqueologistSO : AlliesSO
{
    public List<float> probabilities;
    public override string Description =>
        $"Aumenta la probabilidad de obtener mejores recompensas de los tesoros ";
    public override void AllyAbility()
    {
        
    }


    public override void OnAddedToTeam()
    {
        GameManager.Instance.arqueologist = this;
    }

    public override void OnRemovedOfTeam()
    {
        GameManager.Instance.arqueologist = null;
    }
}
