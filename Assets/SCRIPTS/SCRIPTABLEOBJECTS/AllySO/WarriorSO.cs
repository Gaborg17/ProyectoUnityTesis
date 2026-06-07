using UnityEngine;
[CreateAssetMenu(fileName = "Warrior", menuName = "NPCs/Warrior")]

public class WarriorSO : AlliesSO
{
    [Range(0f,2f)]
    public float damageMultiplier;

}
