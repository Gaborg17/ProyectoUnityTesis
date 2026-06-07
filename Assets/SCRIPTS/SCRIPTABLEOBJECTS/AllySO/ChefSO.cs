using UnityEngine;
[CreateAssetMenu(fileName = "Chef", menuName = "NPCs/Chef")]
public class ChefSO : AlliesSO
{

    [Range(0f, 1f)]
    public float foodCostModifier;

    public override void AllyAbility()
    {
        
    }
}
