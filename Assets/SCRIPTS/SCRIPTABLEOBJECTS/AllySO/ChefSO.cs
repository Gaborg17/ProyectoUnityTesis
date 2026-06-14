using UnityEngine;
[CreateAssetMenu(fileName = "Chef", menuName = "NPCs/Chef")]
public class ChefSO : AlliesSO
{
    
    [Range(0f, 1f)]
    public float foodCostModifier;

    public override string Description =>
        $"Reduce el costo de comida en un {foodCostModifier * 100f:0}%";

    public override void AllyAbility()
    {
        
    }
}
